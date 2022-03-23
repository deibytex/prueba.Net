
-- =============================================
-- Author:     ygonzalez
-- Create Date: 27/07/2021
-- Description: Trae la fuente de la grafica de ralenti por periodo
-- =============================================
CREATE PROCEDURE dbo.Trace_GetDataZonaOperacion
(
    @Tipo AS INT
)
AS
BEGIN

    DECLARE @VelocidadOperacion AS TABLE
    (
        Fecha DATETIME NOT NULL,
        AssetIds INT NOT NULL,
        TotalTimeSeconds INT NOT NULL,
        EventTypeId BIGINT NOT NULL
    );

    --////////////////////////////////////////////////////////////////////////////////////////////
    -- Consolidamos la informacion
    --//////////////////////////////////////////////////////////////////////////////////////////////////
    INSERT INTO @VelocidadOperacion
    (
        Fecha,
        TotalTimeSeconds,
        AssetIds,
        EventTypeId
    )
    SELECT Fecha,
           ISNULL(TotalTimeSeconds, 0),
           assetIdS,
           EventTypeId
    FROM
    (
        SELECT DEventos.AssetId,
               EventTypeId,
               DEventos.Fecha,
               TotalTimeSeconds
        FROM
        (
            SELECT AssetId,
                   CAST(StartDateTime AS DATE) Fecha,
                   TotalTimeSeconds = SUM(TotalTimeSeconds),
                   EventTypeId
            FROM #Trace_Eventos
            WHERE (
                      @Tipo = 1
                      AND EventTypeId IN ( -1380943868608681088, -2991748809485560259, -7384133594607552166,
                                           -5845957306678822475, 604120741568162607, -6565644534883148175,
                                           -3174753191525397774, 2017364983261124560
                                         )
                  )
                  OR
                  (
                      @Tipo = 2
                      AND EventTypeId IN ( 3088119264815087296, 2520207866424091425, -7805330071530369222,
                                           8735594133560600960, -5212524000454609379, -4420544020334837126
                                         )
                  )
            GROUP BY AssetId,
                     EventTypeId,
                     CAST(StartDateTime AS DATE)
        ) AS DEventos
    ) AS r
        INNER JOIN TB_Assets AS a
            ON CAST(a.assetId  AS BIGINT) = r.assetId
   


    -- Actualiza la tabla para los eventos qiue cambiarion o se adicionaron 
    UPDATE tr
    SET tr.TotalTimeSeconds = tt.TotalTimeSeconds + tr.TotalTimeSeconds
    FROM @VelocidadOperacion AS tt
        INNER JOIN TRACE.VelocidadOperacion AS tr
            ON (
                   tr.AssetIds = tt.AssetIds
                   AND tr.Fecha = tt.Fecha
                   AND tr.EventTypeId = tt.EventTypeId
				   AND tr.Tipo = @Tipo
               );

    -- inserta lo que hace falta 
    INSERT INTO TRACE.VelocidadOperacion
    (
        Fecha,
        TotalTimeSeconds,
        AssetIds,
        EventTypeId,
        Tipo,
        FechaSistema
    )
    SELECT tt.Fecha,
           tt.TotalTimeSeconds,
           tt.AssetIds,
           tt.EventTypeId,
           @Tipo,
           DATEADD(HOUR, -5, GETDATE())
    FROM @VelocidadOperacion AS tt
        LEFT OUTER JOIN TRACE.VelocidadOperacion AS tr
            ON (
                   tr.AssetIds = tt.AssetIds
                   AND tr.Fecha = tt.Fecha
                   AND tr.EventTypeId = tt.EventTypeId
               )
    WHERE tr.AssetIds IS NULL;

END;
