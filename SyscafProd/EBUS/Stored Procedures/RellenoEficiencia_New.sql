



---- =============================================
---- Author:      Dlopez
---- Create Date: 22.11.2021
---- Description: Rellena eficiencia
---- Modified: 16/02/2022
---- =============================================
CREATE PROCEDURE [EBUS].[RellenoEficiencia_New]
(
    @Clienteids INT,
    @FormatTime VARCHAR(10),
    @PeriodoFecha DATETIME,
	@Periodoant DATETIME
)
AS
BEGIN

    -- Declaramos eventos a usar segun cliente
    DECLARE @eventDescargaMAX BIGINT;
    SET @eventDescargaMAX = -1819433749460030902;

    DECLARE @eventCargaMAX BIGINT;
    SET @eventCargaMAX = 8455574973419028357;

    DECLARE @eventDescargaMIN BIGINT;
    SET @eventDescargaMIN = -3850842164247107865;

    DECLARE @eventCargaMIN BIGINT;
    SET @eventCargaMIN = -6179005433367454889;

    DECLARE @eventMaxSOC BIGINT;
    SET @eventMaxSOC = -3810239100821492539;

    DECLARE @eventMinSOC BIGINT;
    SET @eventMinSOC = 8302289603356781339;

    -- CREAMOS TABLA TEMPORAL CON EVENTOW A USAR
    IF OBJECT_ID('tempdb..#TemporaryEventsEf') IS NOT NULL
        DROP TABLE #TemporaryEventsEf;
    CREATE TABLE #TemporaryEventsEf
    (
        TripId BIGINT,
        driverId BIGINT,
        assetId BIGINT,
        tripStart DATETIME NOT NULL,
        tripEnd DATETIME NOT NULL,
        Duracion INT NULL,
        distanceKilometers DECIMAL(18, 4) NULL,
        StartOdometerKilometers DECIMAL(18, 4) NULL,
        endOdometerKilometers DECIMAL(18, 4) NULL,
        EventId BIGINT,
        EventTypeId BIGINT,
        Value DECIMAL(18, 4) NULL,
        StartOdometerKilometres DECIMAL(18, 4) NULL
    );

    CREATE INDEX idxtempeventef
    ON #TemporaryEventsEf (EventTypeId)
    INCLUDE (
                driverId,
                assetId
            );
    CREATE INDEX idxtempeventidef
    ON #TemporaryEventsEf (EventId)
    INCLUDE (
                driverId,
                assetId
            );
    CREATE INDEX idxtemeventfechaef
    ON #TemporaryEventsEf (TripId)
    INCLUDE (
                driverId,
                assetId,
                tripStart,
                tripEnd
            );

    -- INSERTAMOS DATOS EN LA TABLE TEMPORAL
    INSERT INTO #TemporaryEventsEf
    (
        TripId,
        driverId,
        assetId,
        tripStart,
        tripEnd,
        Duracion,
        distanceKilometers,
        StartOdometerKilometers,
        endOdometerKilometers,
        EventId,
        EventTypeId,
        Value
    )
    SELECT T.TripId,
           T.driverId,
           T.assetId,
           T.tripStart,
           T.tripEnd,
           T.Duracion,
           T.distanceKilometers,
           T.StartOdometerKilometers,
           T.endOdometerKilometers,
           ED.EventId,
           ED.EventTypeId,
           ED.Value
    FROM #Trips AS T
        INNER JOIN
        (
            SELECT *
            FROM #EventBus AS TE
            WHERE TE.EventTypeId IN ( @eventDescargaMAX, @eventCargaMAX, @eventMaxSOC, @eventMinSOC )
                  AND TE.Value <> 0
        ) AS ED
            ON ED.StartDateTime
               BETWEEN T.tripStart AND T.tripEnd
               AND ED.assetId = T.assetId
               AND ED.driverId = T.driverId;



    -- Insertamos data y ejecutamos script a las distintas tablas reporte
    -- REPORTE EFICIENCIA    
    DECLARE @SQLScript NVARCHAR(MAX)
        = N'
WITH consultaFinal (TripId, Movil, Operador, Fecha, Inicio, Fin, Carga, Descarga, Distancia, Duracion, DuracionHora,
                    TotalConsumo, MaxSOC, MinSOC, DSOC, fechasistema, EsProcesado, StartOdometerKilometers,
                    endOdometerKilometers
                   )
