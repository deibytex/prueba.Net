    
CREATE PROCEDURE SP_ConsultarUltimaPosicion    
AS      
BEGIN    
SELECT MAX(CAST(CAST( TB_Positions.TimeStamp AS DATE) AS datetime) ) as fecha   
FROM TB_Positions     
END