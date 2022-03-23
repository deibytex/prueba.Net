-- =============================================
-- Author:      Dlopez
-- Create Date: 22.11.2021
-- Description: Relleno recargas
-- Modified: 09.02.2022
-- =============================================
CREATE PROCEDURE [EBUS].[RellenoRecargas30Seg_New]
(
    @Clienteids INT,
    @FormatTime VARCHAR(10),
    @PeriodoFecha DATETIME,
	@Periodoant DATETIME
)
AS
BEGIN

    -- Declaramos eventos a usar segun cliente
    DECLARE @EventSOC BIGINT;
    SET @EventSOC = CASE @Clienteids
                        WHEN 858 THEN
                            1398873819910774944
                        WHEN 914 THEN
                            5397322375965560719
                        ELSE
                            4207737291381775032
                    END;

    DECLARE @EventCorriente BIGINT;
    SET @EventCorriente = CASE @Clienteids
                              WHEN 858 THEN
                                  8658221075967970863
                              WHEN 914 THEN
                                  -8920484449075611863
                              ELSE
                                  3568908874272325220
                          END;

    DECLARE @EventVoltaje BIGINT;
    SET @EventVoltaje = CASE @Clienteids
                            WHEN 858 THEN
                                -8253457625116113070
                            WHEN 914 THEN
                                -4454522314739838940
                            ELSE
                                5140930668286817288
                        END;

    DECLARE @EventCharguin BIGINT;
    SET @EventCharguin = CASE @Clienteids
                             WHEN 858 THEN
                                 6187456050509228463
                             WHEN 914 THEN
                                 353363825776515279
                             ELSE
                                 750817455050850931
                         END;

        -- Creamos tablas para Filtrar los eventos a usar
    IF OBJECT_ID('tempdb..#EventBase') IS NOT NULL
        DROP TABLE #EventBase;
    CREATE TABLE #EventBase
    (
        EventId BIGINT NOT NULL,
        assetId BIGINT NOT NULL,
        driverId BIGINT NOT NULL,
        StartDateTime DATETIME NOT NULL,
        EndDateTime DATETIME NOT NULL
    );

    CREATE INDEX idxEventBase
    ON #EventBase (EventId) INCLUDE (AssetId, DriverID, StartDateTime, EndDateTime);

    -- Creamos tabla temp para evento de carga 30 seg
    
    -- Insertamos Datos del filtro
    INSERT INTO #EventBase
    (
        EventId,
        assetId,
        driverId,
        StartDateTime,
        EndDateTime
    )
    SELECT EV.EventId,
           EV.assetId,
           EV.driverId,
           EV.StartDateTime,
           EV.EndDateTime
    FROM #EventBus AS EV
    WHERE EV.EventTypeId = @EventCharguin
    AND TotalTimeSeconds > 30;                     


    -- CREAMOS TABLA TEMPORAL DE EVENTOS A USAR
    IF OBJECT_ID('tempdb..#TemporaryEventsRec30Sec') IS NOT NULL
        DROP TABLE #TemporaryEventsRec30Sec;
    CREATE TABLE #TemporaryEventsRec30Sec
    (
        EventId BIGINT NOT NULL,
        DriverId BIGINT NOT NULL,
        AssetId BIGINT NOT NULL,
        StartDateTime DATETIME NOT NULL,
        EventIdD BIGINT NOT NULL,
        EventTypeId BIGINT NOT NULL,
        StartDateTimeD DATETIME NOT NULL,
        ValueD DECIMAL(11, 4) NULL
    );

    CREATE INDEX idxtempeventef
    ON #TemporaryEventsRec30Sec (EventTypeId)
    INCLUDE (
                DriverId,
                AssetId
            );

    CREATE INDEX idxtempeventidef
    ON #TemporaryEventsRec30Sec (EventId)
    INCLUDE (
                DriverId,
                AssetId
            );

    CREATE INDEX idxtemeventfechaef
    ON #TemporaryEventsRec30Sec (StartDateTime)
    INCLUDE (
                DriverId,
                AssetId
            );

    -- INSERTAMOS EVENTOS A USAR
    INSERT INTO #TemporaryEventsRec30Sec
    (
        EventId,
        DriverId,
        AssetId,
        StartDateTime,
        EventIdD,
        EventTypeId,
        StartDateTimeD,
        ValueD
    )SELECT 
           EB.EventId,
           EB.driverId,
           EB.assetId,
           EB.StartDateTime,
           ED.EventId,
           ED.EventTypeId,
           ED.StartDateTime,
           ED.Value
    FROM #EventBase AS EB
        INNER JOIN
        (
            SELECT *
            FROM #EventBus AS TE
            WHERE TE.EventTypeId IN ( @EventCorriente, @EventSOC, @EventVoltaje )
        ) AS ED
            ON ED.StartDateTime BETWEEN EB.StartDateTime AND EB.EndDateTime
               AND ED.assetId = EB.assetId
               AND ED.driverId = EB.DriverId;      


       
    -- REPORTE RECARGAS 30 SEG
    DECLARE @SQLScript NVARCHAR(MAX)
        = N'
    WITH consultaFinal (EventId, Muestra, NoCarga, AssetId, Movil, Fecha, Hora, FechaHora, SOC, Corriente, Voltaje, 
                        fechasistema, EsProcesado
                    )
