-- =============================================
-- Author:      <Author, , Name>
-- Create Date: <Create Date, , >
-- Description: <Description, , >
-- =============================================
CREATE PROCEDURE [dbo].[SP_CambiarContrasena]
(    
	@correo VARCHAR(100)=NULL,
	@documento VARCHAR(100)=NULL,	
	@key VARBINARY (max) =NULL,
	@IV VARBINARY(max)=NULL,
	@contrasena VARBINARY(max)=NULL
)

AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    IF EXISTS (SELECT nombre, correo, estadoUsuarioIdS, documento, contrasena, IV, [key] FROM TB_Usuarios as U WHERE U.correo = @correo AND estadoUsuarioIdS=3)
	BEGIN
		UPDATE TB_Usuarios
				SET
				[key] = @key,
				IV	= @IV,
				contrasena = @contrasena,
				fechaUltimaActualizacion=GETDATE()
			WHERE correo = @correo
			SELECT nombre, correo, documento, contrasena, IV, [key] FROM TB_Usuarios WHERE correo = @correo
			
	END	

	ELSE IF EXISTS (SELECT nombre, correo, estadoUsuarioIdS, documento, contrasena, IV, [key] FROM TB_Usuarios as U WHERE estadoUsuarioIdS=3 AND documento=@documento)

	BEGIN
		UPDATE TB_Usuarios
				SET
				[key] = @key,
				IV	= @IV,
				contrasena = @contrasena,
				fechaUltimaActualizacion=GETDATE()
			WHERE documento = @documento
			SELECT nombre, correo, documento, contrasena, IV, [key] FROM TB_Usuarios WHERE documento = @documento
			
	END	
	
END