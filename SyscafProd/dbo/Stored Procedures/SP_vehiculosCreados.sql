CREATE procedure SP_vehiculosCreados
@desde datetime ,
@hasta datetime 
as 
begin
select 
ta.createdDate as fecha,
ta.assetsDescription as serie,
ta.estadoSyscafIdS as valor
from TB_Assets ta
where ta.createdDate>@desde 
and ta.createdDate<@hasta
end