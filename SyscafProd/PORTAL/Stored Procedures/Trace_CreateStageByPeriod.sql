

-- author = ygonzalez
-- date = 2021.06.22
-- description = permite crear las tablas stage por mes para los datos del IMG

CREATE procedure [PORTAL].[Trace_CreateStageByPeriod] (
 @Period nVarchar(10)
)
as
begin
DECLARE @SQL NVARCHAR(4000) = '';
DECLARE @SQLEventos NVARCHAR(4000) = '';
DECLARE @SQLMetricas NVARCHAR(4000) = '';
DECLARE @SQLPosiciones NVARCHAR(4000) = '';



 set @SQL = 'IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N''[Portal].[TB_Trips_'+@Period+']'') AND type in (N''U''))

CREATE TABLE [Portal].[TB_Trips_'+@Period+'](
	[TripId] bigint NOT NULL primary key,		
	[assetId] [bigint] NOT NULL,	
	[driverId] [bigint] NOT NULL,
	[notes] [varchar](200)  NULL,
	[distanceKilometers] [decimal](18, 4)  NULL,	
	[StartOdometerKilometers] [decimal](18, 4)  NULL,
	[endOdometerKilometers] [decimal](18, 4)  NULL,
	[maxSpeedKilometersPerHour] [decimal](18, 4)  NULL,	
	[maxAccelerationKilometersPerHourPerSecond] [decimal](18, 4)  NULL,
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
	fechasistema datetime not null
	)'

set @SQLEventos = 'IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N''[Portal].[TB_Event_'+@Period+']'') AND type in (N''U''))
 
CREATE TABLE [Portal].[TB_Event_'+@Period+'](
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
	fechasistema datetime not null
	)
'
	exec sp_executesql @SQLEventos
	
	set @SQLEventos = 'IF NOT EXISTS (SELECT * 
FROM sys.indexes 
WHERE name=''nci_wi_TB_Event_'+@Period+''' AND object_id = OBJECT_ID(''Portal.TB_Event_'+@Period+'''))
	CREATE NONCLUSTERED INDEX [nci_wi_TB_Event_'+@Period+'] ON [PORTAL].[TB_Event_'+@Period+']
	(
		[ClienteIds] ASC,
		[EventTypeId] ASC
	)
	INCLUDE([assetId],[FuelUsedLitres],[StartDateTime],[TotalOccurances],[TotalTimeSeconds]) WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
	'
	exec sp_executesql @SQLEventos
	set @SQLMetricas = 'IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N''[Portal].[TB_TripsMetrics_'+@Period+']'') AND type in (N''U''))
 
CREATE TABLE [Portal].[TB_TripsMetrics_'+@Period+'](
	[TripId] bigint NOT NULL primary key,	
	[NIdleTime] int  NULL,
	[NIdleOccurs] int  NULL,
	[TripStart] [datetime] NOT NULL,	
	[ClienteIds] [int] NULL,
	fechasistema datetime not null
	)'

	set @SQLPosiciones = 'IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N''[Portal].[TB_Positions_'+@Period+']'') AND type in (N''U''))
 
CREATE TABLE [Portal].[TB_Positions_'+@Period+'](
	[PositionId] bigint NOT NULL primary key,
	[assetId] [bigint] NOT NULL,	
	[driverId] [bigint] NOT NULL,
	[Timestamp] datetime NULL,
	[Longitude] decimal(16,4)  NULL,
	[Latitude] decimal(16,4)  NULL,
	[FormattedAddress] varchar(250) NOT NULL,
    [AltitudeMetres] int null,
    [NumberOfSatellites] int null,
	[ClienteIds] [int] NULL,
	fechasistema datetime not null
	)'



	exec sp_executesql @SQL
	exec sp_executesql @SQLMetricas

	exec sp_executesql @SQLPosiciones

end
