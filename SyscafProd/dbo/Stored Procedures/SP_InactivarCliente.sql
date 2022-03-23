CREATE PROCEDURE SP_InactivarCliente    
@idCliente int    
AS    
BEGIN    
DELETE FROM TB_Cliente WHERE TB_Cliente.clienteIdS=@idCliente    
END