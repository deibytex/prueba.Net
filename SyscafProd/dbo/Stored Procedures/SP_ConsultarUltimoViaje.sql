  
CREATE PROCEDURE SP_ConsultarUltimoViaje     
AS           
BEGIN        
SELECT DATEADD(DAY,-1,GETDATE()) AS fecha
END