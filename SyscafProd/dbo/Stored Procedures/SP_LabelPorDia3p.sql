CREATE PROCEDURE [dbo].[SP_LabelPorDia3p]                          
 @DESDE DATETIME =null,                        
 @HASTA DATETIME =null,                     
 @INTERVALOS int =NULL
AS                                
BEGIN
select fechas from FN_FechasIntervalos(@DESDE,@HASTA,@INTERVALOS)	
END