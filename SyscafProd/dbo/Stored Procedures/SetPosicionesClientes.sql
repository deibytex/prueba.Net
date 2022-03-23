
--ygonzalez 2021.05.13
-- valida que la informacion no exista y la inserta
CREATE PROCEDURE dbo.SetPosicionesClientes
(@Posiciones AS dbo.UDT_Positions READONLY)
AS
BEGIN


    DECLARE @fechasistema AS DATE =
            (
                SELECT TOP 1 fechaSistema FROM @Posiciones
            );

    INSERT INTO TB_Positions
    (
        AgeOfReadingSeconds,
        AltitudeMetres,
        AssetId,
        DistanceSinceReadingKilometres,
        DriverId,
        FormattedAddress,
        Hdop,
        Heading,
        IsAvl,
        Latitude,
        Longitude,
        NumberOfSatellites,
        OdometerKilometres,
        Pdop,
        PositionId,
        SpeedKilometresPerHour,
        SpeedLimit,
        Timestamp,
        Vdop,
        assetPositionId,
        assetIdS,
        driverIdS,
        estadoBase,
        fechaSistema,
        ClienteIds
    )
    SELECT pt.AgeOfReadingSeconds,
           pt.AltitudeMetres,
           pt.AssetId,
           pt.DistanceSinceReadingKilometres,
           pt.DriverId,
           pt.FormattedAddress,
           pt.Hdop,
           pt.Heading,
           pt.IsAvl,
           pt.Latitude,
           pt.Longitude,
           pt.NumberOfSatellites,
           pt.OdometerKilometres,
           pt.Pdop,
           pt.PositionId,
           pt.SpeedKilometresPerHour,
           pt.SpeedLimit,
           pt.Timestamp,
           pt.Vdop,
           '',
           a.assetIdS,
           0,
           pt.estadoBase,
           pt.fechaSistema,
           a.clienteIdS
    FROM @Posiciones AS pt
        INNER JOIN dbo.TB_Assets AS a
            ON pt.AssetId = a.assetId COLLATE SQL_Latin1_General_CP1_CI_AS       
    WHERE pt.PositionId NOT IN
          (
              SELECT PositionId FROM dbo.TB_Positions AS TP WHERE fechaSistema > @fechasistema
          );



END;


