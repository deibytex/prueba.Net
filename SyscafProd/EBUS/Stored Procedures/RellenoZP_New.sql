-- =============================================
-- Author:      Dlopez
-- Create Date: 22.11.2021
-- Description: 
-- Update: 25/02/2022
-- =============================================
	
CREATE PROCEDURE [EBUS].[RellenoZP_New]
(
    @Clienteids INT,
    @FormatTime VARCHAR(10),
    @PeriodoFecha DATETIME,
	@Periodoant DATETIME
)
AS
BEGIN

    -- Creamos tablas para Filtrar los eventos a usar
    IF OBJECT_ID('tempdb..#FilterZP') IS NOT NULL
        DROP TABLE #FilterZP;
    CREATE TABLE #FilterZP
    (
        EventTypeId BIGINT NOT NULL
    );

    CREATE INDEX idxFilterZP ON #FilterZP (EventTypeId);

    -- Insertamos datos eventos filtro
    INSERT INTO #FilterZP
    (
        EventTypeId
    )
    SELECT EventTypeId
    FROM #EventTypes
    WHERE EventTypeId IN
          (
              SELECT TPDW.EventTypeId
              FROM dbo.TB_PreferenciasDescargarWS AS TPDW
              WHERE TPDW.TipoPreferencia = 7
                    AND TPDW.Parametrizacion LIKE '%78%'
          );

    -- Creamo tablas temporales necesarias
    IF OBJECT_ID('tempdb..#TemporaryEventsZP') IS NOT NULL
        DROP TABLE #TemporaryEventsZP;
    CREATE TABLE #TemporaryEventsZP
    (
        TripId BIGINT,
        driverId BIGINT,
        AssetId BIGINT,
        tripStart DATETIME NOT NULL,
        tripEnd DATETIME NOT NULL,
        TotalTimeSeconds INT NULL,
        EventId BIGINT,
        EventTypeId BIGINT,
    );

    CREATE INDEX idxtempeventZP
    ON #TemporaryEventsZP (EventTypeId)
    INCLUDE (
                driverId,
                assetId
            );
    CREATE INDEX idxtempeventidZP
    ON #TemporaryEventsZP (EventId)
    INCLUDE (
                driverId,
                assetId
            );
    CREATE INDEX idxtemeventfechaZP
    ON #TemporaryEventsZP (tripStart)
    INCLUDE (
                driverId,
                assetId
            );

    DECLARE @Potencia5 BIGINT;
    SET @Potencia5 = -4208106525102400235;

    --Insertamos datod de eventos a usar 
    INSERT INTO #TemporaryEventsZP
    (
        TripId,
        driverId,
        assetId,
        tripStart,
        tripEnd,
        TotalTimeSeconds,
        EventId,
        EventTypeId
    )
    SELECT T.TripId,
           T.driverId,
           T.assetId,
           T.tripStart,
           T.tripEnd,
           ED.TotalTimeSeconds,
           ED.EventId,
           ED.EventTypeId
    FROM #Trips AS T
        INNER JOIN
        (
            SELECT EV.EventId,
                   EV.EventTypeId,
                   EV.assetId,
                   EV.driverId,
                   EV.StartDateTime,
                   EV.TotalTimeSeconds
            FROM #EventBus AS EV
            WHERE EV.EventTypeId IN
                  (
                      SELECT EventTypeId FROM #FilterZP
                  )
        ) AS ED
            ON (
                   ED.StartDateTime
               BETWEEN T.tripStart AND T.tripEnd
                   AND ED.assetId = T.assetId
                   AND ED.driverId = T.driverId
               );

    -- Insertamos data y ejecutamos script a las distintas tablas reporte
    -- REPORTE ZP
    DECLARE @SQLScript NVARCHAR(MAX)
        = N'
        WITH consultaFinal (EventId, Movil, Operador, Fecha, Inicio, Fin, Descripcion, Duracion, 
                    DuracionHora, fechasistema, EsProcesado
                   )
AS (SELECT
        EB.EventId,
        A.Placa AS Movil,
        D.DriverName AS Operador,
        CAST(EB.tripStart AS DATE) AS Fecha,
        EB.tripStart AS Inicio,
        EB.tripEnd AS Fin,
        E.descriptionEvent AS Descripcion,
        CONVERT(VARCHAR,(DATEADD(SECOND,SUM(EB.TotalTimeSeconds),@FormatTime)),8) AS Duracion,
        ROUND((SUM(EB.TotalTimeSeconds) / 3600.0),1) AS DuracionHora,
        GETDATE() AS fechasistema,
        0 AS EsProcesado
    FROM #TemporaryEventsZP AS EB
        INNER JOIN #Drivers AS D ON (D.DriverID = EB.driverId)
        INNER JOIN #Assets AS A ON (A.assetId = EB.assetId)
        INNER JOIN #EventTypes AS E ON (E.eventTypeId = EB.EventTypeId)
    GROUP BY
       EB.EventId, 
       A.Placa,
       D.DriverName,
       EB.tripStart,
       EB.tripEnd,
       E.descriptionEvent)
    
 
     SELECT
        EventId, 
        Movil, 
        Operador, 
        Fecha, 
        Inicio, 
        Fin, 
        Descripcion, 
        Duracion, 
        DuracionHora, 
        fechasistema, 
        EsProcesado
     FROM consultaFinal AS EB
    ;   ';

    -- Ejecutamos el script con sus variables declaradas
    EXEC sp_executesql @SQLScript, N'@FormatTime AS VARCHAR(10), @PeriodoFecha DATETIME', 
                                    @FormatTime,
                                    @PeriodoFecha;

    -- -- Script para marcar los eventos Usados
    -- DECLARE @PeriodoW VARCHAR(7),
    --         @PeriodoantEvento DATETIME = @Periodoant;

    -- WHILE @PeriodoantEvento <= @PeriodoFecha
    -- BEGIN

    --     SET @PeriodoW
    --         = CAST(DATEPART(MONTH, @PeriodoantEvento) AS VARCHAR) + CAST(DATEPART(YEAR, @PeriodoantEvento) AS VARCHAR);

    --     SET @SQLScript
    --         = N'
	-- 	UPDATE [PORTAL].[TB_Event_' + @PeriodoW + N'_' + CAST(@Clienteids AS VARCHAR)
    --           + N']  SET EsProcesado = 1
	-- 	  WHERE EventId IN (
	-- 	  SELECT EV.EventIdD FROM #TemporaryEventsZP AS EV
	-- 	  )';

    --     -- Ejecutamos el Scritp
    --    EXEC sp_executesql @SQLScript;

	-- 	 SET @SQLScript
    --         = N'
	-- 	 UPDATE [PORTAL].[TB_Trips_' + @PeriodoW + N'_' + CAST(@Clienteids AS VARCHAR)
    --           + N']  SET EsProcesado = 1
	-- 	  WHERE TripId IN (
	-- 	  SELECT TOP 1000 EV.TripId FROM #Trips AS EV
	-- 	  WHERE tripStart <= dateadd(day, -2, getdate())
	-- 	  ) ';

    --     -- Ejecutamos el Scritp
    --     EXEC sp_executesql @SQLScript;

    --     SET @PeriodoantEvento = DATEADD(MONTH, 1, @PeriodoantEvento);

    -- END;

END;
