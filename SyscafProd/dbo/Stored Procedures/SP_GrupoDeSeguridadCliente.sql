CREATE PROCEDURE SP_GrupoDeSeguridadCliente   
 @clienteIdS int     
AS      
BEGIN    
select * from TB_GruposDeSeguridad where TB_GruposDeSeguridad.clienteIdS=@clienteIdS
END