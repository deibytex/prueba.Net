
   
 CREATE FUNCTION [dbo].[FN_FechasIntervalos](@DESDE DATETIME ,@HASTA DATETIME, @INTERVALOS INT )            
RETURNS @tablafechas TABLE (fechas VARCHAR(10))            
AS            
BEGIN            
DECLARE @fechaDesde datetime=@DESDE, @DIVISOR INT             
SET @DIVISOR=(CASE            
WHEN @INTERVALOS=2 THEN 7            
WHEN @INTERVALOS=3 THEN 28            
WHEN @INTERVALOS=4 THEN 84            
WHEN @INTERVALOS=5 THEN 336            
ELSE 1            
END            
)            
insert into @tablafechas          
select CONVERT(VARCHAR(10),fechas, 111) From TB_Fechas WHERE (DATEDIFF(DAY,'19000119',TB_Fechas.fechas)%@DIVISOR)=0 and TB_Fechas.fechas>=@fechaDesde and TB_Fechas.fechas<=@HASTA      
ORDER BY TB_Fechas.fechas asc                  
RETURN             
END