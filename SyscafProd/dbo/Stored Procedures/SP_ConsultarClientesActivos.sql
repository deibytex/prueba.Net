
CREATE PROCEDURE SP_ConsultarClientesActivos            
AS               
BEGIN 
select clienteId from TB_Cliente where estadoClienteIdS=1
END