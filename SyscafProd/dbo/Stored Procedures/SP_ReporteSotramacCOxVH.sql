CREATE PROCEDURE [dbo].[SP_ReporteSotramacCOxVH]( @FechaInicial DATETIME
                                               , @FechaFinal   DATETIME
                                               , @clienteIdS   INT
											   , @DriversIdS   VARCHAR (MAX)
											   , @Assetsids    VARCHAR (MAX)
                                               )
AS
    BEGIN
--DECLARE @FechaInicial DATETIME
--                                               , @FechaFinal   DATETIME
--                                               , @clienteIdS   INT = 898
--											   , @DriversIdS VARCHAR (MAX) = '1073604557475930135'
--											   , @Assetsids VARCHAR (MAX)= '41425,41426,41427,41428,41429,41430,41431,41432,41433,41434'
--   	    set @FechaInicial = '20210801';
--        set @FechaFinal = '20210809';


       
	   -- se incluye esto pera evitar error de div por cero
		SET ARITHABORT OFF 
		SET ANSI_WARNINGS OFF

declare @FechaFin DATETIME;			
set @FechaFin = dateadd(ms,997,@FechaFinal);

        DECLARE @Assets AS TABLE
        (
        assetIdS        INT
      , assetId         VARCHAR(MAX)
      , clienteIdS      INT
      , VehicleID       VARCHAR(MAX)
      , Placa           VARCHAR(MAX)
      , VehicleSiteID   INT
      , VehicleSiteName VARCHAR(MAX)
        ) ;

        DECLARE @Drivers AS TABLE
        (        
       DriverID       bigint
	  , EmployeeNumber VARCHAR (MAX) NULL
      , DriverName     VARCHAR(MAX)
      , DriverSiteID   INT
      , DriverSiteName VARCHAR(MAX)
        ) ;

        DECLARE @Viajes AS TABLE
        (
        Id                 INT            IDENTITY(1, 1) NOT NULL
	  ,	SubViajeId         INT            NULL
      , assetId            VARCHAR(MAX)   NOT NULL
      , driverId           VARCHAR(MAX)   NOT NULL      
      , TripStart          DATETIME       NOT NULL
      , TripEnd            DATETIME       NOT NULL
      , TripDistance       DECIMAL(11,4)  NOT NULL
      , Litres             DECIMAL(11,4)  NULL
      , StartEngineSeconds INT            NULL
      , EndEngineSeconds   INT            NULL
	  , EngineSecond	   DECIMAL(11,4)  NULL
	  , DriverIds		   INT            NULL
      , IdleOccurs    	   DECIMAL(11,4)  NULL
	  , IdleTime           DECIMAL(11,4)  NULL
        ) ;

        DECLARE @EventsType AS TABLE( EventTypeIdS INT ) ;

		DECLARE @ValoresFactores AS TABLE
		( 
		Sigla VARCHAR(MAX),
		Valor FLOAT		
		) ;

		DECLARE @EventFreno AS TABLE
        (
         DriverId bigint,
		 AssetId Varchar(MAX),
		 Freno int
        ) ;

        DECLARE @Events AS TABLE
        (
         DriverId bigint,
		 AssetId Varchar(MAX),
		 Relenti int,
		 Inercia int
        ) ;

		DECLARE @ReporteBase AS TABLE
        (
        Posicion	      INT
      , Cedula			  VARCHAR(MAX)
      , Nombre		      VARCHAR(MAX)
	  , Vehiculo		  VARCHAR(MAX)
      , DRAcumulada       DECIMAL(11,4)
      , CCAcumulado       DECIMAL(11,4)
	  , DRUDia		      DECIMAL(11,4)
	  , HorasMotor		  DECIMAL(11,4)
	  , UsodeFreno		  DECIMAL(11,4)
	  , PorRalenti		  DECIMAL(11,4)
	  , PorInercia		  DECIMAL(11,4)
	  , Fact_M3           DECIMAL(11,4)

        ) ;

        INSERT INTO @Assets
        (
			assetIdS
			, assetId
			, clienteIdS
			, VehicleID
			, Placa
			, VehicleSiteID
            , VehicleSiteName
        )
         SELECT
             TA.assetIdS
			 , TA.assetId
			 , TA.clienteIdS
			 , TA.assetCodigo
             , TA.assetsDescription
			 , TA.siteIdS
			 , TS.siteName
            FROM dbo.TB_Assets AS TA WITH( NOLOCK )
                 INNER JOIN dbo.TB_Site AS TS WITH( NOLOCK )
                     ON( TS.siteIdS=TA.siteIdS )
           WHERE ta.assetIdS in (SELECT value FROM STRING_SPLIT(@Assetsids, ','))
		   and TA.clienteIdS=@clienteIdS ;

      INSERT INTO @Drivers(  DriverID
							, EmployeeNumber
							, DriverName							
                            , DriverSiteID
							, DriverSiteName
                            )
         SELECT
              TD.driverId			 
			 ,TD.employeeNumber
			 , TD.name
			 , TD.siteIdS
			 , TS.siteName
            FROM Portal.TB_Drivers AS TD WITH( NOLOCK )
                 INNER JOIN dbo.TB_Site AS TS WITH( NOLOCK )
                     ON( TS.siteIds=TD.siteIds )
                 INNER JOIN dbo.TB_Cliente AS TC WITH( NOLOCK )
                     ON( TC.clienteIdS=TS.clienteIdS )
           WHERE TD.driverId in (SELECT value FROM STRING_SPLIT(@DriversIdS, ','))
		   and TS.clienteIdS=@clienteIdS and TD.employeeNumber IS NOT NULL ;

        INSERT INTO @Viajes
        (
            subViajeId
		  , assetId
		  , driverId
		  , TripStart
		  , TripEnd
          , TripDistance
          , Litres
		  , StartEngineSeconds
          , EndEngineSeconds
		  , EngineSecond
		  , DriverIds
		  , M.IdleOccurs		     	  
		  , M.IdleTime
        )
         SELECT
             subViajeId
			, TSV.assetId
			, TSV.driverId
			, TSV.tripStart 
			, tripEnd
			, TSV.distanceKilometers
			, fuelUsedLitres 
			, startEngineSeconds
			, endEngineSeconds 
			, engineSeconds
			, TSV.driverIdS
			, M.IdleOccurs		     	  
		    , M.IdleTime
           FROM dbo.TB_SubViaje AS TSV WITH( NOLOCK )
				LEFT JOIN TB_TripsMetrics as M ON (M.TripId = TSV.tripId)
				 WHERE( TSV.tripStart >= @FechaInicial 
				 and  @FechaFin > TSV.tripStart )
				 --and  dateadd(MS, -3,@FechaFinal) > TSV.tripStart )
					AND TSV.assetIdS IN( SELECT A.assetIdS FROM @Assets AS A )
					AND TSV.subViajeId IS NULL;
     
      INSERT INTO @EventsType( EventTypeIdS )
         SELECT 
             TET.eventTypeIdS
            FROM dbo.TB_EventType AS TET WITH( NOLOCK )
                 INNER JOIN dbo.TB_Site AS TS WITH( NOLOCK )
                     ON TS.clienteIdS=TET.clienteIdS
           WHERE TET.clienteIdS=@clienteIdS and 
		   TET.eventTypeId IN ('-460526757267522254','-930548846283217409')
		   GROUP BY TET.eventTypeIdS;

	 INSERT INTO @ValoresFactores
	 ( 
	  Sigla
	 ,Valor	 
	 )
         SELECT 
             DT.sigla,
			 CAST(Valor AS FLOAT)
				FROM TB_DetalleListas as DT
				inner join TB_Listas as L on L.ListaId = DT.ListaId
			 WHERE L.Sigla = 'PREPEXC';

