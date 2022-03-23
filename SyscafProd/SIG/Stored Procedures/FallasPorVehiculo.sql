-- =============================================
-- Author:      ygonzalez
-- Create Date: 2021.12.02
-- Description: Trae el discriminado por vehiculo de fallas
-- =============================================

--EXEC SIG.FallasPorVehiculo '20211130', '20211130 23:59:59', 
CREATE PROCEDURE SIG.FallasPorVehiculo
(
    @FechaInicial DATETIME,
    @FechaFinal DATETIME,
    @AssetId bigint
)
AS
BEGIN
    --DECLARE @FechaInicial DATETIME = '20211130',
    --        @FechaFinal DATETIME = '20211202'
    

    SELECT TA.assetsDescription,
           TA.registrationNumber,
           Total.ClienteIds,
           AssetId = Total.AssetIds,
           Falla = TFS.Nombre,
           Total.TotalFallas,
		   TFS.FallaSenialId
    FROM
    (
        SELECT TDS.AssetIds,
               TDS.ClienteIds,
               --  CAST(TDS.FechaInicial AS DATE) Fecha,
               TDS.FallaSenialId,
               TotalFallas = COUNT(1)
        FROM SIG.TB_DataSeniales AS TDS
        WHERE TDS.FechaInicial
              BETWEEN @FechaInicial AND @FechaFinal
              AND TDS.AssetIds =  @AssetId
        GROUP BY TDS.AssetIds,
                 TDS.ClienteIds,
                 TDS.FallaSenialId
    ) AS Total
        INNER JOIN dbo.TB_Assets AS TA
            ON CAST(TA.assetId AS BIGINT) = Total.AssetIds
        INNER JOIN SIG.TB_FallaSenial AS TFS
            ON TFS.FallaSenialId = Total.FallaSenialId
    ORDER BY Total.AssetIds,
             Total.ClienteIds;


END;
