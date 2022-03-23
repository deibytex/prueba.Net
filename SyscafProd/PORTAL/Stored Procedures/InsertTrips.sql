

-- author = ygonzalez
-- date = 2021.07.23
-- description = guarda información final dee las tablas
--drop procedure [PORTAL].[InsertTrips]
CREATE procedure [PORTAL].[InsertTrips] (

 @Period nVarchar(10),
 @DataTrips [PORTAL].[UDT_Trips] readonly
)
as
begin
DECLARE @SQL NVARCHAR(4000) = '';


 set @SQL = 'Insert into [Portal].[TB_Trips_'+@Period+'](
	[TripId]
      ,[assetId]
      ,[driverId]
      ,[notes]
      ,[distanceKilometers]
	  ,StartOdometerKilometers
      ,[endOdometerKilometers]
      ,[maxSpeedKilometersPerHour]
      ,[maxAccelerationKilometersPerHourPerSecond]
      ,[maxRpm]
      ,[standingTime]
      ,[fuelUsedLitres]
      ,[startPositionId]
      ,[endPositionId]
      ,[startEngineSeconds]
      ,[endEngineSeconds]
      ,[tripEnd]
      ,[tripStart]
      ,[ClienteIds]
      ,[CantSubtrips]
	  ,maxDecelerationKilometersPerHourPerSecond
      ,[fechasistema], Duracion
	)	
	select [TripId]
      ,[assetId]
      ,[driverId]
      ,[notes]
      ,[distanceKilometers]
	  ,StartOdometerKilometers
      ,[endOdometerKilometers]
      ,[maxSpeedKilometersPerHour]
      ,[maxAccelerationKilometersPerHourPerSecond]
      ,[maxRpm]
      ,[standingTime]
      ,[fuelUsedLitres]
      ,[startPositionId]
      ,[endPositionId]
      ,[startEngineSeconds]
      ,[endEngineSeconds]
      ,[tripEnd]
      ,[tripStart]
      ,[ClienteIds]
      ,[CantSubtrips]
	  ,maxDecelerationKilometersPerHourPerSecond
      ,[fechasistema] , Duracion from @data
	'
	

	exec sp_executesql @SQL,N'@data [PORTAL].[UDT_Trips] readonly',@DataTrips
	

end

