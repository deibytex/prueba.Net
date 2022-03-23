-- =============================================
-- Author:      <Author, , Name>
-- Create Date: <Create Date, , >
-- Description: <Description, , >
-- =============================================
CREATE PROCEDURE [dbo].[SP_ObtenerUsuarioNotify]

AS
BEGIN
select usuarioIdS, correo from TB_Usuarios
where notificacion = 1
END