CREATE PROCEDURE [dbo].[SP_CambiarEstadoSyscaf]      
 @estadoSyscafIdS int,      
 @registrationNumber varchar(100) ,      
 @usuarioIdSSession int
AS      
BEGIN  	

		UPDATE TB_Assets      
		SET     
		estadoSyscafIdS=@estadoSyscafIdS 
		WHERE    
		registrationNumber = @registrationNumber    
		INSERT INTO TB_Log   
		VALUES (@usuarioIdSSession, GETDATE(), 'Se cambio el estado del asset' + @registrationNumber)  

END