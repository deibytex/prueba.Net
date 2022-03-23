	
	-- =============================================
	-- Author:      <Author, , Name>
	-- Create Date: <Create Date, , >
	-- Description: <Description, , >
	-- =============================================
	CREATE PROCEDURE [dbo].[SP_PorcentajeTransmision]
	(
		@CarrosSinTx INT = null,
		@usuarioIdS INT = null, 
		@clienteIdS INT = null
	)
	AS
	BEGIN
		DECLARE @UnidadesActivas DECIMAL(10,2)
		SET
			@UnidadesActivas = (SELECT 
									count(*) 
								 FROM  
									TB_Assets ta 
										INNER JOIN 
									VW_GrupoSeguridadSite vgss 
											ON ta.clienteIdS = vgss.ClienteIds 
													AND
												ta.siteIdS = vgss.SiteIds
								 WHERE 
									vgss.usuarioIdS=@usuarioIdS 
										AND 
									(@clienteIdS IS NULL OR ta.clienteIdS=@clienteIdS) 
										AND 
									ta.estadoClienteIdS = 1)

			SELECT 'Total Unidades Activas' AS observacion, @UnidadesActivas AS totales
			UNION
			SELECT 'Total Unidades sin Transmisión', @CarrosSinTx
			UNION
			SELECT '% Unidades sin Transmisión', ROUND(CAST((@CarrosSinTx*100.0)/(@UnidadesActivas) AS FLOAT),2)  
	END