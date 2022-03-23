

-- author = ygonzalez
-- date = 2021.06.22
-- description = verfifica los ids a mostrar

CREATE procedure [PORTAL].[VerifyDataStageByPeriodAndClient] (
 @Period Varchar(10),
 @Clienteids varchar(10),
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

exec Portal.CreateStageByPeriodAndClient @Period, @Clienteids
DeCLARE @Sufix NVARCHAR(20) = @Period + '_' + @ClienteIds;
-- encabezado igual para todas las validaciones

set @SQL = 'select id from 
@ListadoIds 
where id not in';


if(@Table =1)
begin
 set @SQL = @SQL + '( select TripId from [Portal].[TB_Trips_'+@Sufix+'])';
end
else 
	if(@Table =2)
	begin
	set @SQL = @SQL + ' (select EventId from [Portal].[TB_Event_'+@Sufix+'])';
	end
	else 
		if(@Table = 3)
		begin
		set @SQL = @SQL + '  ( select TripId from [Portal].[TB_TripsMetrics_'+@Sufix+'])';
		end
		else 
		if(@Table = 4)
		begin
		set @SQL = @SQL + '  ( select PositionId from [Portal].[TB_Positions_'+@Sufix+'])';
		end


	exec sp_executesql @SQL, N'@ListadoIds UDT_TableIdentity readonly',@Data 
	

end
