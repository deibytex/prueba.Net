﻿-- =============================================
-- Author:      Dlopez
-- Create Date: 22.11.2021
-- Description: Rellena alarmas para servicios de powerbi
-- =============================================

CREATE PROCEDURE EBUS.RellenoAlarmas
(
    @Clienteids INT,
    @FormatTime VARCHAR(10),
    @PeriodoFecha DATETIME
)
AS
BEGIN

    -- DECLARAMOS VARIABLE PARA CONSULTAR 2 MESES 
    DECLARE @Periodoant DATETIME = DATEADD(MONTH, -1, @PeriodoFecha);

    -- Creamos tablas para Filtrar los eventos a usar
    IF OBJECT_ID('tempdb..#FilterAlarmas') IS NOT NULL
        DROP TABLE #FilterAlarmas;
    CREATE TABLE #FilterAlarmas
    (
        EventTypeId BIGINT NOT NULL
    );

    CREATE INDEX idxFilterFilterAlarmas ON #FilterAlarmas (EventTypeId);

    -- Insertamos datos eventos filtro
    INSERT INTO #FilterAlarmas
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
                    AND TPDW.Parametrizacion LIKE '%80%'
          );

    -- CREAMOS TABLA EVENTOS A USAR
    IF OBJECT_ID('tempdb..#TemporaryEventsAlarmas') IS NOT NULL
        DROP TABLE #TemporaryEventsAlarmas;
    CREATE TABLE #TemporaryEventsAlarmas
    (
        EventId BIGINT,
        DriverId BIGINT,
        AssetId BIGINT,
        StartDateTime DATETIME NOT NULL,
        EndDateTime DATETIME NOT NULL,
        TotalTimeSeconds INT NULL,
        EventIdD BIGINT,
        EventTypeId BIGINT,
        Value DECIMAL(11, 4)
    );

    CREATE INDEX idxtempeventAlarmas
    ON #TemporaryEventsAlarmas (EventTypeId)
    INCLUDE (
                DriverId,
                AssetId
            );

    CREATE INDEX idxtempeventidAlarmas
    ON #TemporaryEventsAlarmas (EventId)
    INCLUDE (
                DriverId,
                AssetId
            );

    CREATE INDEX idxtemeventfechaAlarmas
    ON #TemporaryEventsAlarmas (StartDateTime)
    INCLUDE (
                DriverId,
                AssetId
            );

    -- INSERTAMOS DATOS EVENTOS A USAR
    INSERT INTO #TemporaryEventsAlarmas
    (
        EventId,
        DriverId,
        AssetId,
        StartDateTime,
        EndDateTime,
        TotalTimeSeconds,
        EventIdD,
        EventTypeId,
        Value
    )
    SELECT EB.EventId,
           EB.DriverId,
           EB.AssetId,
           EB.StartDateTime,
           EB.EndDateTime,
           ED.TotalTimeSeconds,
           ED.EventId,
           ED.EventTypeId,
           ISNULL(ED.Value, 0)
    FROM
    (
        SELECT EventId,
               DriverId,
               AssetId,
               StartDateTime,
               EndDateTime
        FROM #EventsBase
    ) AS EB
        INNER JOIN
        (
            SELECT EV.EventId,
                   EV.EventTypeId,
                   EV.assetId,
                   EV.driverId,
                   EV.StartDateTime,
                   EV.TotalTimeSeconds,
                   EV.Value
            FROM #EventBus AS EV
            WHERE EV.EventTypeId IN
                  (
                      SELECT EventTypeId FROM #FilterAlarmas
                  )
        ) AS ED
            ON (
                   ED.StartDateTime
               BETWEEN EB.StartDateTime AND EB.EndDateTime
                   AND ED.AssetId = EB.AssetId
                   AND ED.DriverId = EB.DriverId
               );

    -- Insertamos data y ejecutamos script a las distintas tablas reporte
    -- REPORTE ALARMAS
    DECLARE @SQLScript NVARCHAR(MAX)
        = N'
     INSERT INTO EBUS.Alarmas_' + CAST(@Clienteids AS VARCHAR)
          + N' (
        Movil,
        Operador,
        Fecha,
        Inicio,
        Fin,
        Descripcion,
        Duracion,
        DuracionHora,
        Valor,
        fechasistema,
        EsProcesado
        )
     SELECT
        A.Placa AS Movil,
        D.DriverName AS Operador,
        CAST(EB.StartDateTime AS DATE) AS Fecha,
        EB.StartDateTime AS Inicio,
        EB.EndDateTime AS Fin,
        E.descriptionEvent AS Descripcion,
        CONVERT(VARCHAR,(DATEADD(SECOND,SUM(EB.TotalTimeSeconds),@FormatTime)),8) AS Duracion,
        ROUND((SUM(EB.TotalTimeSeconds) / 3600.0),1) AS DuracionHora,
        ROUND(SUM(EB.Value),1) AS Value,
        GETDATE() AS fechasistema,
        0 AS EsProcesado
    FROM #TemporaryEventsAlarmas AS EB
        INNER JOIN #Drivers AS D ON (D.DriverID = EB.driverId)
        INNER JOIN #Assets AS A ON (A.assetId = EB.assetId)
        INNER JOIN #EventTypes AS E ON (E.eventTypeId = EB.EventTypeId)
    GROUP BY 
       A.Placa,
       D.DriverName,
       EB.StartDateTime,
       EB.EndDateTime,
       E.descriptionEvent
    ORDER BY EB.StartDateTime';

    -- Ejecutamos el script con sus variables declaradas
    EXEC sp_executesql @SQLScript, N'@FormatTime AS VARCHAR(10)', @FormatTime;

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
		  SELECT EV.EventIdD FROM #TemporaryEventsAlarmas AS EV
		  )';

        -- Ejecutamos el Scritp
        EXEC sp_executesql @SQLScript;

        SET @PeriodoantEvento = DATEADD(MONTH, 1, @PeriodoantEvento);

    END;

END;
