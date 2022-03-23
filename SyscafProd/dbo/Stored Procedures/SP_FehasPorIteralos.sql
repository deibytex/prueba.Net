  
CREATE PROCEDURE [dbo].[SP_FehasPorIteralos]  
 @DESDE DATETIME =null,          
 @HASTA DATETIME =null,       
 @INTERVALOS int =NULL      
AS                  
BEGIN    
DECLARE @fechaDesde VARCHAR(10), @DIVISOR INT   
SET @fechaDesde=(CONVERT(VARCHAR(10),@DESDE, 111))
SET @DIVISOR=(CASE  
WHEN @INTERVALOS=2 THEN 7  
WHEN @INTERVALOS=3 THEN 28  
WHEN @INTERVALOS=4 THEN 84  
WHEN @INTERVALOS=5 THEN 336  
ELSE 1  
END  
) 

select * From TB_Fechas WHERE (DATEDIFF(DAY,'19000119',TB_Fechas.fechas)%@DIVISOR)=0 and TB_Fechas.fechas>@fechaDesde
ORDER BY TB_Fechas.fechas asc      
END