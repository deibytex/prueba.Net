
-- =============================================
-- Author:     ygonzalez
-- Create Date: 27/07/2021
-- Description: Trae la fuente de la grafica de ralenti por periodo
-- =============================================
CREATE PROCEDURE dbo.Trace_GetDataRendimiento

AS
BEGIN

    DECLARE @Rendimiento AS TABLE
    (
        Fecha DATETIME NOT NULL,
        AssetIds INT NOT NULL,
        FuelUsedLitres DECIMAL(11, 4) NOT NULL,
        HorasMotor DECIMAL(11, 4) NOT NULL,
        DistanciaRecorrida DECIMAL(11, 4) NOT NULL,
        PorRalenti DECIMAL(11, 4) NOT NULL
    );
    --////////////////////////////////////////////////////////////////////////////////////////////
    -- Consolidamos la informacion
    --//////////////////////////////////////////////////////////////////////////////////////////////////

    INSERT INTO @Rendimiento
    (
        Fecha,
        AssetIds,
        FuelUsedLitres,
        HorasMotor,
        DistanciaRecorrida,
        PorRalenti
    )
    SELECT Fecha,
           assetIdS,
           FuelUsedLitres = ISNULL(FuelUsedLitres, 0.0),
           HorasMotor = ISNULL(HorasMotor, 0.0),
           DistanciaRecorrida = ISNULL(DistanciaRecorrida, 0),
           PorRalenti = CAST(ISNULL(PorRalenti, 0.0) AS DECIMAL(11, 4))
    FROM
    (
        --// consolidamos la informacion de ambas tablas 
        SELECT AssetId,
               CAST(TripStart AS DATE) Fecha,
               FuelUsedLitres = SUM(FuelUsedLitres) / 3.785,
               HorasMotor = SUM(EngineSeconds),
               DistanciaRecorrida = SUM(DistanceKilometers),
               PorRalenti = SUM(IdleTime)
        FROM #Trace_Trips AS t
            LEFT OUTER JOIN #Trace_Metricas AS m
                ON t.TripId = m.TripId
        GROUP BY AssetId,
                 CAST(TripStart AS DATE)
    ) AS r
        INNER JOIN TB_Assets AS a
            ON a.assetId = r.assetId;




    -- Actualiza la tabla para los eventos qiue cambiarion o se adicionaron 
    UPDATE tr
    SET tr.FuelUsedLitres = tt.FuelUsedLitres + tr.FuelUsedLitres,
        tr.HorasMotor = tt.HorasMotor + tr.HorasMotor,
        tr.DistanciaRecorrida = tt.DistanciaRecorrida + tr.DistanciaRecorrida,
        tr.PorRalenti = tt.PorRalenti + tr.PorRalenti
    FROM @Rendimiento AS tt
        INNER JOIN TRACE.Rendimiento AS tr
            ON (
                   tr.AssetIds = tt.AssetIds
                   AND tr.Fecha = tt.Fecha
               );

    -- inserta lo que hace falta 
    INSERT INTO TRACE.Rendimiento
    (
        Fecha,
        AssetIds,
        FuelUsedLitres,
        HorasMotor,
        DistanciaRecorrida,
        PorRalenti,
        FechaSistema
    )
    SELECT tt.Fecha,
           tt.AssetIds,
           tt.FuelUsedLitres,
           tt.HorasMotor,
           tt.DistanciaRecorrida,
           tt.PorRalenti,
           DATEADD(HOUR, -5, GETDATE())
    FROM @Rendimiento AS tt
        LEFT OUTER JOIN TRACE.Rendimiento AS tr
            ON (
                   tr.AssetIds = tt.AssetIds
                   AND tr.Fecha = tt.Fecha
               )
    WHERE tr.AssetIds IS NULL;



    -- Actualiza la tabla para los eventos qiue cambiarion o se adicionaron 
    UPDATE tr
    SET tr.FuelUsedLitres = ISNULL(tt.FuelUsedLitres,0) + tr.FuelUsedLitres,
        tr.distanceKilometers = tt.distanceKilometers + tr.distanceKilometers,
        tr.EngineSeconds = tt.EngineSeconds + tr.EngineSeconds
    FROM
    (
        SELECT Fecha = CAST(tripstart AS DATE),
               assetIdS,
               distanceKilometers = SUM(distanceKilometers),
               FuelUsedLitres = SUM(FuelUsedLitres),
               EngineSeconds = SUM(EngineSeconds)
        FROM #Trace_Trips AS tt
            INNER JOIN TB_Assets AS a
                ON a.assetId = tt.assetId
        GROUP BY CAST(tripstart AS DATE),
                 a.assetIdS
    ) AS tt
        INNER JOIN TRACE.ConsolidadoViajes AS tr
            ON (
                   tr.AssetIds = tt.assetIdS
                   AND tr.Fecha = tt.Fecha
               );

    -- inserta lo que hace falta 
    INSERT INTO TRACE.ConsolidadoViajes
    (
        Fecha,
        AssetIds,
        distanceKilometers,
        FuelUsedLitres,
        EngineSeconds,
        FechaSistema
    )
    SELECT tt.Fecha,
           tt.AssetIds,
           tt.distanceKilometers,
           ISNULL(tt.FuelUsedLitres,0),
           tt.EngineSeconds,
           DATEADD(HOUR, -5, GETDATE())
    FROM
    (
        SELECT Fecha = CAST(tripstart AS DATE),
               assetIdS,
               distanceKilometers = SUM(distanceKilometers),
               FuelUsedLitres = ISNULL(SUM(FuelUsedLitres),0),
               EngineSeconds = SUM(EngineSeconds)
        FROM #Trace_Trips AS tt
            INNER JOIN TB_Assets AS a
                ON a.assetId = tt.assetId
        GROUP BY CAST(tripstart AS DATE),
                 a.assetIdS
    ) AS tt
        LEFT OUTER JOIN TRACE.ConsolidadoViajes AS tr
            ON (
                   tr.AssetIds = tt.AssetIds
                   AND tr.Fecha = tt.Fecha
               )
    WHERE tr.AssetIds IS NULL;

END;
