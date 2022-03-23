CREATE PROCEDURE dbo.SP_ReporteSimCard
    -- DECLARE
    @ProcesoGeneracionDatosId INT = NULL
AS
BEGIN

    SELECT TPSC.Placa,
           TPSC.UltimoAvl,
           CASE
               WHEN UltimoAvl = LAG(UltimoAvl) OVER (PARTITION BY Placa ORDER BY FechaSistema) THEN
                   0
               ELSE
                   1
           END AS TieneMovimiento,
           Latitud = ISNULL(TPSC.Latitud, 0),
           Longitud = ISNULL(TPSC.Longitud, 0),
           FechaSistema AS FechaLectura,
           TPSC.ProcesoGeneracionDatosId
    FROM dbo.TB_PruebaSimCard AS TPSC
    WHERE ProcesoGeneracionDatosId = @ProcesoGeneracionDatosId;

END;
