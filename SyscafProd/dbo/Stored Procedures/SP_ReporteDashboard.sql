 --[dbo].[SP_ReporteDashboard] 898,'40,41,42,43,44,45,46,47,48,49,50','2021','','',''
CREATE PROCEDURE [dbo].[SP_ReporteDashboard] 
(	 
	 @ClienteIds						 INT 
	,@TipoReporteId				 VARCHAR(50) 
	,@FechaInicial					DATETIME = NULL
	,@FechaFinal					DATETIME = NULL
	,@Placa						VARCHAR(MAX) = NULL
)
AS
BEGIN
	SELECT 	
		 ReporteDashboradID
		,ClienteIds
		,FechaReporte
		,FechaFinal
		,InformacionExtendida
		,DataReporte
		,TipoReporteId
		,Mes
		,Dia
		,Anio
	FROM
		dbo.TB_ReporteDashboard-- WITH(NOLOCK) 
	WHERE	
		(ClienteIds = @ClienteIds )  
		AND  (TipoReporteId in (select value from STRING_SPLIT(@TipoReporteId, ',')) )
		AND  (CAST(FechaReporte AS DATE) BETWEEN CAST(@FechaInicial AS DATE) AND CAST(@FechaFinal AS DATE))
	ORDER BY 
		FechaReporte
 END