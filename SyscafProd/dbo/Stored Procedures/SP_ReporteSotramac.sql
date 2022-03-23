CREATE PROCEDURE dbo.SP_ReporteSotramac
(
    @FechaInicial DATETIME,
    @FechaFinal DATETIME,
    @clienteIdS INT,
    @DriversIdS VARCHAR(MAX)
)
AS
BEGIN

    --DECLARE @FechaInicial DATETIME
    --											   , @FechaFinal   DATETIME
    --											   , @clienteIdS   INT = 898
    --											   , @DriversIdS VARCHAR (MAX) = '1073604557475930135'
    --		set @FechaInicial = '2021-07-01 00:00:00.000';
    --		set @FechaFinal = '2021-07-31 23:59:59.000';



    -- Se incluye esto pera evitar error de div por cero
    SET ARITHABORT OFF;
    SET ANSI_WARNINGS OFF;

    DECLARE @FechaFin DATETIME;

    SET @FechaFin = DATEADD(ms, 997, @FechaFinal);

    INSERT INTO TB_ActualizacionLogs
    (
        error,
        fechaError
    )
    VALUES
    (CONVERT(VARCHAR(MAX), @FechaInicial, 21) + ' Y ' + CONVERT(VARCHAR(MAX), @FechaFin, 21), GETDATE());

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
        driverIdd VARCHAR(MAX),
        DriverID BIGINT,
        EmployeeNumber VARCHAR(MAX) NULL,
        DriverName VARCHAR(MAX),
        DriverSiteID INT,
        DriverSiteName VARCHAR(MAX)
    );

    DECLARE @Viajes AS TABLE
    (
        Id INT IDENTITY(1, 1) NOT NULL,
        SubViajeId INT NULL,
        assetId VARCHAR(MAX) NOT NULL,
        driverId VARCHAR(MAX) NOT NULL,
        TripStart DATETIME NOT NULL,
        TripEnd DATETIME NOT NULL,
        TripDistance DECIMAL(11, 4) NOT NULL,
        Litres DECIMAL(11, 4) NULL,
        StartEngineSeconds INT NULL,
        EndEngineSeconds INT NULL,
        EngineSecond DECIMAL(11, 4) NULL,
        DriverIds INT NULL,
        IdleOccurs DECIMAL(11, 4) NULL,
        IdleTime DECIMAL(11, 4) NULL
    );


    DECLARE @EventsType AS TABLE
    (
        EventTypeIdS INT
    );

    DECLARE @ValoresFactores AS TABLE
    (
        Sigla VARCHAR(MAX),
        Valor FLOAT
    );

    DECLARE @EventFreno AS TABLE
    (
        DriverId BIGINT,
        Freno DECIMAL(11, 4)
    );

    DECLARE @Events AS TABLE
    (
        DriverId BIGINT,
        Inercia DECIMAL(11, 4)
    );
	DECLARE @EventsData AS TABLE
    (
        assetId BIGINT,		
		driverId BIGINT,
        EventTypeId BIGINT,
        TotalSeconds DECIMAL(11, 4),
        TotalOcurances DECIMAL(11, 4)
    );
    DECLARE @ReporteBase AS TABLE
    (
        Posicion INT,
        Cedula VARCHAR(MAX),
        Nombre VARCHAR(MAX),
        DRAcumulada DECIMAL(11, 4),
        CCAcumulado DECIMAL(11, 4),
        DRUDia DECIMAL(11, 4),
        HorasMotor DECIMAL(11, 4),
        UsodeFreno DECIMAL(11, 4),
        PorRalenti DECIMAL(11, 4),
        PorInercia DECIMAL(11, 4),
        FactM3 DECIMAL(11, 4)
    );

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
        DriverID,
        driverIdd,
        EmployeeNumber,
        DriverName,
        DriverSiteID,
        DriverSiteName
    )
    SELECT TD.DriverId,
           TD.fmDriverId,
           TD.employeeNumber,
           TD.name,
           TD.siteIdS,
           TS.siteName
    FROM PORTAL.TB_Drivers AS TD WITH (NOLOCK)
        INNER JOIN dbo.TB_Site AS TS WITH (NOLOCK)
            ON (TS.siteIdS = TD.siteIdS)
        INNER JOIN dbo.TB_Cliente AS TC WITH (NOLOCK)
            ON (TC.clienteIdS = TS.clienteIdS)
    WHERE TD.DriverId IN
          (
              SELECT CAST(value AS BIGINT)FROM STRING_SPLIT(@DriversIdS, ',')
          )
          AND TS.clienteIdS = @clienteIdS
          AND TD.employeeNumber IS NOT NULL;

    --WHERE TS.clienteIdS=@clienteIdS 
    --and TD.employeeNumber IS NOT NULL ;           

    INSERT INTO @Viajes
    (
        SubViajeId,
        assetId,
        driverId,
        TripStart,
        TripEnd,
        TripDistance,
        Litres,
        StartEngineSeconds,
        EndEngineSeconds,
        EngineSecond,
        DriverIds,
        IdleOccurs,
        IdleTime
    )
    SELECT subViajeId,
           TSV.assetId,
           TSV.driverId,
           TSV.tripStart,
           tripEnd,
           TSV.distanceKilometers,
           fuelUsedLitres,
           startEngineSeconds,
           endEngineSeconds,
           engineSeconds,
           TSV.driverIdS,
           M.IdleOccurs,
           M.IdleTime
    FROM dbo.TB_SubViaje AS TSV WITH (NOLOCK)
        LEFT JOIN TB_TripsMetrics AS M
            ON (M.TripId = TSV.tripId)
    WHERE (
              TSV.tripStart >= @FechaInicial
              AND @FechaFin > TSV.tripStart
          )
          AND TSV.driverId IN
              (
                  SELECT A.DriverID FROM @Drivers AS A
              )
          AND TSV.assetIdS IN
              (
                  SELECT A.assetIdS FROM @Assets AS A
              )
          AND TSV.subViajeId IS NULL;

    INSERT INTO @EventsType
    (
        EventTypeIdS
    )
    SELECT TET.eventTypeIdS
    FROM dbo.TB_EventType AS TET WITH (NOLOCK)
        INNER JOIN dbo.TB_Site AS TS WITH (NOLOCK)
            ON TS.clienteIdS = TET.clienteIdS
    WHERE TET.clienteIdS = @clienteIdS
          AND TET.eventTypeId IN ( '-460526757267522254', '-930548846283217409', '-3393530750645328945' )
    GROUP BY TET.eventTypeIdS;

    INSERT INTO @ValoresFactores
    (
        Sigla,
        Valor
    )
    SELECT DT.Sigla,
           CAST(Valor AS FLOAT)
    FROM TB_DetalleListas AS DT
        INNER JOIN TB_Listas AS L
            ON L.ListaId = DT.ListaId
    WHERE L.Sigla = 'PREPEXC';

	
	INSERT INTO @EventsData
	(
	   
	    EventTypeId,
		assetId,
		driverId,
	    TotalSeconds,
		TotalOcurances
	)
	 SELECT TE.EventTypeId,
               CAST(TE.assetId AS BIGINT),
			    CAST(TE.DriverId AS BIGINT),
               TE.TotalTimeSeconds ,
			   TE.TotalOccurances
        FROM dbo.TB_Event AS TE WITH (NOLOCK)
        WHERE (TE.EventDateTime
              BETWEEN @FechaInicial AND @FechaFin
              )
    INSERT INTO @EventFreno
    (
        DriverId,
        Freno
    )
    SELECT DriverId,
           [-460526757267522254]
    FROM
    (
        SELECT TE.EventTypeId,
               TE.DriverId,
               SUM(TE.TotalOcurances) TotalOccurances
        FROM @EventsData AS TE 
        WHERE  TE.DriverId IN
                  (
                      SELECT D.DriverID FROM @Drivers AS D
                  )
              AND TE.eventTypeId  = -460526757267522254
        GROUP BY TE.EventTypeId,
                 TE.DriverId
    ) AS SourceTable
    PIVOT
    (
        SUM(TotalOccurances)
        FOR EventTypeId IN ([-460526757267522254])
    ) AS PivotTable2;

    INSERT INTO @Events
    (
        DriverId,
        Inercia
    )
    SELECT DriverId,
           [-930548846283217409]
    FROM
    (
        SELECT TE.EventTypeId,
               TE.DriverId,
               SUM(TE.TotalSeconds) TotalTimeSeconds
        FROM  @EventsData AS TE 
        WHERE TE.DriverId IN
                  (
                      SELECT D.DriverID FROM @Drivers AS D
                  )
              AND TE.eventTypeId =-930548846283217409
        GROUP BY TE.EventTypeId,
                 TE.DriverId
    ) AS SourceTable
    PIVOT
    (
        SUM(TotalTimeSeconds)
        FOR EventTypeId IN ([-930548846283217409])
    ) AS PivotTable;




    INSERT INTO @ReporteBase
    (
        Posicion,
        Cedula,
        Nombre,
        DRAcumulada,
        CCAcumulado,
        DRUDia,
        HorasMotor,
        UsodeFreno,
        PorRalenti,
        PorInercia,
        FactM3
    )
    SELECT ROW_NUMBER() OVER (ORDER BY TSV.driverId),
           TD.EmployeeNumber,
           TD.DriverName,
           ISNULL(SUM(TripDistance), 0),
           ISNULL(SUM(Litres), 0),
           ISNULL(
           (
               SELECT SUM(TripDistance)
               FROM @Viajes AS s
               WHERE s.driverId = TSV.driverId
                     AND TripStart > DATEADD(DAY, -1, @FechaFinal)
           ),
           0
                 ),
           ISNULL(SUM(EngineSecond), 0),
           Freno,
           ISNULL(SUM(IdleTime), 0),
           Inercia,
           (
               SELECT Valor FROM @ValoresFactores WHERE Sigla = 'Fact_M3'
           )
    FROM @Viajes AS TSV
        LEFT JOIN @EventFreno AS ef
            ON ef.DriverId = TSV.driverId
        LEFT JOIN @Events AS ev
            ON ev.DriverId = TSV.driverId
        INNER JOIN @Assets AS TA
            ON (TA.assetId = TSV.assetId)
        INNER JOIN @Drivers AS TD
            ON (TD.DriverID = TSV.driverId)
    GROUP BY TD.EmployeeNumber,
             TD.DriverName,
             TSV.driverId,
             Freno,
             Inercia;


    SELECT Posicion,
           Cedula,
           Nombre,
           ROUND((DRAcumulada), 1) AS DistanciaRecorridaAcumulada,
           ROUND((CCAcumulado), 2) AS ConsumodeCombustibleAcumulado,
           ROUND((DRUDia), 1) AS DistanciaRecorridaUltimoDia,
           ROUND(((DRAcumulada / CCAcumulado) / FactM3), 2) AS RendimientoCumbustibleAcumulado,
           CAST(ROUND(((UsodeFreno * 100.0) / DRAcumulada), 0) AS INT) AS UsoDelFreno,
           CAST(ROUND(((PorInercia / (HorasMotor - PorRalenti)) * 100.0), 0) AS INT) AS PorDeInercia,
           CAST(ROUND(((PorRalenti / HorasMotor) * 100.0), 0) AS INT) AS PorDeRalenti
    FROM @ReporteBase
    --where Cedula = '73200877'
    ORDER BY RendimientoCumbustibleAcumulado DESC;
END;