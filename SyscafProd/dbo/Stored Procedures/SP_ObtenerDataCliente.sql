CREATE PROCEDURE [dbo].[SP_ObtenerDataCliente]
(
    -- Add the parameters for the stored procedure here
    @groupId varchar(50) = NULL
)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

    -- Insert statements for procedure here
    SELECT clienteIdS, clienteNombre FROM TB_Cliente
	WHERE clienteId = @groupId
END