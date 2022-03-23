
-- author = ygonzalez
-- date = 2021.06.22
-- description = permite crear las tablas stage por mes para los datos del IMG
-- [PORTAL].[IMG_CreateStageByPeriod] '82021'
CREATE procedure [PORTAL].[IMG_CreateStageByPeriod] 
(
 @Period nVarchar(10)
)
as
begin
DECLARE @SQL NVARCHAR(4000) = '';

 set @SQL = '
 IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N''[dbo].[TB_FuenteIMG_'+@Period+']'') AND type in (N''U''))
 BEGIN
CREATE TABLE [dbo].[TB_FuenteIMG_'+@Period+'](
	[Consecutivo] [int] IDENTITY(1,1) NOT NULL,
	[Cliente] [varchar](300) NULL,
	[US] [varchar](3) NULL,
	[VehicleID] [int] NULL,
	[Placa] [varchar](100) NULL,
	[VehicleSiteID] [bigint] NULL,
	[VehicleSiteName] [varchar](200) NULL,
	[TripNo] [bigint] NULL,
	[DriverID] [bigint] NULL,
	[DriverName] [varchar](300) NULL,
	[DriverSiteID] [bigint] NULL,
	[DriverSiteName] [varchar](200) NULL,
	[OriginalDriverID] [int] NULL,
	[OriginalDriverName] [varchar](50) NULL,
	[TripStart] [datetime] NULL,
	[TripEnd] [datetime] NULL,
	[CategoryID] [int] NULL,
	[Notes] [varchar](50) NULL,
	[StartSubTripSeq] [int] NULL,
	[EndSubTripSeq] [int] NULL,
	[TripDistance] [decimal](18, 1) NULL,
	[Odometer] [decimal](18, 1) NULL,
	[MaxSpeed] [int] NULL,
	[SpeedTime] [int] NULL,
	[SpeedOccurs] [int] NULL,
	[MaxBrake] [int] NULL,
	[BrakeTime] [int] NULL,
	[BrakeOccurs] [int] NULL,
	[MaxAccel] [int] NULL,
	[AccelTime] [int] NULL,
	[AccelOccurs] [int] NULL,
	[MaxRPM] [int] NULL,
	[RPMTime] [int] NULL,
	[RPMOccurs] [int] NULL,
	[GBTime] [int] NULL,
	[ExIdleTime] [int] NULL,
	[ExIdleOccurs] [int] NULL,
	[NIdleTime] [int] NULL,
	[NIdleOccurs] [int] NULL,
	[StandingTime] [int] NULL,
	[Litres] [decimal](18, 1) NULL,
	[StartGPSID] [bigint] NULL,
	[EndGPSID] [bigint] NULL,
	[StartEngineSeconds] [int] NULL,
	[EndEngineSeconds] [int] NULL,
	[ClienteIdS] [int] NOT NULL,
 CONSTRAINT [PK_TB_FuenteIMG] PRIMARY KEY CLUSTERED 
(
	[Consecutivo] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]


CREATE NONCLUSTERED INDEX [FuenteIMG] ON [dbo].[TB_FuenteIMG_'+@Period+']
(
	[ClienteIdS] ASC,
	[TripStart] ASC
)
INCLUDE(TripNo)
WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]

END
'



	exec sp_executesql @SQL
	

end