-- =============================================
-- Author:      <Author, , Name>
-- Create Date: <Create Date, , >
-- Description: <Description, , >
-- =============================================
CREATE PROCEDURE [dbo].[SP_ObtenerContactosNotify]
@usuarioIdS int = null,
@clienteIdS int = null
AS
BEGIN
select * from  TB_CorreosContactos
where usuarioIdS = @usuarioIdS and clienteIdS = @clienteIdS
END