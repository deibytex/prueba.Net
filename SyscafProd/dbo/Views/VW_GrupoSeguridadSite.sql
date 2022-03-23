
--======================================================================================
--FECHA: 08/02/2021
--DESCRIPCION: VISTA PARA TRAER GRUPOS DE SEGURIDAD SITE
--AUTOR: MD
--======================================================================================
CREATE VIEW [dbo].[VW_GrupoSeguridadSite]
AS
	SELECT DISTINCT
		tc.ClienteIds, 
		tsi.SiteIds,
		tsi.siteName,
		tc.clienteNombre,
		tgs.usuarioIdS
	FROM 
		[dbo].[TB_GruposDeSeguridad] tg
			INNER JOIN 
		[dbo].[TB_GrupoSeguridadUsuario] tgs 
			on tg.[GrupoSeguridadId] = tgs.[GrupoSeguridadId]
			left join 
		[dbo].[TB_GrupoSeguridadSites] ts 
			on tg.[GrupoSeguridadId] = ts.[GrupoSeguridadId]
			left join 
		[dbo].[TB_Cliente] tc 
			on (ts.clienteids is null or   tc.clienteids = ts.ClienteIds)
			LEFT JOIN
		TB_Site tsi
			ON
				tc.clienteIdS = tsi.clienteIdS
					AND 
				(TS.SiteIds IS NULL or ts.SiteIds = tsi.siteIdS)
				
	where 

	tc.estadoClienteIdS = 1 
	AND
	(tgs.EsActivo IS NULL OR  tgs.EsActivo = 1)
	and tg.EsActivo = 1 
	and (ts.EsActivo is null or ts.EsActivo = 1)