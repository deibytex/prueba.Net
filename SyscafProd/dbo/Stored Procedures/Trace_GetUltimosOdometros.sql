
-- =============================================
-- Author:     ygonzalez
-- Create Date: 27/07/2021
-- Description: Trae los odometros por periodo
-- =============================================
CREATE PROCEDURE dbo.Trace_GetUltimosOdometros
AS
BEGIN


    DECLARE @FechaSistema DATETIME =
            (
                SELECT GETDATE()AT TIME ZONE 'SA Pacific Standard Time'
            );
    INSERT INTO TRACE.UltimoOdometro
    (
        Fecha,
        AssetIds,
        MaximoOdometro,
        FechaSistema
    )
    SELECT result.MaxFecha,
           result.assetIdS,
           result.MaximoOdometro,
           @FechaSistema
    FROM
    (
        SELECT MaxFecha,
               assetIdS,
               MaximoOdometro
        FROM
        (
            (SELECT AssetId,
                    MaxFecha = CAST(MAX(TripStart) AS DATE),
                    MAX(endOdometerKilometers) MaximoOdometro
             FROM #Trace_Trips
             GROUP BY AssetId,
                      CAST((TripStart) AS DATE))
        ) AS o
            INNER JOIN TB_Assets AS a
                ON a.assetId = o.assetId
    ) AS result
        LEFT OUTER JOIN TRACE.UltimoOdometro AS ul
            ON (
                   ul.AssetIds = result.assetIdS
                   AND ul.Fecha = result.MaxFecha
               )
    WHERE ul.AssetIds IS NULL;

    UPDATE ul
    SET ul.MaximoOdometro = result.MaximoOdometro
    FROM
    (
        SELECT MaxFecha,
               assetIdS,
               MaximoOdometro
        FROM
        (
            (SELECT AssetId,
                    MaxFecha = CAST(MAX(TripStart) AS DATE),
                    MAX(endOdometerKilometers) MaximoOdometro
             FROM #Trace_Trips
             GROUP BY AssetId,
                      CAST((TripStart) AS DATE))
        ) AS o
            INNER JOIN TB_Assets AS a
                ON a.assetId = o.assetId
    ) AS result
        INNER JOIN TRACE.UltimoOdometro AS ul
            ON (
                   ul.AssetIds = result.assetIdS
                   AND ul.Fecha = result.MaxFecha
               );



END;
