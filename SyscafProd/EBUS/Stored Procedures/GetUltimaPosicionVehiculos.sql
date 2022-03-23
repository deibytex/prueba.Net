-- =============================================
-- Author:		ygonzalez
-- Create date: 26.08.2021
-- Description:	 trae la información de la ultima posicion de los vehiculos juntos con sus localizaciones
-- =============================================
CREATE PROCEDURE EBUS.GetUltimaPosicionVehiculos
(
    @ClienteIds INT,
    @Periodo VARCHAR(10)
)
AS
BEGIN


--DECLARE @ClienteIds INT = 914,
--        @Periodo VARCHAR(10) = '22022';
DECLARE @Sql NVARCHAR(MAX);

IF OBJECT_ID('tempdb..#tempEbusPositions') IS NOT NULL
    DROP TABLE #tempEbusPositions;
CREATE TABLE #tempEbusPositions
(
    assetId BIGINT NOT NULL,
    driverId BIGINT NOT NULL,
    Timestamp DATETIME NULL,
    Latitud FLOAT NULL,
    Longitud FLOAT NULL,
    FormattedAddress VARCHAR(200) NULL
);


SET @Sql
    = N'INSERT INTO #tempEbusPositions
				(   
					assetId,
					driverId,
					Timestamp,
					Latitud,
					Longitud
				)
				SELECT P.assetId,
                   P.driverId,
                   P.Timestamp,
                   P.Latitud,
                   P.Longitud
            FROM
            (
                SELECT ROW_NUMBER() OVER (PARTITION BY TP.assetId ORDER BY TP.Timestamp DESC) nr,
                       TP.assetId,
                       TP.driverId,
                       TP.Timestamp,
                       TP.Latitud,
                       TP.Longitud
                 FROM PORTAL.TB_Positions_' + @Periodo + N'_' + CAST(@ClienteIds AS VARCHAR)
      + N' AS TP
            ) AS P
            WHERE P.nr = 1	';


			
EXEC sp_executesql @Sql;



SELECT LocationId = CASE L.Localizado
                        WHEN 0 THEN
                            0
                        ELSE
                            L.LocationId
                    END,
       Localizacion = CASE L.Localizado
                          WHEN 0 THEN
                              ''
                          ELSE
                              L.Name
                      END,
       Placa = L.assetsDescription,
       L.Latitud,
       L.Longitud,
       L.AVL,
       AssetId = CAST(L.assetId AS VARCHAR(50))
FROM
(
    SELECT menorarea = ROW_NUMBER() OVER (PARTITION BY l.assetsDescription ORDER BY l.Localizado DESC, l.Area),
           l.LocationId,
           l.Area,
           l.Name,
           l.assetsDescription,
           l.Latitud,
           l.Longitud,
           l.Timestamp AVL,
           l.Localizado,
           l.assetId
    FROM
    (
        SELECT L.LocationId,
               Area = CASE L.ShapeType
                          WHEN 'Circle' THEN
                              geometry::STGeomFromText(L.ShapeWktValid, 4326).STDistance(geometry::STGeomFromText(
                                                                                                                  'POINT('
                                                                                                                  + CAST(TP.Longitud AS VARCHAR(50))
                                                                                                                  + ' '
                                                                                                                  + CAST(TP.Latitud AS VARCHAR(50))
                                                                                                                  + ')',
                                                                                                                  0
                                                                                                              )
                                                                                     )
                          ELSE
                              geometry::STGeomFromText(L.ShapeWktValid, 0).STArea()
                      END,
               Localizado = CASE L.ShapeType
                                WHEN 'Circle' THEN
                                    CASE
                                        WHEN (geometry::STGeomFromText(L.ShapeWktValid, 4326).STDistance(geometry::STGeomFromText(
                                                                                                                                  'POINT('
                                                                                                                                  + CAST(TP.Longitud AS VARCHAR(50))
                                                                                                                                  + ' '
                                                                                                                                  + CAST(TP.Latitud AS VARCHAR(50))
                                                                                                                                  + ')',
                                                                                                                                  0
                                                                                                                              )
                                                                                                     ) < L.Radius
                                             ) THEN
                                            1
                                        ELSE
                                            0
                                    END
                                ELSE
                                    geometry::STGeomFromText(L.ShapeWktValid, 0).STContains(geometry::STGeomFromText(
                                                                                                                        'POINT('
                                                                                                                        + CAST(TP.Longitud AS VARCHAR(50))
                                                                                                                        + ' '
                                                                                                                        + CAST(TP.Latitud AS VARCHAR(50))
                                                                                                                        + ')',
                                                                                                                        0
                                                                                                                    )
                                                                                           )
                            END,
               TP.Latitud,
               TP.Longitud,
               TP.Timestamp,
               TA.assetsDescription,
               L.Name,
               TP.assetId
        FROM #tempEbusPositions AS TP
            INNER JOIN
            (
                SELECT *
                FROM PORTAL.Locations AS L2
                WHERE L2.ClienteIds = @ClienteIds
                      AND L2.IsParqueo = 1
                      AND L2.IsDeleted = 0
            ) AS L
                ON L.ClienteIds = @ClienteIds
            INNER JOIN dbo.TB_Assets AS TA
                ON TA.assetId = TP.assetId
        WHERE TA.estadoClienteIdS = 1
    ) AS l
) AS L
WHERE menorarea = 1

ORDER BY L.assetsDescription;



END;
