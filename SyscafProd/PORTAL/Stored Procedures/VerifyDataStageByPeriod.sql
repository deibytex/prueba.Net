
-- author = ygonzalez
-- date = 2021.06.22
-- description = verfifica los ids a mostrar

CREATE procedure [PORTAL].[VerifyDataStageByPeriod] (
 @Period Varchar(10),
 @Table int,
 @Data UDT_TableIdentity readonly
)
as
begin
DECLARE @SQL NVARCHAR(4000) = '';
--declare @Period Varchar(10) = '72021',
-- @Table int = 1,
-- @Data UDT_TableIdentity 
-- verificamos que existan las tablas para el periodo seleccionado

exec Portal.Trace_CreateStageByPeriod @Period

-- encabezado igual para todas las validaciones

set @SQL = 'select id from 
@ListadoIds 
where id not in';


if(@Table =1)
begin
 set @SQL = @SQL + '( select TripId from [Portal].[TB_Trips_'+@Period+'])';
end
else 
	if(@Table =2)
	begin
	set @SQL = @SQL + ' (select EventId from [Portal].[TB_Event_'+@Period+'])';
	end
	else 
		if(@Table = 3)
		begin
		set @SQL = @SQL + '  ( select TripId from [Portal].[TB_TripsMetrics_'+@Period+'])';
		end
		else 
		if(@Table = 4)
		begin
		set @SQL = @SQL + '  ( select PositionId from [Portal].[TB_Positions_'+@Period+'])';
		end


	exec sp_executesql @SQL, N'@ListadoIds UDT_TableIdentity readonly',@Data 
	

end