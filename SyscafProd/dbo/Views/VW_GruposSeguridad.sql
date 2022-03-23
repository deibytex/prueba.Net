

--======================================================================================
--FECHA: 08/02/2021
--DESCRIPCION: VISTA PARA TRAER EL LISTADO DE CLIENTES PERMITIDOS POR USUARIO
--AUTOR: MD
--======================================================================================

CREATE VIEW [dbo].[VW_GruposSeguridad] 
AS
select 
	tc.clienteIdS, 
	tc.clienteNombre,
	tgs.usuarioIdS
from 
	[dbo].[TB_GruposDeSeguridad] tg
		left join 
	[dbo].[TB_GrupoSeguridadSites] ts 
			on tg.[GrupoSeguridadId] = ts.[GrupoSeguridadId]
		left join 
	[dbo].[TB_GrupoSeguridadUsuario] tgs 
			on tg.[GrupoSeguridadId] = tgs.[GrupoSeguridadId]
		left join 
	[dbo].[TB_Cliente] tc 
			on (ts.clienteids is null or   tc.clienteids = ts.ClienteIds)
where 
	tc.estadoClienteIdS = 1 
		and 
	tgs.EsActivo = 1
		and 
	tg.EsActivo = 1 
		and 
	(ts.EsActivo is null or ts.EsActivo = 1)