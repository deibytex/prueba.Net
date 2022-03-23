CREATE PROCEDURE [dbo].[SP_TransmisionPorDia4p]                            
 @DESDE DATETIME =null,                          
 @HASTA DATETIME =null,                       
 @INTERVALOS int =NULL,  
 @USUARIOIDS INT = NULL,  
 @clienteIdS int =null  
AS                                  
BEGIN  
 select   
 fecha,  
 nombre as usuarioNombre,  
 sum(total) as total  
 from TB_TransmisionLineayBarras  
 where fecha in (select fechas from FN_FechasIntervalos(@DESDE,@HASTA,@INTERVALOS))  
 and siteIdS in (select tg.siteIdS from VW_GrupoSeguridadSite tg where tg.usuarioIdS=@USUARIOIDS) 
 group by   
 fecha,  
 nombre  
END