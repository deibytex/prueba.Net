-- =============================================
-- Author:      <Author, , Name>
-- Create Date: <Create Date, , >
-- Description: <Description, , >
-- =============================================
CREATE PROCEDURE [dbo].[SP_ConsultarContrasena]
@documento VARCHAR(100) = NULL
AS
BEGIN
 SELECT usuarioIdS, documento, IV, [key], contrasena FROM TB_Usuarios WHERE documento = @documento
END