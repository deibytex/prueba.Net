
CREATE PROCEDURE SP_ConsultarEstado
@TIPOESTADO INT
AS      
BEGIN      
SELECT   
estadoIdS AS estadoSyscafIdS,  
estado AS estadoSyscaf  
FROM TB_Estados WHERE tipoIdS=@TIPOESTADO
END