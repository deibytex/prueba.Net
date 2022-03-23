CREATE PROCEDURE [dbo].[SP_InactivarClientes]
	AS 
	BEGIN

	update TB_Site set estadoBase=0
	where clienteIdS in(select clienteIdS from TB_Cliente where estadoClienteIdS=0)

	END