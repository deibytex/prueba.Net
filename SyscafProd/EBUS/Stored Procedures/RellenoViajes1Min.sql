-- =============================================
-- Author:      Dlopez
-- Create Date: 22.11.2021
-- Description: 
-- =============================================

CREATE PROCEDURE EBUS.RellenoViajes1Min
(
    @Clienteids INT,
    @FormatTime VARCHAR(10),
    @PeriodoFecha DATETIME
)
AS
BEGIN

    -- DECLARAMOS VARIABLE PARA CONSULTAR 2 MESES 
    DECLARE @Periodoant DATETIME = DATEADD(MONTH, -1, @PeriodoFecha);

    -- Declaramos eventos a usar segun cliente
    DECLARE @EventCargakWh BIGINT;
    SET @EventCargakWh = CASE @Clienteids
                             WHEN 858 THEN
                                 8321587102626693941
                             WHEN 914 THEN
                                 -7412415661664623656
                             ELSE
                                 -7504498496990008389
                         END;

    DECLARE @EventDesargakWh BIGINT;
    SET @EventDesargakWh = CASE @Clienteids
                               WHEN 858 THEN
                                   8211150443017575316
                               WHEN 914 THEN
                                   -4310798714548824522
                               ELSE
                                   1947262230636828340
                           END;

    DECLARE @EventAltitud BIGINT;
    SET @EventAltitud = CASE @Clienteids
                            WHEN 858 THEN
                                2794663824013695887
                            WHEN 914 THEN
                                8521167961697377572
                            ELSE
                                -5349858454079666563
                        END;

    DECLARE @EventSOC1Min BIGINT;
    SET @EventSOC1Min = CASE @Clienteids
                            WHEN 858 THEN
                                -6908409462483507142
                            WHEN 914 THEN
                                -3463229819651977452
                            ELSE
                                184959813121443740
                        END;

    --CREAMOS TABLA TEMPORAL PARA EVENTOS A USAR
    IF OBJECT_ID('tempdb..#TemporaryEvents1Min') IS NOT NULL
        DROP TABLE #TemporaryEvents1Min;
    CREATE TABLE #TemporaryEvents1Min
    (
        EventId BIGINT,
        DriverId BIGINT,
        AssetId BIGINT,
        StartDateTime DATETIME NOT NULL,
        Value DECIMAL(18, 4) NULL,
        StartOdometerKilometres DECIMAL(18, 4) NULL,
        Latitud FLOAT NULL,
        Longitud FLOAT NULL,
        EventIdD BIGINT,
        EventTypeId BIGINT,
        ValueD DECIMAL(18, 4) NULL
    );

    CREATE INDEX idxtempeventef
    ON #TemporaryEvents1Min (EventTypeId)
    INCLUDE (
                DriverId,
                AssetId
            );
    CREATE INDEX idxtempeventidef
    ON #TemporaryEvents1Min (EventId)
    INCLUDE (
                DriverId,
                AssetId
            );
    CREATE INDEX idxtemeventfechaef
    ON #TemporaryEvents1Min (StartDateTime)
    INCLUDE (
                DriverId,
                AssetId
            );

    -- INSERTAMOS LOS EVENTOS EN LA TABLA TEMPORAL
    INSERT INTO #TemporaryEvents1Min
    (
        EventId,
        DriverId,
        AssetId,
        StartDateTime,
        Value,
        StartOdometerKilometres,
        Latitud,
        Longitud,
        EventIdD,
        EventTypeId,
        ValueD
    )
    SELECT EB.EventId,
           EB.driverId,
           EB.assetId,
           EB.StartDateTime,
           EB.Value,
           EB.StartOdometerKilometres,
           EB.Latitud,
           EB.Longitud,
           ED.EventId,
           ED.EventTypeId,
           ED.Value
    FROM
    (
        SELECT EV.EventId,
               EV.EventTypeId,
               EV.assetId,
               EV.driverId,
               EV.StartDateTime,
               EV.Value,
               EV.StartOdometerKilometres,
               EV.Latitud,
               EV.Longitud
        FROM #EventBus AS EV
        WHERE EV.EventTypeId = @EventSOC1Min
    ) AS EB
        INNER JOIN
        (
            SELECT *
            FROM #EventBus AS TE
            WHERE TE.EventTypeId IN ( @EventCargakWh, @EventDesargakWh, @EventAltitud )
        ) AS ED
            ON ED.StartDateTime = EB.StartDateTime
               AND ED.assetId = EB.assetId
               AND ED.driverId = EB.driverId;


    -- Insertamos data y ejecutamos script a las distintas tablas reporte
    -- REPORTE EFICIENCIA    
    DECLARE @SQLScript NVARCHAR(MAX)
        = N'
    INSERT INTO EBUS.Viajes1Min_' + CAST(@Clienteids AS VARCHAR)
          + N' (
        Movil,
        Operador,
        Fecha,
        Hora,
        FechaHora,
        SOC,
        CargakWh,
        DescargakWh,
        Odometro,
        Latitud,
        Longitud,
        Altitud,
        fechasistema,
        EsProcesado
        )
    SELECT 
        A.Placa AS Movil,
        D.DriverName AS Operador,
        CAST(EB.StartDateTime as DATE) AS Fecha,
        CAST(EB.StartDateTime AS TIME) AS Hora,
        EB.StartDateTime AS FechaHora,
        EB.[Value] AS SOC,
        CargakWh = SUM(   CASE
                               WHEN @EventCargakWh = EB.EventTypeId THEN
                                   EB.ValueD
                               ELSE
                                   NULL
                           END
                       ),
         DescargakWh = SUM(   CASE
                               WHEN @EventDesargakWh = EB.EventTypeId THEN
                                   EB.ValueD
                               ELSE
                                   NULL
                           END
                       ),
        EB.StartOdometerKilometres AS Odometro,
        EB.Latitud,
        EB.Longitud,
        Altitud = SUM(   CASE
                               WHEN @EventAltitud = EB.EventTypeId THEN
                                   EB.ValueD
                               ELSE
                                   NULL
                           END
                       ),
        GETDATE() AS fechasistema,
        0 AS EsProcesado
    FROM #TemporaryEvents1Min  AS EB
        INNER JOIN #Drivers AS D ON (D.DriverID = EB.driverId)
        INNER JOIN #Assets AS A ON (A.assetId = EB.assetId)
        GROUP BY 
         A.Placa,
         D.DriverName,
         EB.StartDateTime,
         EB.Value,
         EB.StartOdometerKilometres,
         EB.Latitud,
         EB.Longitud
    ORDER BY EB.StartDateTime';

    -- Ejecutamos el script con sus variables declaradas
    EXEC sp_executesql @SQLScript,
                       N'@EventCargakWh BIGINT, @EventDesargakWh BIGINT, @EventAltitud BIGINT ',
                       @EventCargakWh,
                       @EventDesargakWh,
                       @EventAltitud;


    -- Script para marcar los eventos Usados
    DECLARE @PeriodoW VARCHAR(7),
            @PeriodoantEvento DATETIME = @Periodoant;

    WHILE @PeriodoantEvento <= @PeriodoFecha
    BEGIN

        SET @PeriodoW
            = CAST(DATEPART(MONTH, @PeriodoantEvento) AS VARCHAR) + CAST(DATEPART(YEAR, @PeriodoantEvento) AS VARCHAR);

        SET @SQLScript
            = N'
		UPDATE [PORTAL].[TB_Event_' + @PeriodoW + N'_' + CAST(@Clienteids AS VARCHAR)
              + N']  SET EsProcesado = 1
		  WHERE EventId IN (
		  SELECT EV.EventIdD FROM #TemporaryEvents1Min AS EV
		  ) OR EventId IN (
		  SELECT EV.EventId FROM #TemporaryEvents1Min AS EV
		  )';

        -- Ejecutamos el Scritp
        EXEC sp_executesql @SQLScript;

        SET @PeriodoantEvento = DATEADD(MONTH, 1, @PeriodoantEvento);

    END;

END;
