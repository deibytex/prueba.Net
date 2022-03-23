-- =============================================
-- Author:      ygonzalez
-- Create Date: 2021.12.02
-- Description: Trae el reporte detallado de fallas por cliente y fechas o vehiculo 
-- =============================================
--EXEC SIG.ReporteFallasPorFechasCliente '20211130' ,  '20211202',886, -1 -9062449582946347556
CREATE PROCEDURE [SIG].[ReporteFallasPorFechasCliente]
(
    @FechaInicial DATETIME,
    @FechaFinal DATETIME,
    @ClienteIds INT,
    @Assetid BIGINT,
    @FallaId INT
)
AS
BEGIN

    --DECLARE @FechaInicial DATETIME = '20211130',
    --        @FechaFinal DATETIME = '20211202',
    --        @ClienteIds INT = 886,
    --        @Assetid INT = -1;
    SELECT TC.clienteNombre,
           TS.siteName,
           TA.assetsDescription,
           TA.registrationNumber,
           Falla = TFS.Nombre,
           Total.Descripcion,
           Total.Fecha,
           Total.Ocurrencias, Total.FechaInicial, Total.FechaFinal
    FROM
    (
        SELECT TDS.AssetIds,
               TDS.ClienteIds,
               CAST(TDS.FechaInicial AS DATE) Fecha,
               TDS.FallaSenialId,
               TDS.Descripcion,
               TDS.Ocurrencias, TDS.FechaInicial, TDS.FechaFinal
        FROM SIG.TB_DataSeniales AS TDS
        WHERE TDS.FechaInicial
              BETWEEN @FechaInicial AND @FechaFinal
              AND
              (
                  @Assetid = -1
                  OR TDS.AssetIds = @Assetid
              )
              AND
              (
                  @ClienteIds = -1
                  OR TDS.ClienteIds = @ClienteIds
              )
              AND
              (
                  @FallaId = -1
                  OR TDS.FallaSenialId = @FallaId
              )
    ) AS Total
        INNER JOIN dbo.TB_Cliente AS TC
            ON TC.clienteIdS = Total.ClienteIds
        INNER JOIN dbo.TB_Assets AS TA
            ON CAST(TA.assetId AS BIGINT) = Total.AssetIds
        INNER JOIN dbo.TB_Site AS TS
            ON TS.siteIdS = TA.siteIdS
        INNER JOIN SIG.TB_FallaSenial AS TFS
            ON TFS.FallaSenialId = Total.FallaSenialId
    ORDER BY  Total.ClienteIds,
              Total.AssetIds;

END;
