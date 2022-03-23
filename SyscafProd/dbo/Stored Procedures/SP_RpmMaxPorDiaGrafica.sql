-- =============================================
-- Author:		MD
-- Create date: 31/03/2021
-- Description:	RMP MAximo por Dia Grafica TRACE
-- =============================================
CREATE PROCEDURE [dbo].[SP_RpmMaxPorDiaGrafica]
	@FechaInicial DATETIME,
	@FechaFinal	DATETIME,
	@ClienteIds INT,
	@assetId	INT = NULL,
	@registrationNumber VARCHAR(200) = NULL
AS
BEGIN
print(@FechaInicial)
print(@FechaFinal)
	SELECT
		Fecha  = CONVERT(varchar(10),tsv.TRIPSTART, 103),
		Valor = SUM(tsv.maxRpm)
	FROM
		dbo.TB_SubViaje tsv WITH (NOLOCK)
			INNER JOIN
		TB_Assets ta
				ON
					tsv.assetIdS = ta.assetIdS
	WHERE
		(tsv.clienteIdS = @ClienteIds)
			AND
		(tsv.tripStart BETWEEN CAST(@FechaInicial as DATE) AND CAST(@FechaFinal AS DATE))
			AND
		(@assetId IS NULL OR tsv.assetId = @assetId)
			AND
		(@registrationNumber IS NULL OR ta.registrationNumber LIKE '%'+ @registrationNumber +'%')
	GROUP BY
		CONVERT(varchar(10),tsv.TRIPSTART, 103)
	ORDER BY
		CONVERT(varchar(10),tsv.TRIPSTART, 103)
END