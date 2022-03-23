
-- author = ygonzalez
-- date = 2021.07.23
-- description = guarda información final dee las tablas
-- 27.12.2021 se adiciona un campo adicional para identificar los eventos de ebus
--go


CREATE  procedure [PORTAL].[InsertEventByPeriodAndClient] (
 @Period nVarchar(10),
 @ClienteIds nvarchar(10),
 @DataEventos [PORTAL].[UDT_Event] readonly
)
as
begin
DECLARE @SQL NVARCHAR(4000) = '';
DeCLARE @Sufix NVARCHAR(20) = @Period + '_' + @ClienteIds;

	set @SQL = ' 
Insert into [Portal].[TB_Event_'+@Sufix+'](
	[EventId]
      ,[assetId]
      ,[driverId]
      ,[EventTypeId]
      ,[TotalTimeSeconds]
      ,[TotalOccurances]
      ,[StartDateTime]
      ,[EndDateTime]
      ,[FuelUsedLitres]
      ,[Value]
	  ,Latitud 
	  ,Longitud 
	  ,StartOdometerKilometres
	  ,EndOdometerKilometres
	  ,AltitudMeters
      ,[ClienteIds]
	  ,isebus
      ,[fechasistema]
	)
	select [EventId]
      ,[assetId]
      ,[driverId]
      ,[EventTypeId]
      ,[TotalTimeSeconds]
      ,[TotalOccurances]
      ,[StartDateTime]
      ,[EndDateTime]
      ,[FuelUsedLitres]
      ,[Value]
	  ,Latitud 
	  ,Longitud 
	  ,StartOdometerKilometres
	  ,EndOdometerKilometres
	  ,AltitudMeters
      ,[ClienteIds]
	  , isebus
      ,[fechasistema] from @data
	'
	exec sp_executesql @SQL,N'@data [PORTAL].[UDT_Event] readonly',@DataEventos
	

end
