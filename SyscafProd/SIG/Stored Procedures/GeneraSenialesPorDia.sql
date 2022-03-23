-- =============================================
-- Author:      Dlopez
-- Create Date: 2021.12.02
-- Description: Genera la ifnormacion de las condiciones de señales
-- =============================================

--EXEC SIG.GeneraSenialesPorDia '20211130', '20211130 23:59:59'

CREATE PROCEDURE SIG.GeneraSenialesPorDia
(
    @Fi DATETIME,
    @Ff DATETIME,
    @Lote INT
)
AS
BEGIN


    -- Eliminamos la información si volvemos a procesar




    DECLARE @MaxId INT =
            (
                SELECT COUNT(TC.clienteIdS)
                FROM dbo.TB_Cliente AS TC
                WHERE TC.estadoClienteIdS = 1
            );
    -- lo vamos a dividir en 3 lotes para ayudar al rendimiento del sistema
    DECLARE @Range INT = CAST((@MaxId / 3) AS INT) + 1;
    DECLARE @clienteids INT;
    DECLARE cmds CURSOR FOR
    SELECT allClients.clienteIdS
    FROM
    (
        SELECT ROW_NUMBER() OVER (ORDER BY TC.clienteIdS) nr,
               TC.clienteIdS
        FROM dbo.TB_Cliente AS TC
        WHERE TC.estadoClienteIdS = 1
              AND TC.clienteIdS <> 915
    ) AS allClients
    WHERE allClients.nr
    BETWEEN (((@Lote - 1) * @Range) + 1) AND (@Lote * @Range);



    --DELETE FROM SIG.TB_DataSeniales
    --WHERE FechaInicial
    --BETWEEN @Fi AND @Ff;

    -- DECLARE @Fi DATETIME = '20211101',
    --        @Ff DATETIME = '20211102';

    DECLARE @Periodo VARCHAR(6) = CAST(MONTH(@Fi) AS VARCHAR(2)) + CAST(YEAR(@Fi) AS VARCHAR(4));
    DECLARE @Sql AS NVARCHAR(MAX);

    DECLARE @CondicionId INT,
            @EventTypeId VARCHAR(MAX),
            @Valor DECIMAL(18, 2),
            @Ocurrencias INT,
            @Distancia DECIMAL(18, 2),
            @Tiempo INT,
            @OperadorId INT,
            @ClienteidsC VARCHAR(MAX),
            @CondTrips VARCHAR(MAX),
            @CondEvent VARCHAR(MAX),
            @FallaSenialId INT,
            @Descripcion VARCHAR(MAX);

    DECLARE @countC INT,
            @cont INT = 1,
            @tablaname VARCHAR(20);

    IF OBJECT_ID('tempdb..#EventsSenales') IS NOT NULL
        DROP TABLE #EventsSenales;
    CREATE TABLE #EventsSenales
    (
        EventTypeId BIGINT,
        assetId BIGINT,
        driverId BIGINT,
        StartDateTime DATETIME,
        TotalTimeSeconds INT,
        Value DECIMAL(11, 4)
    );

    CREATE INDEX idxtempEventsSenales
    ON #EventsSenales (EventTypeId)
    INCLUDE (
                assetId,
                driverId
            );

    IF OBJECT_ID('tempdb..#ViajesSenales') IS NOT NULL
        DROP TABLE #ViajesSenales;
    CREATE TABLE #ViajesSenales
    (
        assetId BIGINT NOT NULL,
        driverId BIGINT NOT NULL,
        TripStart DATETIME NOT NULL,
        TripEnd DATETIME NOT NULL,
        DistanceKilometers DECIMAL(18, 3) NOT NULL,
        maxSpeedKilometersPerHour INT NOT NULL,
        MaxRPM INT NOT NULL,
    );

    DECLARE @groupby NVARCHAR(400);
    SELECT @countC = COUNT(1)
    FROM SIG.TB_Condiciones AS TCT
    WHERE TCT.Clienteids IS NULL;

    -- abrimos cursos
    OPEN cmds;
    WHILE 1 = 1
    BEGIN
        FETCH cmds
        INTO @clienteids;
        IF @@fetch_status != 0
            BREAK;

        --==================================================================================
        -- inicio segundo cursor
        --==================================================================================

        SET @tablaname = @Periodo + '_' + CAST(@clienteids AS VARCHAR);
        SET @cont = 0;

        TRUNCATE TABLE #EventsSenales;
        TRUNCATE TABLE #ViajesSenales;

        SET @Sql
            = N'
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N''[Portal].[TB_Event_' + @tablaname
              + N']'') AND type IN (N''U''))
	INSERT INTO #EventsSenales (EventTypeId, assetId, driverId, StartDateTime, TotalTimeSeconds, Value)
	SELECT EventTypeId, assetId, driverId, StartDateTime, TotalTimeSeconds, Value
	FROM PORTAL.TB_Event_' + @tablaname + N' (NOLOCK)
	WHERE StartDateTime  BETWEEN @fi AND @ff 
	'   ;
        EXEC sp_executesql @Sql, N'@fi datetime, @ff datetime ', @Fi, @Ff;

        SET @Sql
            = N'
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N''[Portal].[TB_Trips_' + @tablaname
              + N']'') AND type in (N''U''))
	INSERT INTO #ViajesSenales (assetId, driverId, TripStart, TripEnd, DistanceKilometers, maxSpeedKilometersPerHour, MaxRPM)
	SELECT  assetId, driverId, TripStart, TripEnd, DistanceKilometers, maxSpeedKilometersPerHour, MaxRPM 
	FROM PORTAL.TB_Trips_' + @tablaname + N'
	WHERE TripStart  BETWEEN @fi AND @ff 
	'   ;
        EXEC sp_executesql @Sql, N'@fi datetime, @ff datetime', @Fi, @Ff;
        PRINT @clienteids;
        WHILE (@cont <= @countC)
        BEGIN

            SELECT @CondicionId = ISNULL(TCT.CondRef, TCT.CondicionId),
                   @EventTypeId = TCT.EventTypeId,
                   @Valor = TCT.Valor,
                   @Ocurrencias = TCT.Ocurrencias,
                   @Distancia = TCT.Distancia,
                   @Tiempo = TCT.Tiempo,
                   @OperadorId = TCT.OperadorId,
                   @ClienteidsC = TCT.Clienteids,
                   @CondTrips = TCT.CondTrips,
                   @CondEvent = TCT.CondEvent,
                   @FallaSenialId = TCT.FallaSenialId,
                   @Descripcion = TCT.Descripcion
            FROM
            (
                SELECT cont = ROW_NUMBER() OVER (ORDER BY TCT.CondicionId),
                       TCT.CondicionId,
                       TCT.TipoValorId,
                       TCT.EventTypeId,
                       TCT.TipoCondicionId,
                       TCT.Valor,
                       TCT.Ocurrencias,
                       TCT.Distancia,
                       TCT.Tiempo,
                       TCT.ValorTrips,
                       TCT.EsActivo,
                       TCT.FechaSistema,
                       TCT.OperadorId,
                       TCT.Clienteids,
                       TCT.FallaSenialId,
                       TCT.Descripcion,
                       TCT.CondTrips,
                       TCT.CondEvent,
                       TCT.CondRef
                FROM SIG.TB_Condiciones AS TCT
                WHERE (
                          TCT.Clienteids IS NULL
                          OR TCT.Clienteids LIKE '%' + CAST(@clienteids AS VARCHAR) + '%'
                      )
                      AND TCT.CondicionId NOT IN
                          (
                              SELECT CondRef
                              FROM SIG.TB_Condiciones AS TC
                              WHERE TC.Clienteids LIKE '%' + CAST(@clienteids AS VARCHAR) + '%'
                                    AND CondRef IS NOT NULL
                          )
            ) AS TCT
            WHERE TCT.cont = @cont;


            -- contador de eventos
            IF (@Ocurrencias IS NOT NULL AND @Distancia IS NULL AND @Valor IS NULL)
            BEGIN
                SET @Sql
                    = N'	  

					INSERT INTO [SIG].[TB_DataSeniales]
						(
							CondicionId,
                            FallaSenialId,	
                            Descripcion,				
							AssetIds,
							ClienteIds,
							FechaInicial,
							FechaFinal,
                            Ocurrencias,
							FechaSistema,
							EsActivo
						)
						SELECT @CondicionId, @FallaSenialId, @Descripcion, TE.assetId, @ClienteIds, 
							   Min = MIN(StartDateTime), Max = MAX(StartDateTime), Ocurrencias = COUNT(1),
                               GETDATE(), 1 
						FROM #EventsSenales AS TE
						WHERE  TE.EventTypeId IN (' + @EventTypeId + N')';

                SET @groupby = N' GROUP BY TE.assetId
						HAVING COUNT(1) > @Ocurrencias';

                IF (@CondEvent IS NOT NULL)
                BEGIN
                    IF (@OperadorId = 55) -- Or
                    BEGIN
                        SET @Sql
                            = @Sql + @groupby
                              + +N'						   
						  UNION
					SELECT @CondicionId, @FallaSenialId, @Descripcion, TE.assetId, @ClienteIds, 
						   Min = MIN(StartDateTime), Max = MAX(StartDateTime), Ocurrencias = 1,
                           GETDATE(), 1 
					FROM #EventsSenales AS TE
					WHERE TE.EventTypeId IN (' + @EventTypeId + N') AND ' + @CondEvent
                              + N' @Tiempo Group by TE.assetId';
                    END;
                    ELSE IF (@OperadorId = 56) -- AND
                    BEGIN
                        SET @Sql = @Sql + N' AND ' + @CondEvent + N' @Tiempo' + @groupby;
                    END;

                END;
                ELSE
                BEGIN
                    SET @Sql = @Sql + @groupby;
                END;

                EXEC sp_executesql @Sql,
                                   N'@Ocurrencias INT , @CondicionId INT ,@ClienteIds INT, @FallaSenialId INT, @Descripcion VARCHAR(MAX), @Tiempo INT',
                                   @Ocurrencias,
                                   @CondicionId,
                                   @clienteids,
                                   @FallaSenialId,
                                   @Descripcion,
                                   @Tiempo;


            END;

            --==================================================================================================================================

            ELSE IF @CondicionId = 1 --- Velocidad Alta
            BEGIN

                SET @Sql
                    = N'	  
        			INSERT INTO [SIG].[TB_DataSeniales]
						(
							CondicionId,
                            FallaSenialId,	
                            Descripcion,				
							AssetIds,
							ClienteIds,
							FechaInicial,
							FechaFinal,
                            Ocurrencias,
							FechaSistema,
							EsActivo
						)
        				SELECT @CondicionId, @FallaSenialId, @Descripcion, TE.assetId, @ClienteIds,
        					   Tripstart, TripEnd, Ocurrencias = 1, GETDATE(), 1
        				FROM #ViajesSenales AS TE
        				WHERE ' + @CondTrips + N'
        				 @Valor
        				';

                IF (@CondEvent IS NOT NULL)
                BEGIN
                    SET @Sql
                        = @Sql
                          + N'						   
        				  UNION
        			SELECT @CondicionId, @FallaSenialId, @Descripcion, TE.assetId, @ClienteIds,
        			       Min = MIN(StartDateTime), Max = MAX(StartDateTime), Ocurrencias = COUNT(1), GETDATE(), 1
        				FROM #EventsSenales AS TE
        				WHERE TE.EventTypeId IN ( ' + @EventTypeId + N' ) GROUP BY TE.assetId ' + @CondEvent
                          + N'
        				 @Ocurrencias
        				';

                END;

                PRINT @Sql;

                EXEC sp_executesql @Sql,
                                   N'@Ocurrencias INT, @CondicionId INT ,@ClienteIds INT, @FallaSenialId INT, @Descripcion VARCHAR(MAX), @Valor INT',
                                   @Ocurrencias,
                                   @CondicionId,
                                   @clienteids,
                                   @FallaSenialId,
                                   @Descripcion,
                                   @Valor;

            END;

            ELSE IF @CondicionId = 11 --- RPM BAJO
            BEGIN

                SET @Sql
                    = N'	  
        			INSERT INTO [SIG].[TB_DataSeniales]
        				(
        					CondicionId,
                            FallaSenialId,	
                            Descripcion,				
        					AssetIds,
        					ClienteIds,
        					FechaInicial,
        					FechaFinal,
                            Ocurrencias,
        					FechaSistema,
        					EsActivo
        				)
        				SELECT @CondicionId, @FallaSenialId, @Descripcion, TE.assetId,
        					   @ClienteIds, Tripstart, TripEnd, Ocurrencias = 1, GETDATE(), 1
        				FROM #ViajesSenales AS TE
                        WHERE maxRPM > @Valor 
                        AND ' + @CondTrips;

                PRINT @Sql;

                EXEC sp_executesql @Sql,
                                   N'@CondicionId INT ,@ClienteIds INT, @Valor INT, @FallaSenialId INT, @Descripcion VARCHAR(MAX)',
                                   @CondicionId,
                                   @clienteids,
                                   @Valor,
                                   @FallaSenialId,
                                   @Descripcion;

            END;

            EXEC SIG.DifVelGPS @CondicionId = @CondicionId,
                               @EventTypeId = @EventTypeId,
                               @Valor = @Valor,
                               @Ocurrencias = @Ocurrencias,
                               @Distancia = @Distancia,
                               @OperadorId = @OperadorId,
                               @Clienteids = @clienteids,
                               @CondTrips = @CondTrips,
                               @CondEvent = @CondEvent,
                               @FallaSenialId = @FallaSenialId,
                               @Descripcion = @Descripcion;

            SET @cont = @cont + 1;

        END;

    --==================================================================================
    -- fin segundo cursor
    --==================================================================================

    END;
    CLOSE cmds;
    DEALLOCATE cmds;


END;
