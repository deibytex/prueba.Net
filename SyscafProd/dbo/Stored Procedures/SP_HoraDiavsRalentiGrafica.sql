

-- =============================================
-- Author:		MD
-- Create date: 31/03/2021
-- Description:	Ralenti vs Hora del dia Grafica
-- =============================================
CREATE PROCEDURE [dbo].[SP_HoraDiavsRalentiGrafica]
	@FechaInicial DATETIME,
	@FechaFinal	DATETIME,
	@ClienteIds INT,
	@assetId INT = NULL,
	@registrationNumber VARCHAR(200) = NULL
AS
BEGIN
	SELECT 
		DATEPART(HOUR,TripStart) AS Hora,
		SUM(IdleOccurs) AS Valor
	FROM 
		TB_TripsMetrics TM
			INNER JOIN
		TB_Assets Vh
				ON
					TM.AssetId = Vh.assetId
	WHERE
		TM.ClienteIds = @ClienteIds
			AND
		TM.TripStart BETWEEN @FechaInicial AND @FechaFinal
			AND
		(@assetId IS NULL OR TM.AssetId = @assetId)
			AND
		(@registrationNumber IS NULL OR Vh.registrationNumber LIKE '%'+ @registrationNumber +'%')
	GROUP BY 
		DATEPART(HOUR,TripStart)
	ORDER BY
		DATEPART(HOUR,TripStart) ASC
END