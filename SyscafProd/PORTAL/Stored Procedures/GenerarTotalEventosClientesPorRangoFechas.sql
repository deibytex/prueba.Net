-- =============================================
-- Author:      ygonzalez
-- Create Date: 22.12.2021
-- Description: Genera la informacion diaraia de eventos para base de img y fidelizacion bavaria por rango de fechas
-- =============================================
--EXEC PORTAL.GenerarTotalEventosClientesPorRangoFechas 834,'20211201','20211217'



CREATE PROCEDURE PORTAL.GenerarTotalEventosClientesPorRangoFechas
(
    @ClienteId INT,
    @FechaInicial DATETIME,
    @FechaFinal DATETIME
)
AS
BEGIN
    --DECLARE @ClienteId INT = 834;  
    --DECLARE @FechaGeneracion DATE = '20211001';

    DECLARE @FechaInicio DATETIME = @FechaInicial;
    -- DECLARE @FechaFinal DATETIME = DATEADD(DAY, 1, @FechaGeneracion);
    DECLARE @ClienteVarchar VARCHAR(10) = CAST(@ClienteId AS VARCHAR);

    EXEC PORTAL.CreateTableTotalEventByCliente @ClienteVarchar;
    -- eliminamos la infromacion

    DECLARE @SQLScript NVARCHAR(MAX),
            @PeriodoW VARCHAR(7);
    SET @SQLScript
        = N' DELETE FROM PORTAL.TotalEventos_' + CAST(@ClienteId AS VARCHAR)
          + N' WHERE FechaHora
				BETWEEN @FechaI AND @FechaF';

    PRINT @SQLScript;

    -- Ejecutamos el Scritp
    EXEC sp_executesql @SQLScript,
                       N'@FechaI datetime, @FechaF DateTime',
                       @FechaInicio,
                       @FechaFinal;


    -- elimina las distancias recorridas para el cliente cuando se genera
    SET @SQLScript
        = N' DELETE FROM PORTAL.TotalDistanciaRecorrida_' + CAST(@ClienteId AS VARCHAR)
          + N' WHERE FechaHora
				BETWEEN @FechaI AND @FechaF';

    PRINT @SQLScript;

    -- Ejecutamos el Scritp
    EXEC sp_executesql @SQLScript,
                       N'@FechaI datetime, @FechaF DateTime',
                       @FechaInicio,
                       @FechaFinal;

    --===================================================================
    -- DECLARACION DE VARIABLES


    -- Creamos tablas paras los eventos generados
    IF OBJECT_ID('tempdb..#TablePortalEventIMG') IS NOT NULL
        DROP TABLE #TablePortalEventIMG;
    CREATE TABLE #TablePortalEventIMG
    (
        EventId BIGINT NOT NULL,
        assetId BIGINT NOT NULL,
        driverId BIGINT NOT NULL,
        EventTypeId BIGINT NOT NULL,
        TotalOcurrance INT NOT NULL,
        TotalTimeSeconds INT NOT NULL,
        StartDateTime DATETIME NOT NULL,
        EndDateTime DATETIME NULL
    );

    DECLARE @SitesGerencias AS TABLE
    (
        Tipo INT,
        SiteId BIGINT
    );
    CREATE INDEX idxtempeventypes_portal_temp
    ON #TablePortalEventIMG (EventTypeId);

    -- sacamos el periodo para traer la informaci[on de las tablas
    SET @PeriodoW = CAST(DATEPART(MONTH, @FechaInicio) AS VARCHAR) + CAST(DATEPART(YEAR, @FechaInicio) AS VARCHAR);





    DECLARE @eventos VARCHAR(MAX)
        =
            (
                SELECT SUBSTRING(
                                    (
                                        SELECT ',' + CAST(CEI.EventTypeId AS VARCHAR) AS [text()]
                                        FROM PORTAL.ConfiguracionEventosIMG AS CEI
                                        WHERE CEI.ClienteIds = 834
                                        FOR XML PATH(''), TYPE
                                    ).value('text()[1]', 'nvarchar(max)'),
                                    2,
                                    1000
                                ) Students
            );


    -- SCRIPT PARA LLENAR EVENTOS DETALLE
    SET @SQLScript
        = N'
				INSERT INTO #TablePortalEventIMG
				(EventId
					,assetId
					,driverId
					,EventTypeId
					,TotalOcurrance
					,StartDateTime
					,EndDateTime
					,TotalTimeSeconds
				)
				SELECT 
					EventId
					, assetId
					, driverId
					, EventTypeId
					, TotalOccurances
					, StartDateTime
					, EndDateTime
					,TotalTimeSeconds
				FROM [PORTAL].[TB_Event_' + @PeriodoW + N'_' + CAST(@ClienteId AS VARCHAR)
          + N'] WHERE (  StartDateTime between @FechaI and  @FechaF )		  
				AND EventTypeID IN (' + @eventos + N')			
				';

    PRINT @SQLScript;



    -- Ejecutamos el Scritp
    EXEC sp_executesql @SQLScript,
                       N'@FechaI datetime, @FechaF DateTime',
                       @FechaInicio,
                       @FechaFinal;



    SET @SQLScript
        = N'INSERT INTO PORTAL.TotalEventos_' + CAST(@ClienteId AS VARCHAR)
          + N'
			(
				FechaHora,
				AssetId,
				DriverId,
				EventTypeId,
				Ocurrencias,
				Tiempo,
				FechaSistema
			)
			SELECT CAST(TE.StartDateTime AS DATE) Fecha,
				   TE.assetId,
				   TE.driverId,
				   TE.EventTypeId,
				   SUM(TE.TotalOcurrance),
				    SUM(TE.TotalTimeSeconds),
				   DATEADD(HOUR, -5, GETDATE())
			FROM #TablePortalEventIMG AS TE
			GROUP BY CAST(TE.StartDateTime AS DATE),
					 TE.EventTypeId,
					 TE.assetId,
					 TE.driverId;
				';

    -- Ejecutamos el Scritp
    EXEC sp_executesql @SQLScript;

    SET @SQLScript
        = N'INSERT INTO PORTAL.TotalDistanciaRecorrida_' + CAST(@ClienteId AS VARCHAR)
          + N'
			(
				 FechaHora,
				AssetId,
				DriverId,
				Distancia,
				FechaSistema
			)
				SELECT  FechaHora =CAST(TT.tripStart AS DATE),
			TT.assetId, 
			TT.driverId,
			SUM(TT.distanceKilometers) ,  
			DATEADD(HOUR, -5, GETDATE())
			FROM PORTAL.TB_Trips_' + @PeriodoW + N'_' + CAST(@ClienteId AS VARCHAR)
					  + N' AS TT
			WHERE TT.tripStart BETWEEN @FechaI AND  @FechaF
			GROUP BY TT.assetId, TT.driverId, CAST(TT.tripStart AS DATE)
		
				';

    -- Ejecutamos el Scritp
    EXEC sp_executesql @SQLScript,
                       N'@FechaI datetime, @FechaF DateTime',
                       @FechaInicio,
                       @FechaFinal;


END;
