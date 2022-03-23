


-- ygonzalez 23/08/2021 inserta las locaciones de los clientes
CREATE PROCEDURE PORTAL.SetUpdateLocations @Locations AS PORTAL.Locations READONLY
-- WITH ENCRYPTION, RECOMPILE, EXECUTE AS CALLER|SELF|OWNER| 'user_name'
AS
BEGIN

    -- actualizamos las localizaciones actuales

    UPDATE L2
    SET L2.Name = L.Name,
        L2.Address = L.Address,
        L2.ShapeWkt = L.ShapeWkt,
        L2.Radius = L.Radius,
        L2.ColourOnMap = L.ColourOnMap,
        L2.OpacityOnMap = L.OpacityOnMap,
        L2.LocationType = L.LocationType,
        L2.ShapeType = L.ShapeType,
		l2.ShapeWktValid =  geometry::STGeomFromText(L.ShapeWkt, 0).MakeValid().ToString()
    FROM @Locations AS L
        INNER JOIN PORTAL.Locations AS L2
            ON L2.LocationId = L.LocationId;



    INSERT INTO PORTAL.Locations
    (
        LocationId,
        OrganisationId,
        ClienteIds,
        Name,
        Address,
        ShapeWkt,
        Radius,
        ColourOnMap,
        OpacityOnMap,
        LocationType,
        ShapeType,
        FechaSistema,
        IsDeleted,
        ShapeWktValid
    )
    SELECT LocationId,
           OrganisationId,
           ClienteIds,
           Name,
           Address,
           ShapeWkt,
           Radius,
           ColourOnMap,
           OpacityOnMap,
           LocationType,
           ShapeType,
           FechaSistema,
           IsDeleted,
           geometry::STGeomFromText(ShapeWkt, 0).MakeValid().ToString()
    FROM @Locations AS L
    WHERE L.LocationId NOT IN
          (
              SELECT L2.LocationId FROM PORTAL.Locations AS L2
          );

END;