AS (SELECT TE.TripId,
           Movil = A.registrationNumber,
           Operador = D.name,
           Fecha = CAST(TE.tripStart AS DATE),
           Inicio = TE.tripStart,
           Fin = TE.tripEnd,
           Carga = SUM(   CASE
                              WHEN @eventCargaMAX = TE.EventTypeId THEN
                                  TE.Value
                              ELSE
                                  0
                          END
                      ) 
                      - 
                   SUM(   CASE
                            WHEN @eventCargaMIN = TE.EventTypeId THEN
                              TE.Value
                            ELSE
                                0
                          END
                      ),
           Descarga = SUM(   CASE
                              WHEN @eventDescargaMAX = TE.EventTypeId THEN
                                  TE.Value
                              ELSE
                                  0
                          END
                      )
                      - 
                      SUM(   CASE
                            WHEN @eventDescargaMIN = TE.EventTypeId THEN
                              TE.Value
                            ELSE
                                0
                          END
                      ),
           Distancia = TE.distanceKilometers,
           Duracion = CONVERT(VARCHAR, (DATEADD(SECOND, TE.Duracion, @FormatTime)), 8),
           DuracionHora = ROUND((TE.Duracion / 3600.0), 1),
           TotalConsumo = (SUM(   CASE
                              WHEN @eventDescargaMAX = TE.EventTypeId THEN
                                  TE.Value
                              ELSE
                                  0
                          END
                      )
                      - 
                      SUM(   CASE
                            WHEN @eventDescargaMIN = TE.EventTypeId THEN
                              TE.Value
                            ELSE
                                0
                          END
                      ))
                      -
                      (SUM(   CASE
                              WHEN @eventCargaMAX = TE.EventTypeId THEN
                                  TE.Value
                              ELSE
                                  0
                          END
                      ) 
                      - 
                   SUM(   CASE
                            WHEN @eventCargaMIN = TE.EventTypeId THEN
                              TE.Value
                            ELSE
                                0
                          END
                      )),
           MaxSOC = MAX(CASE
                            WHEN @eventMaxSOC = TE.EventTypeId THEN
                                TE.Value
                            ELSE
                                0
                        END),
           MinSOC = MIN(CASE
                            WHEN @eventMinSOC = TE.EventTypeId THEN
                                TE.Value
                            ELSE
                                NULL
                        END),
           DSOC = (
                    MAX(CASE
                            WHEN @eventMaxSOC = TE.EventTypeId THEN
                                TE.Value
                            ELSE
                                0
                            END) 
                   - 
                    MIN(CASE
                            WHEN @eventMinSOC = TE.EventTypeId THEN
                                TE.Value
                            ELSE
                                NULL
                            END)
                  ),
           fechasistema = GETDATE(),
           EsProcesado = 0,
           StartOdometerKilometers = TE.StartOdometerKilometers,
           endOdometerKilometers = TE.endOdometerKilometers
    FROM #TemporaryEventsEf AS TE
        INNER JOIN PORTAL.TB_Drivers AS D
            ON (D.DriverId = TE.driverId)
        INNER JOIN dbo.TB_Assets AS A
            ON (A.AssetId = TE.assetId)
    GROUP BY TE.TripId,
             A.registrationNumber,
             D.name,
             TE.tripStart,
             TE.tripEnd,
             TE.Duracion,
             TE.StartOdometerKilometers,
             TE.endOdometerKilometers,
             TE.distanceKilometers)


SELECT TripId,
       Movil,
       Operador,
       Fecha,
       Inicio,
       Fin,
       Carga,
       Descarga,
       Distancia,
       Duracion,
       DuracionHora,
       TotalConsumo,
       MaxSOC,
       MinSOC,
       DSOC,
       fechasistema,
       EsProcesado,
       StartOdometerKilometers,
       endOdometerKilometers
FROM consultaFinal AS EB
;';

    -- Ejecutamos el script con sus variables declaradas
    EXEC sp_executesql @SQLScript,
                       N'@FormatTime AS VARCHAR(10), @eventCargaMAX BIGINT, @eventMaxSOC BIGINT, 
                       @eventMinSOC BIGINT, @eventDescargaMAX BIGINT, @PeriodoFecha DATETIME,
                       @eventDescargaMIN BIGINT, @eventCargaMIN BIGINT',
                       @FormatTime,
                       @eventCargaMAX,
                       @eventMaxSOC,
                       @eventMinSOC,
                       @eventDescargaMAX,
                       @PeriodoFecha,
                       @eventDescargaMIN,
                       @eventCargaMIN;


    -- -- Script para marcar los eventos Usados
    -- DECLARE @PeriodoW VARCHAR(7),
    --         @PeriodoantEvento DATETIME = @Periodoant;

    -- WHILE @PeriodoantEvento <= @PeriodoFecha
    -- BEGIN

    --     SET @PeriodoW
    --         = CAST(DATEPART(MONTH, @PeriodoantEvento) AS VARCHAR) + CAST(DATEPART(YEAR, @PeriodoantEvento) AS VARCHAR);

    --     SET @SQLScript
    --         = N'
	-- UPDATE [PORTAL].[TB_Event_' + @PeriodoW + N'_' + CAST(@Clienteids AS VARCHAR)
    --           + N']  SET EsProcesado = 1
	--   WHERE EventId IN (
	--   SELECT EV.EventId FROM #TemporaryEventsEf AS EV
	--   )';

    --     -- Ejecutamos el Scritp
    --     EXEC sp_executesql @SQLScript;

    --     SET @PeriodoantEvento = DATEADD(MONTH, 1, @PeriodoantEvento);

    -- END;

END;
