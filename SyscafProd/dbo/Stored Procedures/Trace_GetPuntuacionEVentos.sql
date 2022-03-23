
-- =============================================
-- Author:     ygonzalez
-- Create Date: 27/07/2021
-- Description: Trae los datos Pedales por periodo
-- =============================================
CREATE PROCEDURE dbo.Trace_GetPuntuacionEVentos
AS
BEGIN

    DECLARE @Safeti AS TABLE
    (
        Fecha DATETIME NOT NULL,
        AssetIds INT NOT NULL,
        TotalTimeSeconds INT NOT NULL,
        distanceKilometers DECIMAL(11, 4) NOT NULL,
        TotalOccurances INT NOT NULL,
        EventTypeId BIGINT NOT NULL
    );
    INSERT INTO @Safeti
    (
        Fecha,
        EventTypeId,
        AssetIds,
        TotalTimeSeconds,
        distanceKilometers,
        TotalOccurances
    )
    SELECT Fecha,
           EventTypeId,
           assetIdS,
           TotalTimeSeconds,
           0,
           TotalOccurances
    FROM
    (
        SELECT DEventos.AssetId,
               EventTypeId,
               DEventos.Fecha,
               TotalTimeSeconds,
               TotalOccurances
        FROM
        (
            SELECT AssetId,
                   EventTypeId,
                   CAST(StartDateTime AS DATE) Fecha,
                   TotalTimeSeconds = SUM(TotalTimeSeconds),
                   TotalOccurances = SUM(TotalOccurances)
            FROM #Trace_Eventos AS p
            WHERE EventTypeId IN ( 4750800303282680186, 6454149451280645233, -2543341242566313180, -3890646499157906515 )
            GROUP BY AssetId,
                     EventTypeId,
                     CAST(StartDateTime AS DATE)
        ) AS DEventos
    ) AS r
        INNER JOIN TB_Assets AS a
            ON a.assetId = r.assetId
    ORDER BY assetIdS,
             EventTypeId;


    -- Actualiza la tabla para los eventos qiue cambiarion o se adicionaron 
    UPDATE tr
    SET tr.distanceKilometers = tt.distanceKilometers + tr.distanceKilometers,
        tr.TotalOccurances = tt.TotalOccurances + tr.TotalOccurances,
        tr.TotalTimeSeconds = tt.TotalTimeSeconds + tr.TotalTimeSeconds
    FROM @Safeti AS tt
        INNER JOIN TRACE.Safeti AS tr
            ON (
                   tr.AssetIds = tt.AssetIds
                   AND tr.Fecha = tt.Fecha
                   AND tr.EventTypeId = tt.EventTypeId
               );

    -- inserta lo que hace falta 
    INSERT INTO TRACE.Safeti
    (
        Fecha,
        EventTypeId,
        AssetIds,
        FechaSistema,
        TotalTimeSeconds,
        distanceKilometers,
        TotalOccurances
    )
    SELECT tt.Fecha,
           tt.EventTypeId,
           tt.AssetIds,
           DATEADD(HOUR, -5, GETDATE()),
           tt.TotalTimeSeconds,
           tt.distanceKilometers,
           tt.TotalOccurances
    FROM @Safeti AS tt
        LEFT OUTER JOIN TRACE.Safeti AS tr
            ON (
                   tr.AssetIds = tt.AssetIds
                   AND tr.Fecha = tt.Fecha
                   AND tr.EventTypeId = tt.EventTypeId
               )
    WHERE tr.AssetIds IS NULL;


END;


