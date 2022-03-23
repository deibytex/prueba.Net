
CREATE PROCEDURE [dbo].[SP_TransmisionPorClienteDia4p] (                             
	 @DESDE DATETIME =null,                              
	 @HASTA DATETIME =null,                           
	 @INTERVALOS int =NULL,      
	 @USUARIOIDS INT = NULL,      
	 @clienteIdS int =null  
 )
AS                                      
BEGIN      
	 SELECT       
		 fecha,      
		 clienteIdS,      
		 clienteNombre,      
		 total      
	 FROM 
		TB_TransmisionLineayBarras      
	 WHERE 
		fecha in (select fechas from FN_FechasIntervalos(@DESDE,@HASTA,@INTERVALOS))   
			and 
		siteIdS in (select tg.siteIdS from VW_GrupoSeguridadSite tg where tg.usuarioIdS=@USUARIOIDS)
			and 
		clienteIdS=ISNULL(@clienteIdS, clienteIdS)
	 order by 
		clienteIdS asc
END