INSERT INTO @EventFreno
		( 
		DriverId
		, AssetId
		, Freno
		)
		SELECT  
		driverId
		, AssetId
		, [-460526757267522254]
FROM  
( SELECT
            TE.EventTypeId
			, te.driverId
			, te.AssetId
			, sum(TotalOccurances) TotalOccurances
            FROM dbo.TB_Event AS TE WITH( NOLOCK )
         WHERE( TE.EventDateTime BETWEEN @FechaInicial					  
					  and  @FechaFin)
                AND TE.driverId IN( SELECT D.DriverId FROM @Drivers AS D )
                AND TE.eventTypeIdS IN( SELECT ET.EventTypeIdS FROM @EventsType AS ET ) 
				group by TE.EventTypeId, te.driverId, te.AssetId) AS SourceTable  
PIVOT  
(  
Sum(TotalOccurances)  
FOR EventTypeId IN ([-460526757267522254])  
) AS PivotTable2; 

        INSERT INTO @Events
		( 
		DriverId
		, AssetId
		, Inercia 
		)
		SELECT  
		driverId
		, AssetId
		, [-930548846283217409] 
FROM  
( SELECT
            TE.EventTypeId
			, te.driverId
			, te.AssetId
			, sum(TotalTimeSeconds) TotalTimeSeconds
            FROM dbo.TB_Event AS TE WITH( NOLOCK )
         WHERE( TE.EventDateTime BETWEEN @FechaInicial
					  AND  @FechaFin )
                AND TE.driverId IN( SELECT D.DriverId FROM @Drivers AS D )
                AND TE.eventTypeIdS IN( SELECT ET.EventTypeIdS FROM @EventsType AS ET ) 
				group by TE.EventTypeId, te.driverId, te.AssetId) AS SourceTable  
