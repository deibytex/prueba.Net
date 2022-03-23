CREATE PROCEDURE [dbo].[SP_InsertUpdateReporteTrace]
(
	@ClienteIds					INT,
	@FechaReporte				DATETIME,
	@TipoReporteId				INT,
	@FechaFinal					DATETIME,
	@InformacionExtendida		VARCHAR(MAX),
	@DataReporte				VARCHAR(MAX),
	@FechaSistema				DATETIME,
	@EsActivo					BIT
)
AS
DECLARE
	--=========================================================
	-- SE DECLARAN LAS VARIABLES
	--=========================================================
	@ReporteDashboradID INT = NULL
BEGIN
	--==============================================================================
	-- SE REVISA QUE EXISTA EL REPORTE SI EXISTE SE ACTUALIZA SINO SE INSERTA NUEVO
	--==============================================================================
	IF EXISTS (SELECT ReporteDashboradID,ClienteIds,DataReporte FROM TB_ReporteDashboard WHERE ClienteIds =  @ClienteIds AND FechaReporte = @FechaReporte AND TipoReporteId = @TipoReporteId)
	BEGIN
		BEGIN TRY
			BEGIN TRAN
				--=======================================================================
				-- SE CONSULTA EL ID DEL REGISTRO PARA ACTUALIZAR
				--=======================================================================
				SELECT
					@ReporteDashboradID = RD.ReporteDashboradID
				FROM 
					TB_ReporteDashboard RD WITH (NOLOCK)
				WHERE 
					(RD.ClienteIds = @ClienteIds)
						AND 
					(RD.FechaReporte = @FechaReporte)
						AND 
					(RD.TipoReporteId = @TipoReporteId)

				--=======================================================================
				-- SE ACTUALIZA EL REGISTRO DEL REPORTE
				--=======================================================================
				UPDATE       
					dbo.TB_ReporteDashboard
				SET 
					ClienteIds = @ClienteIds
					,FechaReporte = @FechaReporte
					,FechaFinal = @FechaFinal
					,InformacionExtendida = @InformacionExtendida
					,DataReporte = @DataReporte
					,FechaSistema = @FechaSistema
					,EsActivo = ISNULL(@EsActivo,1)
					,TipoReporteId = @TipoReporteId
				WHERE
					ReporteDashboradID = @ReporteDashboradID
			COMMIT
		END TRY
		BEGIN CATCH
			IF @@TRANCOUNT >0
				ROLLBACK
				--=======================================================================
				-- SI HAY ALGUN PROBLEMA DEVUELVE LA OPERACION
				--=======================================================================
		END CATCH
	END
	ELSE
	BEGIN
		BEGIN TRY
			BEGIN TRAN	
				--=======================================================================
				-- SE INSERTA EL REPORTE
				--=======================================================================
				INSERT INTO dbo.TB_ReporteDashboard (
					 ClienteIds
					,FechaReporte
					,TipoReporteId
					,FechaFinal
					,InformacionExtendida
					,DataReporte
					,FechaSistema
					,EsActivo, mes, dia , anio
				)
				VALUES 
				(
					@ClienteIds,
					@FechaReporte,
					@TipoReporteId,
					@FechaFinal,
					@InformacionExtendida,
					@DataReporte,
					@FechaSistema,
					ISNULL(@EsActivo,1), 
					month(@FechaReporte),
					day(@FechaReporte),
					year(@FechaReporte)
				);
		COMMIT
		END TRY
		BEGIN CATCH
			IF @@TRANCOUNT >0
				ROLLBACK
				--=======================================================================
				-- SI HAY ALGUN PROBLEMA DEVUELVE LA OPERACION
				--=======================================================================
		END CATCH
	END
END