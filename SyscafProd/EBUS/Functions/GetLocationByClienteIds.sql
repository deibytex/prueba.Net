
-- =============================================
-- Author:		ygonzalez
-- Create date: 26.08.2021
-- Description:	 Identifica si se encentra en una locacion y trae la de menor area
-- =============================================
CREATE FUNCTION EBUS.GetLocationByClienteIds
(
    @ClienteIds INT,
    @Latitud FLOAT,
    @Longitud FLOAT
)
RETURNS FLOAT
AS
BEGIN
--DECLARE   @ClienteIds INT = 858,
--    @Latitud FLOAT = 4.63091659545898		,
--    @Longitud FLOAT = -74.1746139526367
DECLARE @Location FLOAT;

    SELECT @Location = F.LocationId
    FROM
    (
        SELECT F.LocationId,
               ROW_NUMBER() OVER (ORDER BY F.Area) nr
        FROM
        (
            SELECT Localizado = geometry::STGeomFromText(L.ShapeWkt, 0).STContains(geometry::STGeomFromText(
                                                                                                               'POINT('
                                                                                                               + CAST(@Longitud AS VARCHAR(50))
                                                                                                               + ' '
                                                                                                               + CAST(@Latitud AS VARCHAR(50))
                                                                                                               + ')',
                                                                                                               0
                                                                                                           )
                                                                                  ),
                   Area = geometry::STGeomFromText(L.ShapeWkt, 0).STArea(),
                   L.LocationId
            FROM PORTAL.Locations AS L
            WHERE L.ClienteIds = @ClienteIds AND L.IsParqueo = 1
        ) AS F
        WHERE F.Localizado = 1
    ) AS F
    WHERE nr = 1;
	--SELECT @Location
    RETURN @Location;
END;
