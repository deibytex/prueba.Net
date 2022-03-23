
--======================================================================================
--FECHA: 08/02/2021
--DESCRIPCION: VISTA PARA TRAER LOS VEHICULOS SIN CONEXION
--AUTOR: MD
--======================================================================================
--select * from [dbo].[VW_VehiculosSinTransmision]
CREATE FUNCTION dbo.fn_VehiculosSinTransmision
(
    @FechaActual DATETIME
)
RETURNS TABLE
AS
RETURN
(
    SELECT TP.assetIdS,
           DATEDIFF(DAY, MAX(TP.Timestamp), @FechaActual) DiasSinTx,
           MAX(TP.Timestamp) UltimoAvl
    FROM dbo.TB_Positions AS TP
        LEFT JOIN dbo.TB_Senales AS TS
            ON TS.assetsIdS = TP.assetIdS
        INNER JOIN dbo.TB_Assets AS TA
            ON TA.assetIdS = TP.assetIdS
    WHERE TA.estadoClienteIdS = 1
    GROUP BY TP.assetIdS,
             TS.diasTransmision
    HAVING ((DATEDIFF(MINUTE, MAX(TP.Timestamp), @FechaActual) / 1440) >= ISNULL(TS.diasTransmision, 1))
);



