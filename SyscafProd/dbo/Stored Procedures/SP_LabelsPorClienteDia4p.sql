
CREATE PROCEDURE [dbo].[SP_LabelsPorClienteDia4p]                                
 @DESDE DATETIME =null,                              
 @HASTA DATETIME =null,                           
 @INTERVALOS int =NULL,      
 @USUARIOIDS INT = NULL,      
 @clienteIdS int =null      
AS                                      
BEGIN      
 select      
 clienteIdS,      
 clienteNombre   
 from TB_TransmisionLineayBarras      
 where fecha in (select fechas from FN_FechasIntervalos(@DESDE,@HASTA,@INTERVALOS))   
 and siteIdS in (select tg.siteIdS from VW_GrupoSeguridadSite tg where tg.usuarioIdS=@USUARIOIDS)
 and clienteIdS=ISNULL(@clienteIdS, clienteIdS)
 group by clienteIdS, clienteNombre
 order by clienteIdS asc
END