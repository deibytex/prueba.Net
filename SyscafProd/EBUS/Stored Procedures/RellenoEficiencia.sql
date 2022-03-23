



---- =============================================
---- Author:      Dlopez
---- Create Date: 22.11.2021
---- Description: Rellena eficiencia
---- Modified: 16/02/2022
---- =============================================
CREATE PROCEDURE EBUS.RellenoEficiencia
(
    @Clienteids INT,
    @FormatTime VARCHAR(10),
    @PeriodoFecha DATETIME,
	@Periodoant DATETIME
)
AS
BEGIN

  

    -- Declaramos eventos a usar segun cliente
    DECLARE @EventCarga BIGINT;
    SET @EventCarga = CASE @Clienteids
                          WHEN 858 THEN
                              -1733479165119402838
                          WHEN 914 THEN
                              1948943968326595879
                          ELSE
                              -4355925518935620665
                      END;

    DECLARE @EventMaxSOC BIGINT;
    SET @EventMaxSOC = CASE @Clienteids
                           WHEN 858 THEN
                               -1631066733938754176
                           WHEN 914 THEN
                               8816511971478625922
                           ELSE
                               -5733879658885524387
                       END;



    DECLARE @EventMinSOC BIGINT;
    SET @EventMinSOC = CASE @Clienteids
                           WHEN 858 THEN
                               -6908409462483507142
                           WHEN 914 THEN
                               -1409170827063065428
                           ELSE
                               5915482825600987196
                       END;

    -- Evento usado para calcula el start odometer
    DECLARE @SOCViaje BIGINT;
    SET @SOCViaje = CASE @Clienteids
                        WHEN 858 THEN
                            -1733479165119402838
                        WHEN 914 THEN
                            -3463229819651977452
                        ELSE
                            184959813121443740
                    END;

    -- CREAMOS TABLA TEMPORAL CON EVENTOW A USAR
    IF OBJECT_ID('tempdb..#TemporaryEventsEf') IS NOT NULL
        DROP TABLE #TemporaryEventsEf;
    CREATE TABLE #TemporaryEventsEf
    (
        EventId BIGINT,
        DriverId BIGINT,
        AssetId BIGINT,
        StartDateTime DATETIME NOT NULL,
        EndDateTime DATETIME NOT NULL,
        Value DECIMAL(18, 4) NULL,
        TotalTimeSeconds INT NULL,
        EndOdometerKilometres DECIMAL(18, 4) NULL,
        StartOdometerKilometres DECIMAL(18, 4) NULL,
        EventIdD BIGINT,
        EventTypeId BIGINT,
        ValueD DECIMAL(18, 4) NULL,
        StartOdometerKilometresD DECIMAL(18, 4) NULL
    );

    CREATE INDEX idxtempeventef
    ON #TemporaryEventsEf (EventTypeId)
    INCLUDE (
                DriverId,
                AssetId
            );
    CREATE INDEX idxtempeventidef
    ON #TemporaryEventsEf (EventId)
    INCLUDE (
                DriverId,
                AssetId
            );
    CREATE INDEX idxtemeventfechaef
    ON #TemporaryEventsEf (StartDateTime)
    INCLUDE (
                DriverId,
                AssetId
            );

    -- INSERTAMOS DATOS EN LA TABLE TEMPORAL
    INSERT INTO #TemporaryEventsEf
    (
        EventId,
        DriverId,
        AssetId,
        StartDateTime,
        EndDateTime,
        Value,
        TotalTimeSeconds,
        EndOdometerKilometres,
        StartOdometerKilometres,
        EventIdD,
        EventTypeId,
        ValueD,
        StartOdometerKilometresD
    )
    SELECT EB.EventId,
           EB.driverId,
           EB.assetId,
           EB.StartDateTime,
           EB.EndDateTime,
           EB.Value,
           EB.TotalTimeSeconds,
           EB.EndOdometerKilometres,
           EB.StartOdometerKilometres,
           ED.EventId,
           ED.EventTypeId,
           ED.Value,
           ED.StartOdometerKilometres
    FROM
    (SELECT * FROM #EventsBase AS TE WHERE StartDateTime <> EndDateTime) AS EB
        INNER JOIN
        (
            SELECT *
            FROM #EventBus AS TE
            WHERE TE.EventTypeId IN ( @EventCarga, @EventMaxSOC, @EventMinSOC, @SOCViaje )
                  AND TE.Value <> 0
        ) AS ED
            ON ED.StartDateTime
               BETWEEN EB.StartDateTime AND EB.EndDateTime
               AND ED.assetId = EB.assetId;


SELECT * FROM PORTAL.Assets AS A

    -- Insertamos data y ejecutamos script a las distintas tablas reporte
    -- REPORTE EFICIENCIA    
    DECLARE @SQLScript NVARCHAR(MAX)
        = N'
WITH consultaFinal (EventId, Movil, Operador, Fecha, Inicio, Fin, Carga, Descarga, Distancia, Duracion, DuracionHora,
                    TotalConsumo, MaxSOC, MinSOC, DSOC, fechasistema, EsProcesado, StartOdometerKilometresD,
                    EndOdometerKilometres
                   )
AS (SELECT EB.EventId,
           A.AssetsDescription AS Movil,
           D.name AS Operador,
           CAST(EB.StartDateTime AS DATE) AS Fecha,
           EB.StartDateTime AS Inicio,
           EB.EndDateTime AS Fin,
           Carga = SUM(   CASE
                              WHEN @EventCarga = EB.EventTypeId THEN
                                  EB.ValueD
                              ELSE
                                  0
                          END
                      ),
           EB.Value AS Descarga,
           Distancia = (EB.EndOdometerKilometres - ISNULL((MIN(   CASE
                                                                      WHEN @SOCViaje = EB.EventTypeId THEN
                                                                          EB.StartOdometerKilometresD
                                                                      ELSE
                                                                          NULL
                                                                  END
                                                              )
                                                          ),
                                                          EB.EndOdometerKilometres
                                                         )
                       ),
           CONVERT(VARCHAR, (DATEADD(SECOND, EB.TotalTimeSeconds, @FormatTime)), 8) AS Duracion,
           ROUND((EB.TotalTimeSeconds / 3600.0), 1) AS DuracionHora,
           (EB.Value - SUM(   CASE
                                  WHEN @EventCarga = EB.EventTypeId THEN
                                      EB.ValueD
                                  ELSE
                                      0
                              END
                          )
           ) AS TotalConsumo,
           MaxSOC = MAX(   CASE
                               WHEN @EventMaxSOC = EB.EventTypeId THEN
                                   EB.ValueD
                               ELSE
                                   0
                           END
                       ),
           MinSOC = MIN(   CASE
                               WHEN @EventMinSOC = EB.EventTypeId THEN
                                   EB.ValueD
                               ELSE
                                   NULL
                           END
                       ),
           DSOC = (MAX(   CASE
                              WHEN @EventMaxSOC = EB.EventTypeId THEN
                                  EB.ValueD
                              ELSE
                                  0
                          END
                      ) - MIN(   CASE
                                     WHEN @EventMinSOC = EB.EventTypeId THEN
                                         EB.ValueD
                                     ELSE
                                         NULL
                                 END
                             )
                  ),
           GETDATE() AS fechasistema,
           0 AS EsProcesado,
           StartOdometerKilometresD = ISNULL((MIN(   CASE
                                                         WHEN @SOCViaje = EB.EventTypeId THEN
                                                             EB.StartOdometerKilometresD
                                                         ELSE
                                                             NULL
                                                     END
                                                 )
                                             ),
                                             EB.EndOdometerKilometres
                                            ),
           EB.EndOdometerKilometres
    FROM #TemporaryEventsEf AS EB
        INNER JOIN PORTAL.TB_Drivers AS D
            ON (D.DriverId = EB.DriverId)
        INNER JOIN dbo.TB_Assets AS A
            ON (A.AssetId = EB.AssetId)
    GROUP BY EB.EventId,
             A.AssetsDescription,
             D.name,
             EB.StartDateTime,
             EB.EndDateTime,
             EB.Value,
             EB.TotalTimeSeconds,
             EB.EndOdometerKilometres,
             EB.StartOdometerKilometres)

INSERT INTO EBUS.Eficiencia_' + CAST(@Clienteids AS VARCHAR)
          + N' 
(
    EventId,
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
    StartOdometer,
    EndOdometer
)
SELECT EventId,
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
       StartOdometerKilometresD,
       EndOdometerKilometres
FROM consultaFinal AS EB
WHERE EB.EventId NOT IN
      (
          SELECT EventId
          FROM EBUS.Eficiencia_' + CAST(@Clienteids AS VARCHAR) + N' 
          WHERE Fecha >= dateadd(day, -3, @PeriodoFecha)
      );';





    -- Ejecutamos el script con sus variables declaradas
    EXEC sp_executesql @SQLScript,
                       N'@FormatTime AS VARCHAR(10), @EventCarga BIGINT, @EventMaxSOC BIGINT, @EventMinSOC BIGINT, @SOCViaje BIGINT, @PeriodoFecha datetime',
                       @FormatTime,
                       @EventCarga,
                       @EventMaxSOC,
                       @EventMinSOC,
                       @SOCViaje, @PeriodoFecha;


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
	  SELECT EV.EventIdD FROM #TemporaryEventsEf AS EV
	  )';

        -- Ejecutamos el Scritp
        EXEC sp_executesql @SQLScript;

        SET @PeriodoantEvento = DATEADD(MONTH, 1, @PeriodoantEvento);

    END;

END;
