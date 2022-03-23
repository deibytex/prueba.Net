 CREATE PROCEDURE [dbo].[SP_ListarAsset]
 (
	  @usuarioIdS int = null, 
	  @clienteIdS int = NULL
  )
AS             
BEGIN 
	SELECT  distinct      
		ta.assetIdS,      
		ta.assetId,      
		ta.groupId,      
		ta.createdDate,      
		     
		ta.siteIdS,      
		
		ta.registrationNumber,      
		ta.assetsDescription,      
		ta.estadoClienteIdS,      
		ta.estadoSyscafIdS ,
		si.siteName,
		cl.clienteNombre
	FROM           
		TB_Assets ta 
			LEFT JOIN 
		TB_Site si 
				ON (ta.siteIdS=si.siteIdS)  
			LEFT JOIN 
		TB_cliente cl 
				ON (si.clienteIdS=cl.clienteIdS)
			LEFT JOIN 
		(SELECT
			*
		FROM
			VW_GruposSeguridad tgi
		WHERE
			(@usuarioIdS IS NULL OR tgi.usuarioIdS = @usuarioIdS)) AS tg
				ON (tg.clienteIdS = ta.clienteIdS)
				
	WHERE 
		ta.estadoClienteIdS = 1
			AND 
		(@clienteIdS IS NULL OR ta.clienteIdS = @clienteIdS )


END