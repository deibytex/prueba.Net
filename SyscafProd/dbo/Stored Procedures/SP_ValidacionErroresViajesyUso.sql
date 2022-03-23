

--EXEC dbo.SP_ValidacionErroresViajesyUso '20211101','20211102','112021',886
-- ygonzalez 
-- 03.11.2021
-- se valida que la informacion que proviene de errores viajes y usos sea la correcta
-- se vuelve a ejecutar por procesos y se actualiza la información
CREATE PROCEDURE dbo.SP_ValidacionErroresViajesyUso
(
    @FechaInicial DATETIME,
    @FechaFinal DATETIME,
    @Periodo VARCHAR(10),
    @clienteIdS INT
)
AS
BEGIN

    DECLARE @Clientes AS TABLE
    (
        clienteName VARCHAR(MAX),
        clienteIdS INT,
        US VARCHAR(MAX)
    );

    DECLARE @Assets AS TABLE
    (
        assetIdS INT,
        assetId VARCHAR(MAX),
        clienteIdS INT,
        VehicleID VARCHAR(MAX),
        Placa VARCHAR(MAX),
        VehicleSiteID INT,
        VehicleSiteName VARCHAR(MAX)
    );
    DECLARE @Drivers AS TABLE
    (
        DriverIdS INT,
        driverIdd INT,
        DriverID VARCHAR(MAX),
        DriverName VARCHAR(MAX),
        DriverSiteID INT,
        DriverSiteName VARCHAR(MAX)
    );



    DECLARE @TB_ErroresViajesyUso AS TABLE
    (
        Consecutivo INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
        Cliente VARCHAR(300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
        US VARCHAR(3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
        VehicleID INT NULL,
        Placa VARCHAR(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
        VehicleSiteID BIGINT NULL,
        VehicleSiteName VARCHAR(200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
        TripNo BIGINT NULL,
        DriverID BIGINT NULL,
        DriverName VARCHAR(300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
        DriverSiteID BIGINT NULL,
        DriverSiteName VARCHAR(200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
        OriginalDriverID INT NULL,
        OriginalDriverName VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
        TripStart DATETIME NULL,
        TripEnd DATETIME NULL,
        CategoryID INT NULL,
        Notes VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
        StartSubTripSeq INT NULL,
        EndSubTripSeq INT NULL,
        TripDistance DECIMAL(18, 1) NULL,
        Odometer DECIMAL(18, 1) NULL,
        MaxSpeed INT NULL,
        SpeedTime INT NULL,
        SpeedOccurs INT NULL,
        MaxBrake INT NULL,
        BrakeTime INT NULL,
        BrakeOccurs INT NULL,
        MaxAccel INT NULL,
        AccelTime INT NULL,
        AccelOccurs INT NULL,
        MaxRPM INT NULL,
        RPMTime INT NULL,
        RPMOccurs INT NULL,
        GBTime INT NULL,
        ExIdleTime INT NULL,
        ExIdleOccurs INT NULL,
        NIdleTime INT NULL,
        NIdleOccurs INT NULL,
        StandingTime INT NULL,
        Litres DECIMAL(18, 1) NULL,
        StartGPSID BIGINT NULL,
        EndGPSID BIGINT NULL,
        StartEngineSeconds INT NULL,
        EndEngineSeconds INT NULL,
        ClienteIdS INT NOT NULL
    );


    IF OBJECT_ID('tempdb..#Portal_ValidacionViajes') IS NOT NULL
        DROP TABLE #Portal_ValidacionViajes;
    CREATE TABLE #Portal_ValidacionViajes
    (
        tripId BIGINT NOT NULL,
        assetId BIGINT NOT NULL,
        driverId BIGINT NOT NULL,
        TripStart DATETIME NOT NULL,
        TripEnd DATETIME NOT NULL,
        TripDistance DECIMAL(18, 3) NOT NULL,
        Odometer DECIMAL(18, 3) NOT NULL,
        MaxSpeed INT NOT NULL,
        MaxBrake INT NOT NULL,
        MaxAccel INT NOT NULL,
        MaxRPM INT NOT NULL,
        StandingTime DECIMAL(18, 4) NOT NULL,
        Litres DECIMAL(18, 2) NULL,
        StartGPSID VARCHAR(50) NULL,
        EndGPSID VARCHAR(50) NULL,
        StartEngineSeconds INT NULL,
        EndEngineSeconds INT NULL,
        NIdleTime INT,
        NIdleOccurs INT,
        CantSubtrips INT
    );



    IF OBJECT_ID('tempdb..#Events') IS NOT NULL
        DROP TABLE #Events;
    CREATE TABLE #Events
    (
        AssetId BIGINT,
        FechaInicioEvento DATETIME,
        Occurances INT,
        TimeSeconds INT,
        EventTypeId BIGINT,
        DriverId BIGINT
    );
    CREATE INDEX idxtempevent
    ON #Events (EventTypeId)
    INCLUDE (
                DriverId,
                AssetId
            );
    CREATE INDEX idxtemeventfecha
    ON #Events (FechaInicioEvento)
    INCLUDE (
                DriverId,
                AssetId
            );
    INSERT INTO @Clientes
    (
        clienteIdS,
        clienteName,
        US
    )
    SELECT clienteIdS,
           clienteNombre,
           'US'
    FROM dbo.TB_Cliente AS TC WITH (NOLOCK)
    WHERE clienteIdS = @clienteIdS;

    INSERT INTO @Assets
    (
        assetIdS,
        assetId,
        clienteIdS,
        VehicleID,
        Placa,
        VehicleSiteID,
        VehicleSiteName
    )
    SELECT TA.assetIdS,
           TA.assetId,
           TA.clienteIdS,
           TA.assetCodigo,
           TA.assetsDescription,
           TA.siteIdS,
           TS.siteName
    FROM dbo.TB_Assets AS TA WITH (NOLOCK)
        INNER JOIN dbo.TB_Site AS TS WITH (NOLOCK)
            ON (TS.siteIdS = TA.siteIdS)
    WHERE TA.clienteIdS = @clienteIdS;


    INSERT INTO @Drivers
    (
        driverIdd,
        DriverID,
        DriverName,
        DriverSiteID,
        DriverSiteName
    )
    SELECT TD.fmDriverId,
           TD.DriverId,
           TD.name,
           TD.siteIdS,
           TS.siteName
    FROM PORTAL.TB_Drivers AS TD WITH (NOLOCK)
        INNER JOIN dbo.TB_Site AS TS WITH (NOLOCK)
            ON (TS.siteIdS = TD.siteIdS)
        INNER JOIN dbo.TB_Cliente AS TC WITH (NOLOCK)
            ON (TC.clienteIdS = TS.clienteIdS)
    WHERE TS.clienteIdS = @clienteIdS;

    DECLARE @SQLScript NVARCHAR(MAX)
        = N' 

        INSERT INTO #Portal_ValidacionViajes
        ( tripId, assetId, driverId, TripStart, TripEnd
          , TripDistance, Odometer, MaxSpeed, MaxBrake, MaxAccel, MaxRPM
          , StandingTime, Litres, StartGPSID, EndGPSID, StartEngineSeconds
          , EndEngineSeconds, NIdleTime, NIdleOccurs, CantSubtrips
        )
         SELECT 
		     T.TripId, T.assetId, T.driverId,  T.tripStart , tripEnd  , distanceKilometers, endOdometerKilometers
           , maxSpeedKilometersPerHour     , maxDecelerationKilometersPerHourPerSecond
           , maxAccelerationKilometersPerHourPerSecond, maxRpm, standingTime
           , fuelUsedLitres, startPositionId, endPositionId
           , startEngineSeconds, endEngineSeconds, M.[NIdleTime], M.[NIdleOccurs], [CantSubtrips]
            FROM [PORTAL].[TB_Trips_' + @Periodo + N'_' + CAST(@clienteIdS AS VARCHAR)
          + N'] T 
			Inner Join [PORTAL].[TB_TripsMetrics_' + @Periodo + N'_' + CAST(@clienteIdS AS VARCHAR)
          + N'] M
					on T.TripId = M.TripId
		   Where  T.[tripStart] between @Fi and @ff
		              ';


    EXEC sp_executesql @SQLScript,
                       N'@Fi as datetime,  @ff as datetime',
                       @FechaInicial,
                       @FechaFinal;;


    SET @SQLScript
        = N'
        INSERT INTO #Events(  AssetId, FechaInicioEvento, Occurances
                           , TimeSeconds, EventTypeId, DriverId
                           )
         SELECT 
             TE.AssetId, TE.[StartDateTime], TE.TotalOccurances
           , TE.TotalTimeSeconds, TE.EventTypeId, te.driverId
            FROM [PORTAL].[TB_Event_' + @Periodo + N'_' + CAST(@clienteIdS AS VARCHAR)
          + N']  AS TE                  
                Where  TE.[StartDateTime] between @Fi and @ff
					   ';
    SELECT @FechaInicial = MIN(V.TripStart),
           @FechaFinal = MAX(V.TripEnd)
    FROM #Portal_ValidacionViajes AS V;

    EXEC sp_executesql @SQLScript,
                       N'@Fi as datetime,  @ff as datetime',
                       @FechaInicial,
                       @FechaFinal;

    PRINT 'insert errorese';
    INSERT INTO @TB_ErroresViajesyUso
    (
        Cliente,
        US,
        VehicleID,
        Placa,
        VehicleSiteID,
        VehicleSiteName,
        TripNo,
        DriverID,
        DriverName,
        DriverSiteID,
        DriverSiteName,
        OriginalDriverID,
        OriginalDriverName,
        TripStart,
        TripEnd,
        CategoryID,
        Notes,
        StartSubTripSeq,
        EndSubTripSeq,
        TripDistance,
        Odometer,
        MaxSpeed,
        SpeedTime,
        SpeedOccurs,
        MaxBrake,
        BrakeTime,
        BrakeOccurs,
        MaxAccel,
        AccelTime,
        AccelOccurs,
        MaxRPM,
        RPMTime,
        RPMOccurs,
        GBTime,
        ExIdleTime,
        ExIdleOccurs,
        NIdleTime,
        NIdleOccurs,
        StandingTime,
        Litres,
        StartGPSID,
        EndGPSID,
        StartEngineSeconds,
        EndEngineSeconds,
        ClienteIdS
    )
    SELECT TC.clienteName,
           US = 'US',
           VehicleID = CAST(TA.VehicleID AS INT),
           TA.Placa,
           TA.VehicleSiteID,
           TA.VehicleSiteName,
           TSV.tripId,
           TD.driverIdd,
           TD.DriverName,
           TD.DriverSiteID,
           TD.DriverSiteName,
           0 AS OriginalDriverID,
           'Conductor Desconocido' AS OriginalDriverName,
           TSV.TripStart,
           TSV.TripEnd,
           0 AS CategoryID,
           '' AS Notes,
           CASE TSV.CantSubtrips
               WHEN 0 THEN
                   0
               ELSE
                   1
           END AS StartSubTripSeq,
           TSV.CantSubtrips AS EndSubTripSeq,
           TSV.TripDistance,
           TSV.Odometer,
           TSV.MaxSpeed,
           SpeedTime = ISNULL(
                       (
                           SELECT SUM(E.TimeSeconds)
                           FROM #Events AS E
                           WHERE E.FechaInicioEvento
                                 BETWEEN TSV.TripStart AND TSV.TripEnd
                                 AND E.EventTypeId = '-3890646499157906515'
                                 AND E.DriverId = TSV.driverId
                                 AND E.AssetId = TSV.assetId
                       ),
                       0
                             ),
           SpeedOccurs = ISNULL(
                         (
                             SELECT SUM(E.Occurances)
                             FROM #Events AS E
                             WHERE E.FechaInicioEvento
                                   BETWEEN TSV.TripStart AND TSV.TripEnd
                                   AND E.EventTypeId = '-3890646499157906515'
                                   AND E.DriverId = TSV.driverId
                                   AND E.AssetId = TSV.assetId
                         ),
                         0
                               ),
           TSV.MaxBrake,
           BrakeTime = ISNULL(
                       (
                           SELECT SUM(E.TimeSeconds)
                           FROM #Events AS E
                           WHERE E.FechaInicioEvento
                                 BETWEEN TSV.TripStart AND TSV.TripEnd
                                 AND E.EventTypeId = '4750800303282680186'
                                 AND E.DriverId = TSV.driverId
                                 AND E.AssetId = TSV.assetId
                       ),
                       0
                             ),
           BrakeOccurs = ISNULL(
                         (
                             SELECT SUM(E.Occurances)
                             FROM #Events AS E
                             WHERE E.FechaInicioEvento
                                   BETWEEN TSV.TripStart AND TSV.TripEnd
                                   AND E.EventTypeId = '4750800303282680186'
                                   AND E.DriverId = TSV.driverId
                                   AND E.AssetId = TSV.assetId
                         ),
                         0
                               ),
           TSV.MaxAccel,
           AccelTime = ISNULL(
                       (
                           SELECT SUM(E.TimeSeconds)
                           FROM #Events AS E
                           WHERE E.FechaInicioEvento
                                 BETWEEN TSV.TripStart AND TSV.TripEnd
                                 AND E.EventTypeId = '6454149451280645233'
                                 AND E.DriverId = TSV.driverId
                                 AND E.AssetId = TSV.assetId
                       ),
                       0
                             ),
           AccelOccurs = ISNULL(
                         (
                             SELECT SUM(E.Occurances)
                             FROM #Events AS E
                             WHERE E.FechaInicioEvento
                                   BETWEEN TSV.TripStart AND TSV.TripEnd
                                   AND E.EventTypeId = '6454149451280645233'
                                   AND E.DriverId = TSV.driverId
                                   AND E.AssetId = TSV.assetId
                         ),
                         0
                               ),
           TSV.MaxRPM,
           RPMTime = ISNULL(
                     (
                         SELECT SUM(E.TimeSeconds)
                         FROM #Events AS E
                         WHERE E.FechaInicioEvento
                               BETWEEN TSV.TripStart AND TSV.TripEnd
                               AND E.EventTypeId = '-7372181092478897411'
                               AND E.DriverId = TSV.driverId
                               AND E.AssetId = TSV.assetId
                     ),
                     0
                           ),
           RPMOccurs = ISNULL(
                       (
                           SELECT SUM(E.Occurances)
                           FROM #Events AS E
                           WHERE E.FechaInicioEvento
                                 BETWEEN TSV.TripStart AND TSV.TripEnd
                                 AND E.EventTypeId = '-7372181092478897411'
                                 AND E.DriverId = TSV.driverId
                                 AND E.AssetId = TSV.assetId
                       ),
                       0
                             ),
           GBTime = ISNULL(
                    (
                        SELECT SUM(E.TimeSeconds)
                        FROM #Events AS E
                        WHERE E.FechaInicioEvento
                              BETWEEN TSV.TripStart AND TSV.TripEnd
                              AND E.EventTypeId = '-7417774485302453264'
                              AND E.DriverId = TSV.driverId
                              AND E.AssetId = TSV.assetId
                    ),
                    0
                          ),
           ExIdleTime = ISNULL(
                        (
                            SELECT SUM(E.TimeSeconds)
                            FROM #Events AS E
                            WHERE E.FechaInicioEvento
                                  BETWEEN TSV.TripStart AND TSV.TripEnd
                                  AND E.EventTypeId = '4650840888823746694'
                                  AND E.DriverId = TSV.driverId
                                  AND E.AssetId = TSV.assetId
                        ),
                        0
                              ),
           ExIdleOccurs = ISNULL(
                          (
                              SELECT SUM(E.Occurances)
                              FROM #Events AS E
                              WHERE E.FechaInicioEvento
                                    BETWEEN TSV.TripStart AND TSV.TripEnd
                                    AND E.EventTypeId = '4650840888823746694'
                                    AND E.DriverId = TSV.driverId
                                    AND E.AssetId = TSV.assetId
                          ),
                          0
                                ),
           TSV.NIdleTime,
           TSV.NIdleOccurs,
           TSV.StandingTime,
           TSV.Litres,
           TSV.StartGPSID,
           TSV.EndGPSID,
           TSV.StartEngineSeconds,
           TSV.EndEngineSeconds,
           TA.clienteIdS
    FROM #Portal_ValidacionViajes AS TSV
        INNER JOIN @Assets AS TA
            ON (TA.assetId = TSV.assetId)
        INNER JOIN @Clientes AS TC
            ON (TC.clienteIdS = TA.clienteIdS)
        INNER JOIN @Drivers AS TD
            ON (TD.DriverID = TSV.driverId);


    PRINT 'actualizados';

    -- actualizamos los que tengan diferencias
    UPDATE TEVU
    SET TEVU.SpeedOccurs = V.SpeedOccurs,
        TEVU.SpeedTime = V.SpeedTime,
        TEVU.MaxBrake = V.MaxBrake,
        TEVU.BrakeTime = V.BrakeTime,
        TEVU.BrakeOccurs = V.BrakeOccurs,
        TEVU.MaxAccel = V.MaxAccel,
        TEVU.AccelOccurs = V.AccelOccurs,
        TEVU.MaxRPM = V.MaxRPM,
        TEVU.RPMTime = V.RPMTime,
        TEVU.RPMOccurs = V.RPMOccurs,
        TEVU.GBTime = V.GBTime,
        TEVU.ExIdleTime = V.ExIdleTime,
        TEVU.ExIdleOccurs = V.ExIdleOccurs,
        TEVU.NIdleTime = V.NIdleTime,
        TEVU.NIdleOccurs = V.NIdleOccurs
    FROM @TB_ErroresViajesyUso AS V
        INNER JOIN
        (
            SELECT *
            FROM dbo.TB_ErroresViajesyUso AS TEVU
            WHERE 
			 TEVU.TripStart BETWEEN @FechaInicial AND @FechaFinal AND TEVU.ClienteIdS = @clienteIdS
        ) AS TEVU
            ON (TEVU.TripNo = V.TripNo)
    WHERE TEVU.SpeedTime <> V.SpeedTime
          OR TEVU.SpeedOccurs <> V.SpeedOccurs
          OR TEVU.MaxBrake <> V.MaxBrake
          OR TEVU.BrakeTime <> V.BrakeTime
          OR TEVU.BrakeOccurs <> V.BrakeOccurs
          OR TEVU.MaxAccel <> V.MaxAccel
          OR TEVU.AccelOccurs <> V.AccelOccurs
          OR TEVU.MaxRPM <> V.MaxRPM
          OR TEVU.RPMTime <> V.RPMTime
          OR TEVU.RPMOccurs <> V.RPMOccurs
          OR TEVU.GBTime <> V.GBTime
          OR TEVU.ExIdleTime <> V.ExIdleTime
          OR TEVU.ExIdleOccurs <> V.ExIdleOccurs
          OR TEVU.NIdleTime <> V.NIdleTime
          OR TEVU.NIdleOccurs <> V.NIdleOccurs;

SELECT @clienteIdS, @@ROWCOUNT , @FechaInicial


END;


