

-- author = ygonzalez
-- date = 2021.06.22
-- description = permite crear las tablas stage por mes para los datos del IMG

CREATE PROCEDURE PORTAL.CreateStageByPeriodAndClient
(
    @Period NVARCHAR(10),
    @Clienteids NVARCHAR(10)
)
AS
BEGIN
    DECLARE @SQL NVARCHAR(4000) = N'';
    DECLARE @SQLEventos NVARCHAR(4000) = N'';
    DECLARE @SQLMetricas NVARCHAR(4000) = N'';
    DECLARE @SQLPosiciones NVARCHAR(4000) = N'';
    DECLARE @Sufix NVARCHAR(20) = @Period + N'_' + @Clienteids;

    DECLARE @Trips BIT,
            @Metrics BIT,
            @Event BIT,
            @Position BIT;

    SELECT @Trips = Trips,
           @Metrics = Metrics,
           @Event = Event,
           @Position = Position
    FROM dbo.TB_Cliente AS TC
    WHERE clienteIdS = CAST(@Clienteids AS INT);

    SET @SQL
        = N'IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N''[Portal].[TB_Trips_' + @Sufix
          + N']'') AND type in (N''U''))

CREATE TABLE [Portal].[TB_Trips_' + @Sufix
          + N'](
	[TripId] bigint NOT NULL primary key,		
	[assetId] [bigint] NOT NULL,	
	[driverId] [bigint] NOT NULL,
	[notes] [varchar](200)  NULL,
	[distanceKilometers] [decimal](18, 4)  NULL,	
	[StartOdometerKilometers] [decimal](18, 4)  NULL,
	[endOdometerKilometers] [decimal](18, 4)  NULL,
	[maxSpeedKilometersPerHour] [decimal](18, 4)  NULL,	
	[maxAccelerationKilometersPerHourPerSecond] [decimal](18, 4)  NULL,
	maxDecelerationKilometersPerHourPerSecond  [decimal](18, 4)  NULL,
	[maxRpm] [decimal](18, 4)  NULL,
	[standingTime] [decimal](18, 4)  NULL,
	[fuelUsedLitres] [decimal](18, 4) NULL,	
	[startPositionId] [varchar](50) NULL,
	[endPositionId] [varchar](50) NULL,	
	[startEngineSeconds] [decimal](18, 4) NULL,
	[endEngineSeconds] [int] NULL,
	[tripEnd] [datetime] NOT NULL,
	[tripStart] [datetime] NOT NULL,
	[ClienteIds] [int] NULL,
	[CantSubtrips] [int] NULL,
	EsProcesado bit not null default(0),
	EsActivo bit not null default(1),
	fechasistema datetime not null
	)';

    IF (@Event = 1)
    BEGIN
        SET @SQLEventos
            = N'IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N''[Portal].[TB_Event_' + @Sufix
              + N']'') AND type in (N''U''))
 
CREATE TABLE [Portal].[TB_Event_' + @Sufix
              + N'](
	[EventId] bigint NOT NULL primary key,		
	[assetId] [bigint] NOT NULL,	
	[driverId] [bigint] NOT NULL,
    [EventTypeId] bigint not null,
	[TotalTimeSeconds] int NOT NULL,
	[TotalOccurances] int NOT NULL,
	[StartDateTime] [datetime] NOT NULL,
	[EndDateTime] [datetime]  NULL,
	FuelUsedLitres decimal(10, 4) null,
	Value decimal(11, 4) null,
	[Latitud] float NULL,
	[Longitud] float NULL,
	[StartOdometerKilometres] [decimal](18, 4) NULL,
	[EndOdometerKilometres] [decimal](18, 4) NULL,
	[AltitudMeters] [int] NULL,
	[ClienteIds] [int] NULL,
	fechasistema datetime not null,
	EsActivo bit not null default(1),
	EsProcesado bit not null default(0),
	IseBus bit default(0)
	)
'       ;
        EXEC sp_executesql @SQLEventos;

        SET @SQLEventos
            = N'IF NOT EXISTS (SELECT * 
FROM sys.indexes 
WHERE name=''nci_wi_TB_Event_' + @Sufix + N''' AND object_id = OBJECT_ID(''Portal.TB_Event_' + @Sufix
              + N'''))
	CREATE NONCLUSTERED INDEX [nci_wi_TB_Event_' + @Sufix + N'] ON [PORTAL].[TB_Event_' + @Sufix
              + N']
	(
	
		[EventTypeId] ASC
	)
	INCLUDE([assetId],[StartDateTime], driverid, esprocesado, isebus, EsActivo) WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
	'   ;
        EXEC sp_executesql @SQLEventos;


      
        SET @SQLEventos
            = N'IF NOT EXISTS (SELECT * 
FROM sys.indexes 
WHERE name=''nci_wi_TB_startdatetime_' + @Sufix + N''' AND object_id = OBJECT_ID(''Portal.TB_Event_' + @Sufix
              + N'''))
	CREATE NONCLUSTERED INDEX [nci_wi_TB_startdatetime_' + @Sufix + N'] ON [PORTAL].[TB_Event_' + @Sufix
              + N']
	(
		[StartDateTime] ASC
	)include ([EventTypeId]  , assetid, driverid, esprocesado, isebus, EsActivo)'  ;
        EXEC sp_executesql @SQLEventos;


      
        SET @SQLMetricas
            = N'IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N''[Portal].[TB_TripsMetrics_'
              + @Sufix + N']'') AND type in (N''U''))
 
CREATE TABLE [Portal].[TB_TripsMetrics_' + @Sufix
              + N'](
	[TripId] bigint NOT NULL primary key,	
	[NIdleTime] int  NULL,
	[NIdleOccurs] int  NULL,
	[TripStart] [datetime] NOT NULL,	
	[ClienteIds] [int] NULL,
	fechasistema datetime not null,
	EsProcesado bit not null default(0)
	)'  ;
    END;
    SET @SQLPosiciones
        = N'IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N''[Portal].[TB_Positions_' + @Sufix
          + N']'') AND type in (N''U''))
 
CREATE TABLE [Portal].[TB_Positions_' + @Sufix
          + N'](
	[PositionId] bigint NOT NULL primary key,
	[assetId] [bigint] NOT NULL,	
	[driverId] [bigint] NOT NULL,
	[Timestamp] datetime NULL,
	[Latitud] float NULL,
	[Longitud] float NULL,
	[FormattedAddress] varchar(250) NOT NULL,
    [AltitudeMetres] int null,
    [NumberOfSatellites] int null,
	[ClienteIds] [int] NULL,
	fechasistema datetime not null
	)';


    IF (@Trips = 1) EXEC sp_executesql @SQL;
    IF (@Metrics = 1) EXEC sp_executesql @SQLMetricas;
    IF (@Position = 1) EXEC sp_executesql @SQLPosiciones;

END;
