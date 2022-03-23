CREATE procedure [dbo].[SP_TransmisionAdministradorVrEstado3p] 
 (
	@usuarioIdS int,
	@clienteIdS int = NULL
)
AS  
BEGIN  

	--============================================================================================================
	-- DANGER-- HAY QUE CAMBIARLO
	--============================================================================================================
	declare @FechaActual datetime = CONVERT(datetime,SWITCHOFFSET(CONVERT(datetimeoffset,GetUTCDate()),'-05:00'))


	DECLARE @TABLA TABLE (
			usuarioNombre Varchar(50) null,  
			usuarioIdS int null,  
			detenidoAparcado int null,  
			enMantenimiento int null,   
			operandoNormal int null,  
			sinRespuestaCliente int null,  
			total int NULL
	)  

	DECLARE @TABLASINTX TABLE (
			usuarioIdS INT null,  
			usuarioNombre Varchar(50) null,  
			assetId Varchar(50),   
			estadoSyscaf int NULL
	)


	INSERT INTO 
		@TABLASINTX   
	SELECT DISTINCT
		tu.usuarioIdS,  
		tu.nombre,  
		ta.assetId,  
		estadoSyscafIdS = ISNULL(ta.estadoSyscafIdS,8)
	FROM 
		(select ta1.clienteids, ta1.assetIdS, ta1.estadoSyscafIdS  , ta1.assetId, ta1.siteIdS from TB_Assets ta1
			INNER JOIN 
		TB_Site AS ts   
				ON (ts.siteIdS=ta1.siteIdS)
			WHERE 
		ta1.estadoClienteIdS=1
			AND 
	ts.tipoSitio <> 9
	 and ta1.siteIdS in (
	 SELECT
				distinct vgsi.SiteIds
			FROM
				VW_GrupoSeguridadSite vgsi
			WHERE
				(vgsi.usuarioIdS = @usuarioIdS)
					AND
				(@clienteIdS IS NULL OR vgsi.clienteIdS = @clienteIdS)
					
	 
	 )
		)  as ta   
	
			INNER JOIN 
		TB_AdministradorClientes tac 
				ON tac.ClienteIds = ta.ClienteIds 
						AND
					tac.SiteIds = ta.siteIdS
			INNER JOIN 
		TB_Usuarios tu 
				ON (tu.usuarioIdS=tac.usuarioIdS)
 


	
	INSERT INTO @TABLA    
	SELECT 
		usuarioNombre,  
		usuarioIds, 
		isnull([5],0) as DetenidoAparcado, isnull([6],0) EnMantenimiento, isnull([7],0) OperandoNormal, isnull([8],0) as SinRespuestaCliente,
		isnull([5],0)+ isnull([6],0)+ isnull([7],0)+ isnull([8],0) Total
	FROM  
	(
		 select 
			COUNT(*) cantidad,
			x.usuarioNombre,
			x.estadoSyscaf , 
			x.usuarioIds 
		 from 
			@TABLASINTX x
		 group by  
			x.usuarioIds, 
			x.usuarioNombre, 
			x.estadoSyscaf
	) AS SourceTable  
	PIVOT  
	(  
		sum(cantidad)  
		FOR estadoSyscaf IN ([5], [6], [7], [8])  
	) AS PivotTable
	order by 
		usuarioIds;  

	INSERT INTO @TABLA
	SELECT
	   'total'
	  ,''
	  ,SUM(detenidoAparcado)
	  ,SUM(enMantenimiento)
	  ,SUM(operandoNormal)
	  ,SUM(sinRespuestaCliente)
	  ,SUM(total)
	from 
		@TABLA

	SELECT   
		usuarioNombre,  
		detenidoAparcado = ISNULL(detenidoAparcado,0) ,  
		enMantenimiento = ISNULL(enMantenimiento,0) ,   
		operandoNormal = ISNULL(operandoNormal,0) ,  
		sinRespuestaCliente = ISNULL(sinRespuestaCliente,0),  
		total = ISNULL(total,0) 
	 FROM 
		@TABLA  
END