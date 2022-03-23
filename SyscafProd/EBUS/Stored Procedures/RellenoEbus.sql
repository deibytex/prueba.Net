



-- author: dLopez
-- date: 21.10.2021
-- se realizan cambios en los eventos ya que no los traia
--EXEC EBUS.RellenoEbus @PeriodoFecha = '20220222',   @clienteIdS = 914,    @FechaInicial = null,     @FechaFinal = null 

--SELECT * FROM EBUS.Eficiencia_915 AS E
CREATE PROCEDURE [EBUS].[RellenoEbus]
(
    @PeriodoFecha DATETIME,
    @clienteIdS INT,
    @FechaInicial DATETIME = NULL,
    @FechaFinal DATETIME = NULL
)
AS
BEGIN

    --eBus_PBI30
    --DECLARE @PeriodoFecha DATETIME = '20211227',
    --        @clienteIdS INT = 914,
    --        @FechaInicial DATETIME = NULL,
    --        @FechaFinal DATETIME = NULL;

    -- si son los primeros 3 dias del mes verifica para realizar correctamente 
    DECLARE @Day INT = DATEPART(DAY, @PeriodoFecha);
    DECLARE @mesAtras INT = 0;
    IF (@Day BETWEEN 1 AND 3)
    BEGIN
        SET @mesAtras = -1;
        SET @FechaInicial = EOMONTH(@PeriodoFecha, @mesAtras); -- trae el ultimo dia del periodo anterior 

    END;
    ELSE
    BEGIN
        SET @FechaInicial = DATEADD(MONTH, DATEDIFF(MONTH, 0, @PeriodoFecha), 0);

    END;

    IF (@FechaFinal IS NULL)
        SET @FechaFinal = DATEADD(DAY,  1, EOMONTH(@PeriodoFecha, 0)); -- trae el ultimio dia del periodo actual




    -- DECLARAMOS VARIABLE PARA CONSULTAR 2 MESES 
    DECLARE @Periodoant DATETIME = DATEADD(MONTH, DATEDIFF(MONTH, 0, @FechaInicial), 0); 

    -- Creamos tablas Temp con indices si aplica

    -- Asset
    IF OBJECT_ID('tempdb..#Assets') IS NOT NULL
        DROP TABLE #Assets;
    CREATE TABLE #Assets
    (
        assetId VARCHAR(50),
        Placa VARCHAR(200)
    );

    CREATE INDEX idxtempAssets ON #Assets (assetId) INCLUDE (Placa);

    -- Drivers
    IF OBJECT_ID('tempdb..#Drivers') IS NOT NULL
        DROP TABLE #Drivers;
    CREATE TABLE #Drivers
    (
        DriverID VARCHAR(50),
        DriverName VARCHAR(MAX)
    );

    CREATE INDEX idxtempDrivers ON #Drivers (DriverID) INCLUDE (DriverName);

    -- Event Base
    IF OBJECT_ID('tempdb..#EventsBase') IS NOT NULL
        DROP TABLE #EventsBase;
    CREATE TABLE #EventsBase
    (
        EventId BIGINT,
        EventTypeId BIGINT,
        assetId BIGINT,
        driverId BIGINT,
        StartDateTime DATETIME,
        EndDateTime DATETIME,
        TotalTimeSeconds INT,
        Value DECIMAL(11, 4),
        EndOdometerKilometres DECIMAL(18, 4),
        StartOdometerKilometres DECIMAL(18, 4)
    );

    CREATE INDEX idxtempEventsBase
    ON #EventsBase (EventTypeId)
    INCLUDE (
                assetId,
                driverId
            );

    -- Event Types
    IF OBJECT_ID('tempdb..#EventTypes') IS NOT NULL
        DROP TABLE #EventTypes;
    CREATE TABLE #EventTypes
    (
        eventTypeId BIGINT,
        descriptionEvent VARCHAR(200)
    );

    CREATE INDEX idxtempeventypes
    ON #EventTypes (eventTypeId)
    INCLUDE (descriptionEvent);

    -- Creamos tablas paras los eventos generados
    IF OBJECT_ID('tempdb..#EventBus') IS NOT NULL
        DROP TABLE #EventBus;
    CREATE TABLE #EventBus
    (
        EventId BIGINT NOT NULL,
        assetId BIGINT NOT NULL,
        driverId BIGINT NOT NULL,
        EventTypeId BIGINT NOT NULL,
        TotalTimeSeconds INT NOT NULL,
        StartDateTime DATETIME NOT NULL,
        EndDateTime DATETIME NULL,
        Value DECIMAL(11, 4) NULL,
        Latitud FLOAT NULL,
        Longitud FLOAT NULL,
        StartOdometerKilometres DECIMAL(18, 4) NULL,
        EndOdometerKilometres DECIMAL(18, 4) NULL,
        AltitudMeters INT NULL,
        ClienteIds INT NULL,
        fechasistema DATETIME NOT NULL
    );

    CREATE INDEX idxtempevent
    ON #EventBus (EventTypeId)
    INCLUDE (
                driverId,
                assetId
            );
    CREATE INDEX idxtemeventfecha
    ON #EventBus (StartDateTime)
    INCLUDE (
                driverId,
                assetId
            );

    -- Insertamos Datos a todas las tablas temporales

    -- Aseets
    INSERT INTO #Assets
    (
        assetId,
        Placa
    )
    SELECT TA.assetId,
           TA.assetsDescription
    FROM dbo.TB_Assets AS TA WITH (NOLOCK)
    WHERE TA.clienteIdS = @clienteIdS;

    --Drivers
    INSERT INTO #Drivers
    (
        DriverID,
        DriverName
    )
    SELECT TD.DriverId,
           TD.name
    FROM PORTAL.TB_Drivers AS TD WITH (NOLOCK)
    WHERE TD.ClienteIds = @clienteIdS;

    -- Declaramos las variables usadas para traer los eventos por cliente
    DECLARE @eventdetalleid VARCHAR(50) = '%' + CAST(
                                                (
                                                    SELECT DetalleListaId
                                                    FROM TB_DetalleListas
                                                    WHERE Sigla = 'eBus_PBIDetalle'
                                                ) AS VARCHAR) + '%';
    DECLARE @Cliente VARCHAR(50) = '%' + CAST(@clienteIdS AS VARCHAR) + '%';

    -- Insertamos los tipos eventos segun preferencias descarga
    INSERT INTO #EventTypes
    (
        eventTypeId,
        descriptionEvent
    )
    SELECT eventTypeId,
           descriptionEvent
    FROM TB_EventType
    WHERE eventTypeId IN
          (
              SELECT PD.EventTypeId
              FROM dbo.TB_PreferenciasDescargarWS AS PD
              WHERE PD.Parametrizacion LIKE @eventdetalleid
                    AND ClientesId LIKE @Cliente
          );




    -- SCRIPT PARA LLENAR EVENTOS DETALLE
    DECLARE @SQLScript NVARCHAR(MAX),
            @PeriodoW VARCHAR(7),
            @PeriodoantEvento DATETIME = @Periodoant;


    -- While para buscar eventos cargados del mes anterior
    WHILE @PeriodoantEvento <= @PeriodoFecha
    BEGIN

        SET @PeriodoW
            = CAST(DATEPART(MONTH, @PeriodoantEvento) AS VARCHAR) + CAST(DATEPART(YEAR, @PeriodoantEvento) AS VARCHAR);

        -- SCRIPT PARA LLENAR EVENTOS DETALLE
        SET @SQLScript
            = N'
        INSERT INTO #EventBus
        (EventId
      , assetId
      , driverId
      , EventTypeId
      , TotalTimeSeconds
      , StartDateTime
      , EndDateTime
      , Value
      , Latitud
      , Longitud
      , StartOdometerKilometres
      , EndOdometerKilometres
      , AltitudMeters
      , ClienteIds
      , fechasistema
        )
         SELECT 
		     EventId
             , assetId
             , driverId
             , EventTypeId
             , TotalTimeSeconds
             , StartDateTime
             , EndDateTime
             , Value
             , Latitud
             , Longitud
             , StartOdometerKilometres
             , EndOdometerKilometres
             , AltitudMeters
             , ClienteIds
             , fechasistema
            FROM [PORTAL].[TB_Event_' + @PeriodoW + N'_' + CAST(@clienteIdS AS VARCHAR(10))
              + N'] (NOLOCK)  WHERE ( (@FechaI is null and @FechaF  is null) or  (StartDateTime >= @FechaI and  StartDateTime < @FechaF) )		  
		  AND  (EsProcesado = 0 ) and isEbus = 1  ';

        -- Ejecutamos el Scritp
        EXEC sp_executesql @SQLScript,
                           N'@FechaI datetime, @FechaF DateTime',
                           @FechaInicial,
                           @FechaFinal;

        SET @PeriodoantEvento = DATEADD(MONTH, 1, @PeriodoantEvento);

    END;



    -- Declaramos variables para extraer eventos por sigla e insertamos data
    DECLARE @eventbaseid VARCHAR(50)
        = '%' + CAST(
                (
                    SELECT DetalleListaId FROM TB_DetalleListas WHERE Sigla = 'eBus_PBI'
                ) AS VARCHAR) + '%';

    -- Rellenamos Los eventos base
    INSERT INTO #EventsBase
    (
        EventId,
        EventTypeId,
        assetId,
        driverId,
        StartDateTime,
        EndDateTime,
        TotalTimeSeconds,
        Value,
        EndOdometerKilometres,
        StartOdometerKilometres
    )
    SELECT EventId,
           EventTypeId,
           assetId,
           driverId,
           StartDateTime,
           EndDateTime,
           TotalTimeSeconds,
           Value,
           EndOdometerKilometres,
           StartOdometerKilometres
    FROM #EventBus
    WHERE EventTypeId IN
          (
              SELECT PD.EventTypeId
              FROM dbo.TB_PreferenciasDescargarWS AS PD
              WHERE PD.Parametrizacion LIKE @eventbaseid
                    AND ClientesId LIKE @Cliente
          );

    -- Declaramos Variable para aplicar formato de tiempo
    DECLARE @FormatTime AS VARCHAR(10) = '00:00:00';

    -- Ejecutamos Todos los Procedimientos asociados a los reportes

    -- Ejecutamos Relleno de Eficiencia
    EXEC EBUS.RellenoEficiencia @Clienteids = @clienteIdS,
                                @FormatTime = @FormatTime,
                                @PeriodoFecha = @PeriodoFecha,
                                @Periodoant = @Periodoant;

								 PRINT 'Eficiencia'
    -- Ejecutamos Relleno de ZP
    EXEC EBUS.RellenoZP @Clienteids = @clienteIdS,
                        @FormatTime = @FormatTime,
                        @PeriodoFecha = @PeriodoFecha;

    -- Ejecutamos Relleno de Alarmas
    EXEC EBUS.RellenoAlarmas @Clienteids = @clienteIdS,
                             @FormatTime = @FormatTime,
                             @PeriodoFecha = @PeriodoFecha;

    -- Ejecutamos Relleno de VIAJES 1 MIN
    EXEC EBUS.RellenoViajes1Min @Clienteids = @clienteIdS,
                                @FormatTime = @FormatTime,
                                @PeriodoFecha = @PeriodoFecha;

    -- -- Ejecutamos Relleno de Recargas 30 Seg
    -- EXEC EBUS.RellenoRecargas30Seg @Clienteids = @clienteIdS,
    --                               @FormatTime = @FormatTime,
    --                               @PeriodoFecha = @PeriodoFecha;



END;
