

-- author = ygonzalez
-- date = 2021.07.23
-- description = guarda información final dee las tablas

create procedure [PORTAL].[InsertTripsMetricsByPeriodAndClient] (
 @Period nVarchar(10),
 @Clienteids nvarchar(10),
 @DataMetrics [PORTAL].[UDT_TripsMetrics] readonly
)
as
begin
DECLARE @SQL NVARCHAR(4000) = '';


	set @SQL = ' 
Insert into [Portal].[TB_TripsMetrics_'+@Period+'_'+@Clienteids+'](
	[TripId]
      ,[NIdleTime]
      ,[NIdleOccurs]
      ,[TripStart]      
      ,[ClienteIds]
      ,[fechasistema]
	)
	select [TripId]
      ,[NIdleTime]
      ,[NIdleOccurs]
      ,[TripStart]      
      ,[ClienteIds]
      ,[fechasistema] from @data
	'
	exec sp_executesql @SQL,N'@data [PORTAL].[UDT_TripsMetrics] readonly',@DataMetrics
	

end

