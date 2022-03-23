-- =============================================
-- Author:      Dlopez
-- Create Date: 22.11.2021
-- Description: 
-- =============================================
	
CREATE PROCEDURE EBUS.RellenoZP
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
        EventId BIGINT,
        DriverId BIGINT,
        AssetId BIGINT,
        StartDateTime DATETIME NOT NULL,
        EndDateTime DATETIME NOT NULL,
        TotalTimeSeconds INT NULL,
        EventIdD BIGINT,
        EventTypeId BIGINT,
    );

    CREATE INDEX idxtempeventZP
    ON #TemporaryEventsZP (EventTypeId)
    INCLUDE (
                DriverId,
                AssetId
            );
    CREATE INDEX idxtempeventidZP
    ON #TemporaryEventsZP (EventId)
    INCLUDE (
                DriverId,
                AssetId
            );
    CREATE INDEX idxtemeventfechaZP
    ON #TemporaryEventsZP (StartDateTime)
    INCLUDE (
                DriverId,
                AssetId
            );

    --Insertamos datod de eventos a usar 
    INSERT INTO #TemporaryEventsZP
    (
        EventId,
        DriverId,
        AssetId,
        StartDateTime,
        EndDateTime,
        TotalTimeSeconds,
        EventIdD,
        EventTypeId
    )
    SELECT EB.EventId,
           EB.driverId,
           EB.assetId,
           EB.StartDateTime,
           EB.EndDateTime,
           ED.TotalTimeSeconds,
           ED.EventId,
           ED.EventTypeId
    FROM
    (SELECT * FROM #EventsBase) AS EB
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
               BETWEEN EB.StartDateTime AND EB.EndDateTime
                   AND ED.assetId = EB.assetId
                   AND ED.driverId = EB.driverId
               );

    -- Insertamos data y ejecutamos script a las distintas tablas reporte
    -- REPORTE ZP
    DECLARE @SQLScript NVARCHAR(MAX)
        = N'
    INSERT INTO EBUS.ZP_' + CAST(@Clienteids AS VARCHAR)
          + N' (
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
        GETDATE() AS fechasistema,
        0 AS EsProcesado
    FROM #TemporaryEventsZP AS EB
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
		  SELECT EV.EventIdD FROM #TemporaryEventsZP AS EV
		  )';

        -- Ejecutamos el Scritp
        EXEC sp_executesql @SQLScript;

        SET @PeriodoantEvento = DATEADD(MONTH, 1, @PeriodoantEvento);

    END;

END;
