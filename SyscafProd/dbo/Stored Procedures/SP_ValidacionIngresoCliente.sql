CREATE PROCEDURE [dbo].[SP_ValidacionIngresoCliente]
@usuario VARCHAR(50),
@macAddress VARCHAR(200)

AS
BEGIN

	IF EXISTS(SELECT top 1 * FROM TB_Usuarios WITH(NOLOCK) WHERE usuario = ISNULL(@usuario,usuario) OR correo = ISNULL(@usuario,correo))
	BEGIN
		SELECT top 1 contrasena, [key], iv, usuarioIdS, perfilIdS, estadoUsuarioIdS, nombre + ' ' + apellido as nombre,imagen FROM TB_Usuarios WHERE usuario = ISNULL(@usuario,usuario) OR correo = ISNULL(@usuario,correo)
	END
	ELSE
	BEGIN
		SELECT 0x00 AS contrasena, 0x00 AS [key], 0x00 AS iv, 0x00 AS usuarioIdS, 0x00 AS perfilIdS
	END
END

select * from TB_Usuarios