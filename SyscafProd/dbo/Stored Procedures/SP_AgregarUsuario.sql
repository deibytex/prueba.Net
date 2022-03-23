    
CREATE PROCEDURE [dbo].[SP_AgregarUsuario]  
(  
 @usuarioIdS int=NULL,  
 @nombre VARCHAR(100)=NULL,  
 @apellido VARCHAR(100)=NULL,  
 @telefono VARCHAR(20)=NULL,  
 @documento VARCHAR(20)=NULL,  
 @correo VARCHAR(100)=NULL,  
 @key VARBINARY (max) =NULL,  
 @IV VARBINARY(max)=NULL,   
 @usuario VARCHAR(50)=NULL,  
 @perfilIdS int = NULL,  
 @contrasena VARBINARY(max)=NULL,  
 @usuarioIdSSession VARCHAR(50)=NULL  
)  
AS  
BEGIN  
    -- SET NOCOUNT ON added to prevent extra result sets from  
    -- interfering with SELECT statements.  
    IF NOT EXISTS (SELECT * FROM TB_Usuarios as U WHERE U.documento = @documento OR U.correo = @correo OR U.usuario = @usuario)  
  
  BEGIN  
  INSERT INTO TB_Usuarios  
  VALUES (@nombre,@apellido,@telefono,@documento,@correo,@key,@IV,@usuario,GETDATE(),GETDATE(),GETDATE(),@perfilIdS,3,@contrasena,'',0)  
  INSERT INTO TB_Log   
  VALUES (@usuarioIdSSession, GETDATE(), 'Se hizo la creación del usuario ' + @usuario)  
  SELECT 1  
  END  
  
 ELSE IF EXISTS (SELECT documento, correo, usuario FROM TB_Usuarios as U WHERE usuarioIdS = @usuarioIdS)  
  BEGIN  
  UPDATE TB_Usuarios  
   SET       
   nombre=@nombre,      
   apellido=@apellido,      
   telefono=@telefono,      
   documento=@documento,      
   correo=@correo,  
   fechaUltimaActualizacion=GETDATE(),  
   perfilIdS=@perfilIdS   
  WHERE usuarioIdS=@usuarioIdS  
  INSERT INTO TB_Log   
  VALUES (@usuarioIdSSession, GETDATE(), 'Se hizo la actualización del usuario ' + (SELECT usuario FROM TB_Usuarios as u WHERE @usuarioIdS = usuarioIdS))  
  SELECT 2  
  END  
 ELSE  
  BEGIN  
  SELECT 3  
  END  
END