AS (
    SELECT
        EventId  = MAX(   CASE
                                WHEN @EventSOC = ER.EventTypeId THEN
                                    ER.EventIdD
                                ELSE
                                    0
                            END
                        ),
        (ISNULL(
                       (
                           SELECT MAX(E.Muestra)
                           FROM EBUS.Recargas30seg_' + CAST(@Clienteids AS VARCHAR)
          + N' AS E                          
                        ),
                       0
                        ) + 
                       (
                           ROW_NUMBER()OVER(ORDER BY ER.StartDateTime) 
                        )
                ) AS Muestra,
        NC.carga AS NoCarga,            
        A.assetId AS AssetId,
        A.Placa AS Movil,
        CAST(ER.StartDateTimeD AS DATE) AS Fecha,
        CAST(ER.StartDateTimeD AS TIME) AS Hora,
        ER.StartDateTimeD AS FechaHora,        
        SOC = SUM(   CASE
                                WHEN @EventSOC = ER.EventTypeId THEN
                                    ER.ValueD
                                ELSE
                                    NULL
                            END
                        ),
        Corriente = SUM(   CASE
                                WHEN @EventCorriente = ER.EventTypeId THEN
                                    ER.ValueD
                                ELSE
                                    NULL
                            END
                        ),
        Voltaje = SUM(   CASE
                                WHEN @EventVoltaje = ER.EventTypeId THEN
                                    ER.ValueD
                                ELSE
                                    0
                            END
                        ),
        GETDATE() AS fechasistema,
        0 AS EsProcesado
    FROM #TemporaryEventsRec30Sec AS ER
    INNER JOIN
        ( 
        SELECT
            (ISNULL(
                       (SELECT MAX(E.NoCarga)
            FROM EBUS.Recargas30seg_' + CAST(@Clienteids AS VARCHAR)
          + N' AS E
            WHERE E.assetId = TE.assetId                     
                       ),
                       0
                             ) + 
                       (ROW_NUMBER()OVER(PARTITION BY TE.assetId ORDER BY TE.StartDateTime))
        ) AS carga,
        TE.EventId
        FROM #EventBase AS TE) AS NC            
        ON (NC.EventId = ER.EventId)
        INNER JOIN #Assets AS A ON (A.assetId = ER.assetId)
        GROUP BY 
         ER.assetId,
         A.assetId,
         A.Placa,
         NC.carga,
         ER.StartDateTime,
         ER.StartDateTimeD
        )

      
        SELECT  EventId,
                Muestra,
                NoCarga,
                AssetId,
                Movil,
                Fecha,
                Hora,
                FechaHora,
                SOC,
                Corriente,
                Voltaje,
                fechasistema,
                EsProcesado
        FROM consultaFinal AS EB
 
        ORDER BY Muestra;
        ';

    -- Ejecutamos el Scritp

    -- Ejecutamos el script con sus variables declaradas
    EXEC sp_executesql @SQLScript,
                       N'@FormatTime AS VARCHAR(10), @EventCharguin BIGINT, @EventSOC BIGINT, @EventCorriente BIGINT, @EventVoltaje BIGINT, @PeriodoFecha DATETIME',
                       @FormatTime,
                       @EventCharguin,
                       @EventSOC,
                       @EventCorriente,
                       @EventVoltaje,
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
	-- 	update [PORTAL].[TB_Event_' + @PeriodoW + N'_' + CAST(@Clienteids AS VARCHAR)
    --           + N']  set EsProcesado = 1
	-- 	  where EventId in (
	-- 	  SELECT EV.EventIdD FROM #TemporaryEventsRec30Sec AS EV
	-- 	  )';

    --     -- Ejecutamos el Scritp
    --     EXEC sp_executesql @SQLScript;

	-- 	SET @SQLScript
    --         = N'
	-- 	update [PORTAL].[TB_Event_' + @PeriodoW + N'_' + CAST(@Clienteids AS VARCHAR)
    --           + N']  set EsProcesado = 1
	-- 	  where EventId in (
	-- 	  SELECT top 1000 EV.EventId FROM #EventBus AS EV
	-- 	  where EventTypeId = @EventCharguin  and StartDateTime <= dateadd(day, -2, getdate())
	-- 	  ) ';

    --     -- Ejecutamos el Scritp
    --     EXEC sp_executesql @SQLScript, N'@EventCharguin BIGINT',  @EventCharguin ;

    --     SET @PeriodoantEvento = DATEADD(MONTH, 1, @PeriodoantEvento);

    -- END;

END;
