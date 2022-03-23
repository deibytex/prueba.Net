CREATE PROCEDURE [dbo].[SP_CambiarEstadoUsuario]  
(      
 @usuarioIdS int=NULL,  
 @usuarioIdSSession int=NULL  
)  
  
AS  
BEGIN  
    -- SET NOCOUNT ON added to prevent extra result sets from  
    -- interfering with SELECT statements.  
    IF ((SELECT estadoUsuarioIdS FROM TB_Usuarios as U WHERE U.usuarioIdS = @usuarioIdS)=3)  
  
 BEGIN  
  UPDATE TB_Usuarios  
    SET       
    estadoUsuarioIdS=4,  
    fechaUltimaActualizacion=GETDATE()  
   WHERE usuarioIdS=@usuarioIdS  
   INSERT INTO TB_Log   
   VALUES (@usuarioIdSSession, GETDATE(), 'Se inactivo el usuario ' + (SELECT usuario FROM TB_Usuarios as u WHERE @usuarioIdS = usuarioIdS))      
   SELECT 1  
 END  
  ELSE   
   BEGIN  
   UPDATE TB_Usuarios  
    SET       
    estadousUarioIdS=3,  
    fechaUltimaActualizacion=GETDATE()  
   WHERE usuarioIdS=@usuarioIdS    
   INSERT INTO TB_Log   
   VALUES (@usuarioIdSSession, GETDATE(), 'Se Activo el usuario ' + (SELECT usuario FROM TB_Usuarios as u WHERE @usuarioIdS = usuarioIdS))      
   SELECT 2  
  END  
END