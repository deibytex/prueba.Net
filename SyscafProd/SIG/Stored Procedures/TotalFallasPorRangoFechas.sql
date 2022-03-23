-- =============================================
-- Author:      ygonzalez
-- Create Date: 2021.12.02
-- Description: Trae el total de falla por vehiculos por rango de fechas
-- =============================================
--EXEC SIG.TotalFallasPorRangoFechas '20211130' ,  '20211202', -1
CREATE PROCEDURE [SIG].[TotalFallasPorRangoFechas]
(
    @FechaInicial DATETIME,
    @FechaFinal DATETIME,
    @ClienteIds INT
)
AS
BEGIN

    SELECT TC.clienteNombre,
           TS.siteName,
           TA.assetsDescription,
           TA.registrationNumber,
           Total.ClienteIds,
           AssetId = Total.AssetIds,
           --Total.Fecha,
           FechaInicial = CAST(MIN(Total.FechaInicial) AS DATE),
           FechaFinal = CAST(MAX(Total.FechaFinal) AS DATE),
           TFallas = COUNT(1)
    FROM
    (
        SELECT TDS.AssetIds,
               TDS.ClienteIds,
               --  CAST(TDS.FechaInicial AS DATE) Fecha,
               TDS.FallaSenialId,
               FechaInicial = MIN(TDS.FechaInicial),
               FechaFinal = MAX(TDS.FechaFinal)
        FROM SIG.TB_DataSeniales AS TDS
        WHERE TDS.FechaInicial
              BETWEEN @FechaInicial AND @FechaFinal
              AND
              (
                  @ClienteIds = -1
                  OR TDS.ClienteIds = @ClienteIds
              )
        GROUP BY TDS.ClienteIds,
                 TDS.AssetIds,                 
                 TDS.FallaSenialId
    ) AS Total
        INNER JOIN dbo.TB_Cliente AS TC
            ON TC.clienteIdS = Total.ClienteIds
        INNER JOIN dbo.TB_Assets AS TA
            ON CAST(TA.assetId AS BIGINT) = Total.AssetIds
        INNER JOIN dbo.TB_Site AS TS
            ON TS.siteIdS = TA.siteIdS
    GROUP BY TC.clienteNombre,
             TS.siteName,
             TA.assetsDescription,
             TA.registrationNumber,
             Total.AssetIds,
             Total.ClienteIds
    ORDER BY Total.AssetIds,
             Total.ClienteIds;

END;