PIVOT  
(  
Sum(TotalTimeSeconds)  
FOR EventTypeId IN ([-930548846283217409])  
) AS PivotTable; 

        INSERT INTO @ReporteBase
        (
            Posicion	      
      , Cedula			 
      , Nombre
	  , Vehiculo
      , DRAcumulada       
      , CCAcumulado      
	  , DRUDia		      
	  , HorasMotor		  
	  , UsodeFreno		  
	  , PorRalenti		  
	  , PorInercia
	  , Fact_M3
        )
         SELECT
             ROW_NUMBER() OVER (ORDER BY TSV.driverId) 
           , TD.EmployeeNumber
		   , TD.DriverName
		   , TA.Placa
		   ,ISNULL( SUM( TripDistance )  , 0 ) 
		   ,ISNULL( SUM( Litres ), 0 )  
		   ,ISNULL(
             ( SELECT
                   SUM( TripDistance )
                  FROM @Viajes as s
                 WHERE 
				 s.DriverId = TSV.driverId and 
				 S.assetId = TSV.assetId and
				   TripStart > DATEADD(DAY,-1,@FechaFinal)), 0 ) 
		   ,ISNULL(         
                   SUM( EngineSecond ), 0 )  ,
				   freno 
				   , ISNULL( SUM( IdleTime )  , 0 ) 
				   ,  inercia
				   , (SELECT Valor from @ValoresFactores 
		              where Sigla = 'Fact_M3') 
         
            FROM @Viajes AS TSV
			
			 left join @EventFreno ef on (ef.DriverId = tsv.DriverId
			 and ef.AssetId = tsv.assetId)
			 left join @Events ev on (ev.DriverId = tsv.DriverId
			 and ev.AssetId = tsv.assetId)
                INNER JOIN @Assets AS TA
                    ON( TA.assetId=TSV.assetId )
                INNER JOIN @Drivers AS TD
                    ON( TD.driverId=TSV.driverId )
					GROUP BY TD.EmployeeNumber, TD.DriverName, ta.Placa, TSV.DriverId
					, tsv.assetId,  freno, inercia;

	SELECT 
	Posicion					       
      , Cedula					  
      , Nombre
	  , Vehiculo
      , ROUND((DRAcumulada), 1)				                     AS DistanciaRecorridaAcumulada     
      , ROUND((CCAcumulado) ,2)			                         AS ConsumodeCombustibleAcumulado     
	  , ROUND((DRUDia) ,1)					                         AS DistanciaRecorridaUltimoDia
	  , ROUND(((DRAcumulada / CCAcumulado) / Fact_M3),2)           AS RendimientoCumbustibleAcumulado
	  , CAST(ROUND(((UsodeFreno * 100.0) / 	DRAcumulada),0) AS INT)	                  AS UsoDelFreno
	  , CAST(ROUND(((PorInercia / (HorasMotor - PorRalenti))* 100.0),0)  AS INT)         AS PorDeInercia
	  , CAST(ROUND(((PorRalenti / HorasMotor)*100.0),0) AS INT)				         AS PorDeRalenti
    FROM @ReporteBase
	ORDER BY Nombre ASC;
	END