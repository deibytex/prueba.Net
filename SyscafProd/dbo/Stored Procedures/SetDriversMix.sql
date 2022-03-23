



-- =============================================
-- Author:     ygonzalez
-- Create Date: 28/05/2021
-- Description: guarda los DRIVERS si no existen y si existen actualiza la iformacion

-- SE EDITA SP
-- =============================================
CREATE PROCEDURE dbo.SetDriversMix
(
    @ClienteIds INT,
    @drivers UDT_Driver READONLY
)
AS
BEGIN
    -- ACTUALIZAMOS LOS VEHICULOS QUE  EXISTEN
    UPDATE a
    SET a.siteIdS = v.siteIdS,
        a.fmDriverId = v.fmDriverId,
        a.extendedDriverIdType = v.extendedDriverIdType,
        a.name = v.name,
        a.employeeNumber = v.employeeNumber,
        a.aditionalFields = v.aditionalFields
    FROM @drivers AS v
        INNER JOIN
        (SELECT * FROM PORTAL.TB_Drivers WHERE ClienteIds = @ClienteIds) AS a
            ON v.driverId = a.DriverId;

    -- INSERTAMOS LOS DATOS NUEVOS
    INSERT INTO PORTAL.TB_Drivers
    (
        fmDriverId,
        DriverId,
        siteIdS,
        extendedDriverIdType,
        name,
        employeeNumber,
        ClienteIds,
        FechaSistema,
        aditionalFields
    )
    SELECT fmDriverId,
           driverId,
           siteIdS,
           extendedDriverIdType,
           name,
           employeeNumber,
           @ClienteIds,
           DATEADD(HOUR, -5, GETDATE()),
           aditionalFields
    FROM @drivers
    WHERE DriverId NOT IN
          (
              SELECT DriverId FROM PORTAL.TB_Drivers WHERE ClienteIds = @ClienteIds
          );


    DELETE FROM PORTAL.TB_Drivers
    WHERE ClienteIds = @ClienteIds
          AND DriverId NOT IN
              (
                  SELECT driverId FROM @drivers
              );



END;
