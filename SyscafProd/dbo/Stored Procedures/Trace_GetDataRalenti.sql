
-- =============================================
-- Author:     ygonzalez
-- Create Date: 27/07/2021
-- Description: Trae los ralenti por periodo
-- =============================================
CREATE PROCEDURE dbo.Trace_GetDataRalenti
AS
BEGIN

    DECLARE @TablaRalenti AS TABLE
    (
        Fecha DATETIME NOT NULL,
        AssetIds INT NOT NULL,
        CombustibleEvento DECIMAL(11, 4) NOT NULL,
        CombustibleViaje DECIMAL(11, 4) NOT NULL
    );
    DECLARE @RalentiFranjaHoraria AS TABLE
    (
        Fecha DATETIME NOT NULL,
        AssetIds INT NOT NULL,
        Hour INT NOT NULL,
        TotalRalenti INT NOT NULL
    );

    INSERT INTO @TablaRalenti
    (
        Fecha,
        CombustibleEvento,
        CombustibleViaje,
        AssetIds
    )
    SELECT Fecha,
           ISNULL(CombustibleEvento, 0) CombustibleEvento,
           ISNULL(CombustibleViaje, 0) CombustibleViaje,
           assetIdS
    FROM
    (
        SELECT DEventos.AssetId,
               DEventos.Fecha,
               CombustibleEvento,
               CombustibleViaje
        FROM
        (
            SELECT AssetId,
                   CAST(StartDateTime AS DATE) Fecha,
                   SUM(FuelUsedLitres) CombustibleEvento
            FROM #Trace_Eventos
            WHERE EventTypeId = -3393530750645328945
            GROUP BY AssetId,
                     CAST(StartDateTime AS DATE)
        ) AS DEventos
            LEFT OUTER JOIN
            (
                SELECT AssetId,
                       CAST(TripStart AS DATE) Fecha,
                       SUM(FuelUsedLitres) CombustibleViaje
                FROM #Trace_Trips
                GROUP BY AssetId,
                         CAST(TripStart AS DATE)
            ) AS DViajes
                ON DEventos.AssetId = DViajes.AssetId
                   AND DEventos.Fecha = DViajes.Fecha
    ) AS r
        INNER JOIN TB_Assets AS a
            ON a.assetId = r.assetId;

    -- Actualiza la tabla para los eventos qiue cambiarion o se adicionaron 
    UPDATE tr
    SET tr.CombustibleEvento = tt.CombustibleEvento + tr.CombustibleEvento,
        tr.CombustibleViaje = tt.CombustibleViaje + tr.CombustibleViaje
    FROM @TablaRalenti AS tt
        INNER JOIN TRACE.Ralenti AS tr
            ON (
                   tr.AssetIds = tt.AssetIds
                   AND tr.Fecha = tt.Fecha
               );

    -- inserta lo que hace falta 
    INSERT INTO TRACE.Ralenti
    (
        Fecha,
        CombustibleEvento,
        CombustibleViaje,
        AssetIds,
        FechaSistema
    )
    SELECT tt.Fecha,
           tt.CombustibleEvento,
           tt.CombustibleViaje,
           tt.AssetIds,
           DATEADD(HOUR, -5, GETDATE())
    FROM @TablaRalenti AS tt
        LEFT OUTER JOIN TRACE.Ralenti AS tr
            ON (
                   tr.AssetIds = tt.AssetIds
                   AND tr.Fecha = tt.Fecha
               )
    WHERE tr.AssetIds IS NULL;


    INSERT INTO @RalentiFranjaHoraria
    (
        AssetIds,
        Fecha,
        Hour,
        TotalRalenti
    )
    SELECT assetIdS,
           Fecha,
           Hour,
           TotalRalenti
    FROM
    (
        (SELECT AssetId,
                CAST(StartDateTime AS DATE) Fecha,
                DATEPART(HOUR, StartDateTime) Hour,
                SUM(TotalTimeSeconds) TotalRalenti
         FROM #Trace_Eventos
		 WHERE EventTypeId = -3393530750645328945
         GROUP BY AssetId,
                  CAST(StartDateTime AS DATE),
                  DATEPART(HOUR, StartDateTime))
    ) AS o
        INNER JOIN TB_Assets AS a
            ON a.assetId = o.assetId;



    INSERT INTO TRACE.RalentiFranjaHoraria
    (
        AssetIds,
        Fecha,
        Hour,
        TotalRalenti,
        FechaSistema
    )
    SELECT dr.AssetIds,
           dr.Fecha,
           dr.Hour,
           dr.TotalRalenti,
           DATEADD(HOUR, -5, GETDATE())
    FROM @RalentiFranjaHoraria AS dr
        LEFT OUTER JOIN TRACE.RalentiFranjaHoraria AS tr
            ON (
                   tr.AssetIds = dr.AssetIds
                   AND tr.Fecha = dr.Fecha
                   AND tr.Hour = dr.Hour
               )
    WHERE tr.AssetIds IS NULL;

	  -- Actualiza la tabla para los eventos qiue cambiarion o se adicionaron para ralenti franja hroaria
    UPDATE tr
    SET tr.TotalRalenti = dr.TotalRalenti + tr.TotalRalenti
     FROM @RalentiFranjaHoraria AS dr
        INNER JOIN TRACE.RalentiFranjaHoraria AS tr
            ON (
                   tr.AssetIds = dr.AssetIds
                   AND tr.Fecha = dr.Fecha
                   AND tr.Hour = dr.Hour
               )


END;
