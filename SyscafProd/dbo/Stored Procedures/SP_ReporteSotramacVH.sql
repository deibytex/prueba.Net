--/****** Object:  StoredProcedure [dbo].[SP_ReporteSotramacVH]    Script Date: 13/08/2021 10:40:36 a. m. ******/
--SET ANSI_NULLS ON;
--GO
--SET QUOTED_IDENTIFIER ON;
--GO

CREATE PROCEDURE dbo.SP_ReporteSotramacVH
(
    @FechaInicial DATETIME,
    @FechaFinal DATETIME,
    @clienteIdS INT,
    @Assetsids VARCHAR(MAX)
)
AS
BEGIN

    --DECLARE @FechaInicial DATETIME
    --                                               , @FechaFinal   DATETIME
    --                                               , @clienteIdS   INT = 898
    --											   , @Assetsids    VARCHAR (MAX) = '41422,41423,41424,41425,41426,41427,41428,41429,41430,41431,41432,41433,41434,41435,41436,41437,41438,41439,41441,41442,41443,41444,41445,41446,41447,41448,41449,41450,41451,41452,41453,41454,41455,41456,41457,41458,41459,41460,41461,41462,41463,41464,41465,41466,41467,41468,41469,41470,41417,41418,'
    --        set @FechaInicial = '2021/07/01 00:00:0';
    --        set @FechaFinal = '2021/07/31 23:59:59';


    -- se incluye esto pera evitar error de div por cero
    SET ARITHABORT OFF;
    SET ANSI_WARNINGS OFF;

    DECLARE @FechaFin DATETIME;

    SET @FechaFin = DATEADD(ms, 997, @FechaFinal);

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
        assetId VARCHAR(MAX),
        Freno DECIMAL(11, 4)
    );

    DECLARE @Events AS TABLE
    (
        assetId VARCHAR(MAX) COLLATE Modern_Spanish_CI_AS,
        Relenti DECIMAL(11, 4),
        Inercia DECIMAL(11, 4)
    );

	DECLARE @EventsData AS TABLE
    (
        assetId BIGINT,
        EventTypeId BIGINT,
        TotalSeconds DECIMAL(11, 4),
        TotalOcurances DECIMAL(11, 4)
    );

    DECLARE @ReporteBase AS TABLE
    (
        Posicion INT,
        Vehiculo VARCHAR(MAX),
        DRAcumulada DECIMAL(11, 4),
        CCAcumulado DECIMAL(11, 4),
        DRUDia DECIMAL(11, 4),
        HorasMotor DECIMAL(11, 4),
        UsodeFreno DECIMAL(11, 4),
        PorRalenti DECIMAL(11, 4),
        PorInercia DECIMAL(11, 4),
        Co2Equivalente DECIMAL(11, 4),
        GalEquivalente DECIMAL(11, 4),
        ConsumokWh DECIMAL(11, 4),
        ComgkWh DECIMAL(11, 4),
        NomgkWh DECIMAL(11, 4),
        PMMasa DECIMAL(11, 4),
        Fact_M3 DECIMAL(11, 4),
        Rendimiento DECIMAL(11, 4)
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
    WHERE TA.assetIdS IN
          (
              SELECT value FROM STRING_SPLIT(@Assetsids, ',')
          )
          AND TA.clienteIdS = @clienteIdS;
    --where TA.clienteIdS=@clienteIdS;


	INSERT INTO @EventsData
	(
	   
	    EventTypeId,
		assetId,
	    TotalSeconds,
		TotalOcurances
	)
	 SELECT TE.EventTypeId,
               CAST(TE.assetId AS BIGINT),
               TE.TotalTimeSeconds ,
			   TE.TotalOccurances
        FROM dbo.TB_Event AS TE WITH (NOLOCK)
        WHERE (TE.EventDateTime
              BETWEEN @FechaInicial AND @FechaFin
              )

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
          --and  dateadd(MS, -3,@FechaFinal) > TSV.tripStart )
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
          AND TET.eventTypeId IN ( '-460526757267522254', '-930548846283217409' )
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

    INSERT INTO @EventFreno
    (
        assetId,
        Freno
    )
    SELECT AssetId,
           [-460526757267522254]
    FROM
    (
        SELECT TE.EventTypeId,
               TE.AssetId,
               SUM(TE.TotalOcurances) TotalOccurances
        FROM @EventsData  AS TE 
        WHERE  TE.AssetId IN
                  (
                      SELECT  CAST(A.assetId  AS BIGINT)FROM @Assets AS A
                  )
              AND TE.EventTypeId = -460526757267522254
        GROUP BY TE.EventTypeId,
                 TE.AssetId
    ) AS SourceTable
    PIVOT
    (
        SUM(TotalOccurances)
        FOR EventTypeId IN ([-460526757267522254])
    ) AS PivotTable2;



    INSERT INTO @Events
    (
        assetId,
        Inercia
    )
    SELECT AssetId,
           [-930548846283217409]
    FROM
    (
        SELECT TE.EventTypeId,
               TE.AssetId,
               SUM(TE.TotalSeconds) TotalTimeSeconds
        FROM @EventsData AS TE 
        WHERE  TE.AssetId IN
                  (
                      SELECT A.assetId COLLATE Modern_Spanish_CI_AS FROM @Assets AS A
                  )
              AND TE.EventTypeId  = -930548846283217409
        GROUP BY TE.EventTypeId,
                 TE.AssetId
    ) AS SourceTable
    PIVOT
    (
        SUM(TotalTimeSeconds)
        FOR EventTypeId IN ([-930548846283217409])
    ) AS PivotTable;

    INSERT INTO @ReporteBase
    (
        Posicion,
        Vehiculo,
        DRAcumulada,
        CCAcumulado,
        DRUDia,
        HorasMotor,
        UsodeFreno,
        PorRalenti,
        PorInercia,
        Co2Equivalente,
        GalEquivalente,
        ConsumokWh,
        ComgkWh,
        NomgkWh,
        PMMasa,
        Fact_M3,
        Rendimiento
    )
    SELECT ROW_NUMBER() OVER (ORDER BY TSV.assetId),
           TA.Placa,
           ISNULL(SUM(TripDistance), 0),
           ISNULL(SUM(Litres), 0),
           ISNULL(
           (
               SELECT SUM(TripDistance)
               FROM @Viajes AS s
               WHERE s.assetId = TSV.assetId
                     AND TripStart > DATEADD(DAY, -1, @FechaFinal)
           ),
           0
                 ),
           ISNULL(SUM(TSV.EngineSecond), 0),
           ef.Freno,
           ISNULL(SUM(TSV.IdleTime), 0),
           ev.Inercia,
           (
               SELECT Valor FROM @ValoresFactores WHERE Sigla = 'Fact_Co2_Equivalente'
           ),
           (
               SELECT Valor FROM @ValoresFactores WHERE Sigla = 'Fact_Gal_Equivalente'
           ),
           (
               SELECT Valor FROM @ValoresFactores WHERE Sigla = 'Fact_consumo_kWH'
           ),
           (
               SELECT Valor FROM @ValoresFactores WHERE Sigla = 'CO_kWH'
           ),
           (
               SELECT Valor FROM @ValoresFactores WHERE Sigla = 'NOx_kWH'
           ),
           (
               SELECT Valor FROM @ValoresFactores WHERE Sigla = 'PM_Masa'
           ),
           (
               SELECT Valor FROM @ValoresFactores WHERE Sigla = 'Fact_M3'
           ),
           ISNULL(SUM(TripDistance), 0) / ISNULL(SUM(Litres), 0)
    FROM @Viajes AS TSV
        LEFT JOIN @EventFreno AS ef
            ON ef.assetId = TSV.assetId
        LEFT JOIN @Events AS ev
            ON ev.assetId = TSV.assetId COLLATE Modern_Spanish_CI_AS
        INNER JOIN @Assets AS TA
            ON (TA.assetId = TSV.assetId)
    GROUP BY TA.Placa,
             TSV.assetId,
             Freno,
             Inercia;

    SELECT Posicion,
           Vehiculo,
           ROUND(DRAcumulada, 1) AS DistanciaRecorridaAcumulada,
           ROUND(CCAcumulado, 2) AS ConsumodeCombustibleAcumulado,
           ROUND(DRUDia, 1) AS DistanciaRecorridaUltimoDia,
           ROUND(((DRAcumulada / CCAcumulado) / Fact_M3), 2) AS RendimientoCumbustibleAcumulado,
           CAST(ROUND(((UsodeFreno * 100.0) / DRAcumulada), 0) AS INT) AS UsoDelFreno,
           CAST(ROUND(((PorInercia / (HorasMotor - PorRalenti)) * 100.0), 0) AS INT) AS PorDeInercia,
           CAST(ROUND(((PorRalenti / HorasMotor) * 100.0), 0) AS INT) AS PorDeRalenti,
           ROUND(((CCAcumulado + Rendimiento) * Co2Equivalente), 1) AS Co2Equivalente,
           ROUND(((CCAcumulado + Rendimiento) / GalEquivalente), 1) AS GalEquivalente,
           ROUND((ConsumokWh * ((CCAcumulado + Rendimiento) / GalEquivalente)), 1) AS ConsumokWh,
           ROUND((ComgkWh * ((ConsumokWh * ((CCAcumulado + Rendimiento) / GalEquivalente))) / 1000000), 2) AS COmgkWh,
           ROUND((NomgkWh * ((ConsumokWh * ((CCAcumulado + Rendimiento) / GalEquivalente))) / 1000000), 2) AS NOxmgkWh,
           ROUND((PMMasa * ((ConsumokWh * ((CCAcumulado + Rendimiento) / GalEquivalente))) / 1000000), 2) AS PMMasamgkWh
    FROM @ReporteBase
    ORDER BY RendimientoCumbustibleAcumulado DESC;
END;