--[dbo].[SP_ConsultarUsuario] 194, null,  1
CREATE PROCEDURE [dbo].[SP_ConsultarUsuario]-- null,null,0
@usuarioIdS int = NULL,
@clienteIdS int = NULL,
@perfilIdS int = NULL
AS  
BEGIN
	if (@perfilIdS <> 1 )
	BEGIN
		SELECT 
			nombre, 
			apellido, 
			usuario, 
			tp.perfilIdS, 
			descripcionPerfil, 
			te.estadoIdS as estadoUsuarioIdS, 
			estado, 
			usuarioIdS, 
			documento, 
			correo, 
			telefono, 
			fechaUltimaActualizacion 
		FROM 
			TB_Usuarios AS tu   
				INNER JOIN 
			TB_Perfil AS tp 
					ON tp.perfilIdS = tu.perfilIdS   
				INNER JOIN 
			TB_Estados AS te 
					ON te.estadoIdS = tu.estadoUsuarioIdS   
		WHERE 
			tu.usuarioIdS in (select tg.usuarioIdS FROM VW_GruposSeguridad tg where @usuarioIdS IS NULL OR tg.usuarioIdS= @usuarioIdS and @clienteIdS IS NULL OR tg.clienteIdS = @clienteIdS)   
	END
	ELSE IF (@clienteIdS IS  NULL)
	BEGIN
		SELECT 
			nombre, 
			apellido, 
			usuario, 
			tp.perfilIdS, 
			descripcionPerfil, 
			te.estadoIdS as estadoUsuarioIdS, 
			estado, 
			usuarioIdS, 
			documento, 
			correo, 
			telefono, 
			fechaUltimaActualizacion 
		FROM 
			TB_Usuarios AS tu   
				INNER JOIN 
			TB_Perfil AS tp 
					ON tp.perfilIdS = tu.perfilIdS   
				INNER JOIN 
			TB_Estados AS te 
					ON te.estadoIdS = tu.estadoUsuarioIdS
	 WHERE 
		@usuarioIdS IS NULL OR tu.usuarioIdS = @usuarioIdS
	END
	ELSE
	BEGIN
		SELECT 
			nombre, 
			apellido, 
			usuario, 
			tp.perfilIdS, 
			descripcionPerfil, 
			te.estadoIdS as estadoUsuarioIdS, 
			estado, 
			usuarioIdS, 
			documento, 
			correo, 
			telefono, 
			fechaUltimaActualizacion 
		FROM 
			TB_Usuarios AS tu   
				INNER JOIN 
			TB_Perfil AS tp 
					ON tp.perfilIdS = tu.perfilIdS   
				INNER JOIN 
			TB_Estados AS te 
					ON te.estadoIdS = tu.estadoUsuarioIdS   
		 WHERE 
			tu.usuarioIdS in (select tg.usuarioIdS FROM VW_GruposSeguridad tg where @usuarioIdS IS NULL OR tg.usuarioIdS= @usuarioIdS and @clienteIdS IS NULL OR tg.clienteIdS = @clienteIdS)
	END
END