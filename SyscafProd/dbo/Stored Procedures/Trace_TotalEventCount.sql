
-- =============================================
-- Author:     ygonzalez
-- Create Date: 27/07/2021
-- Description: Trae los datos Mekanicals skills por periodo
-- =============================================
--EXEC [dbo].[Trace_TotalEventCount] '20210806', '20211001', 49, 909

CREATE PROCEDURE dbo.Trace_TotalEventCount
(@Clasificacion AS INT)
AS
BEGIN

    DECLARE @Msk AS TABLE
    (
        Fecha DATETIME NOT NULL,
        AssetIds INT NOT NULL,
        EventTypeId BIGINT NOT NULL,
        TotalOcurrencias INT NOT NULL,
        TotalTimeSeconds INT
    );


    INSERT INTO @Msk
    (
        Fecha,
        EventTypeId,
        TotalOcurrencias,
        TotalTimeSeconds,
        AssetIds
    )
    SELECT Fecha,
           EventTypeId,
           TotalOcurrencias = ISNULL(TotalOcurrencias, 0),
           TotalTimeSeconds = ISNULL(TotalTimeSeconds, 0),
           assetIdS
    FROM
    (
        SELECT AssetId,
               EventTypeId,
               CAST(StartDateTime AS DATE) Fecha,
               TotalOcurrencias = SUM(TotalOccurances),
               TotalTimeSeconds = SUM(TotalTimeSeconds)
        FROM #Trace_Eventos
        WHERE (
                  @Clasificacion = 46
                  AND EventTypeId IN ( -474118258489130417, 7018592833329676384 )
              )
              OR
              (
                  @Clasificacion = 47
                  AND EventTypeId IN ( 26438716926357308 )
              )
              OR
              (
                  @Clasificacion = 49
                  AND EventTypeId IN ( 8139448790035307011, -2090218144268250614, -6915945686263892437,
                                       -9073776053423883317
                                     )
              )
        GROUP BY AssetId,
                 EventTypeId,
                 CAST(StartDateTime AS DATE)
    ) AS DEventos
        INNER JOIN TB_Assets AS a
            ON a.assetId = DEventos.assetId;




    -- Actualiza la tabla para los eventos qiue cambiarion o se adicionaron 
    UPDATE tr
    SET tr.TotalOcurrencias = tt.TotalOcurrencias + tr.TotalOcurrencias,
        tr.TotalTimeSeconds = tt.TotalTimeSeconds + tr.TotalTimeSeconds
    FROM @Msk AS tt
        INNER JOIN TRACE.TotalEventCount AS tr
            ON (
                   tr.AssetIds = tt.AssetIds
                   AND tr.Fecha = tt.Fecha
                   AND tr.EventTypeId = tt.EventTypeId
               );

    -- inserta lo que hace falta 
    INSERT INTO TRACE.TotalEventCount
    (
        Fecha,
        EventTypeId,
        AssetIds,
        FechaSistema,
        TotalOcurrencias,
        TotalTimeSeconds,
        Clasificacion
    )
    SELECT tt.Fecha,
           tt.EventTypeId,
           tt.AssetIds,
           DATEADD(HOUR, -5, GETDATE()),
           tt.TotalOcurrencias,
           tt.TotalTimeSeconds,
           @Clasificacion
    FROM @Msk AS tt
        LEFT OUTER JOIN TRACE.TotalEventCount AS tr
            ON (
                   tr.AssetIds = tt.AssetIds
                   AND tr.Fecha = tt.Fecha
                   AND tr.EventTypeId = tt.EventTypeId
               )
    WHERE tr.AssetIds IS NULL;

END;

