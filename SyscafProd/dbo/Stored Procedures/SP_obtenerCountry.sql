create procedure [SP_obtenerCountry]
as
begin
select * from TB_Country order by TB_Country.descripcion asc
end