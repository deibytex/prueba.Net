
CREATE PROCEDURE [dbo].[SP_ListarClientes] 
    @usuarioIdS INT = NULL,
    @clienteIdS INT = NULL
AS
BEGIN
    SELECT distinct tc.clienteIdS,
           tc.clienteNombre,
           tc.usuario,
           tu.usuario AS usuarioNombre,
           tc.countryIdS,
           tp.descripcion AS countryNombre,
           tc.telefono,
           tc.planComercial,
           tc.nit,
           tc.fechaIngreso,
           tc.estadoClienteIdS,
           tec.estado
    FROM TB_Cliente AS tc
        LEFT JOIN TB_Country AS tp
            ON (tc.countryIdS = tp.countryIdS)
        LEFT JOIN TB_Usuarios AS tu
            ON (tc.usuario = tu.usuarioIdS)
        LEFT JOIN TB_Estados AS tec
            ON (tc.estadoClienteIdS = tec.estadoIdS)
    WHERE clienteIdS IN (
				SELECT 
					tci.clienteIdS
				FROM 
					[dbo].[TB_GruposDeSeguridad] tg WITH (NOLOCK)
						LEFT JOIN 
					[dbo].[TB_GrupoSeguridadSites] ts WITH (NOLOCK)
							ON tg.[GrupoSeguridadId] = ts.[GrupoSeguridadId]
						LEFT JOIN 
					[dbo].[TB_GrupoSeguridadUsuario] tgs WITH (NOLOCK)
							ON tg.[GrupoSeguridadId] = tgs.[GrupoSeguridadId]
						LEFT JOIN 
					[dbo].[TB_Cliente] tci WITH (NOLOCK)
							ON (ts.clienteids IS NULL OR tci.clienteids = ts.clienteids)
				WHERE 
					tgs.usuarioids = @usuarioIdS
						AND 
					tci.estadoClienteIdS = 1
						AND 
					tgs.EsActivo = 1
						AND 
					tg.EsActivo = 1
						AND 
					ts.EsActivo IS NULL OR ts.EsActivo = 1)
			AND 
		tc.estadoClienteIdS = 1
			AND
		@clienteIdS IS NULL OR tc.clienteIdS = @clienteIdS
END;