-- ygonzalez 09/05/2021
-- trae la cantidad de llamadas realizadas al servidor
CREATE procedure [PORTAL].[GetSinceTokenMethodByCliente](
@Clienteid int,
@method varchar(100),
@fechasistema datetime, 
@SinceToken varchar(20) = null
)
as
begin
		

		if(@SinceToken is not  null)
		begin
				if not exists(select 1 from PORTAL.SinceTokenMix  where clienteid = @Clienteid and Method =@method)
				begin
						insert into PORTAL.SinceTokenMix(ClienteId,Method,SinceToken,UltimaActualizacion)
						values(@Clienteid,@method,@SinceToken, @fechasistema)	
				end
				else
				begin
						update PORTAL.SinceTokenMix set SinceToken = @SinceToken, UltimaActualizacion = @fechasistema
						where clienteid = @Clienteid and Method =@method
				end 
		end

		select  SinceToken from PORTAL.SinceTokenMix
		where clienteid = @Clienteid and Method =@method
end