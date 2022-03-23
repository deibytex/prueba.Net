
CREATE PROCEDURE SP_ObtenerPerfil

AS

BEGIN
	
	SELECT * FROM TB_Perfil ORDER BY TB_Perfil.descripcionPerfil ASC

END