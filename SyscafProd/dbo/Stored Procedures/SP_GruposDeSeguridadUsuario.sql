

CREATE PROCEDURE [dbo].[SP_GruposDeSeguridadUsuario]
(
	@usuarioIdS INT
)
AS
BEGIN
	SELECT 
		distinct TC.clienteIdS, 
		TC.clienteNombre
	FROM 
		[dbo].[TB_GruposDeSeguridad] tg WITH (NOLOCK)
			LEFT JOIN 
		[dbo].[TB_GrupoSeguridadSites] ts WITH (NOLOCK)
				ON tg.[GrupoSeguridadId] = ts.[GrupoSeguridadId]
			LEFT JOIN 
		[dbo].[TB_GrupoSeguridadUsuario] tgs WITH (NOLOCK)
				ON tg.[GrupoSeguridadId] = tgs.[GrupoSeguridadId]
			LEFT JOIN 
		[dbo].[TB_Cliente] tc WITH (NOLOCK)
				ON (ts.clienteids IS NULL or tc.clienteids = ts.ClienteIds)
	WHERE 
		(usuarioids = @usuarioIdS)
			AND 
		(tc.estadoClienteIdS = 1)
			AND 
		(tgs.EsActivo = 1)
			AND 
		(tg.EsActivo = 1)
			AND 
		(ts.EsActivo IS NULL OR ts.EsActivo = 1)
END;