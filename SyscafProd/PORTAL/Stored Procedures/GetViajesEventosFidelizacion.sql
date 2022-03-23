CREATE PROCEDURE PORTAL.GetViajesEventosFidelizacion
    @EsViaje BIT,
    @ClienteIdS INT,
    @FechaInicial DATETIME,
    @FechaFinal DATETIME,
    @DriverId BIGINT = NULL,
    @AssetIdS INT,
    @EventTypeId BIGINT = NULL
AS
BEGIN
    SET DATEFORMAT YMD;
    ---=====================================================================================================================================
    -- Declaramos variables para los periodos
    ---=====================================================================================================================================
    DECLARE @PeriodoInicial DATETIME
        = CONVERT(VARCHAR(25), DATEADD(dd, - (DAY(@FechaInicial) - 1), @FechaInicial), 101);
    DECLARE @PeriodoFinal DATETIME = CONVERT(VARCHAR(25), DATEADD(dd, - (DAY(@FechaFinal) - 1), @FechaFinal), 101);
    ---=====================================================================================================================================
    -- Declaramos variables para el script, el perdiodo, y el asset
    ---=====================================================================================================================================
    DECLARE @SQLScript NVARCHAR(MAX),
            @PeriodoW VARCHAR(7),
            @PeriodoantEvento DATETIME = @PeriodoInicial;
    DECLARE @AssetId BIGINT;
    ---=====================================================================================================================================
    -- Seteamos la variable asset ya que nos viene el asset ids
    ---=====================================================================================================================================
    SET @AssetId =
    (
        SELECT TOP (1)
               CAST(A.assetId AS BIGINT)
        FROM dbo.TB_Assets AS A
        WHERE A.assetIdS = ISNULL(@AssetIdS, 0)
    );

    ---=====================================================================================================================================
    -- Creamos las tablas temporales, cada vez eliminamos las que esten en memoria
    ---=====================================================================================================================================
    IF OBJECT_ID('tempdb..#TableEventBus') IS NOT NULL
        DROP TABLE #TableEventBus;
    CREATE TABLE #TableEventBus
    (
        EventId BIGINT NOT NULL,
        assetId BIGINT NOT NULL,
        driverId BIGINT NOT NULL,
        EventTypeId BIGINT NOT NULL,
        TotalTimeSeconds INT NOT NULL,
        StartDateTime DATETIME NOT NULL,
        EndDateTime DATETIME NULL,
        Value DECIMAL(11, 4) NULL,
        EsActivo BIT NULL
    );

    IF OBJECT_ID('tempdb..#TableStripBus') IS NOT NULL
        DROP TABLE #TableStripBus;

    CREATE TABLE #TableStripBus
    (
        TripId BIGINT NOT NULL,
        assetId BIGINT NOT NULL,
        driverId BIGINT NOT NULL,
        distanceKilometers DECIMAL(18, 9) NULL,
        startEngineSeconds DECIMAL(18, 9) NULL,
        endEngineSeconds DECIMAL(18, 9) NULL,
        fechasistema DATETIME NOT NULL,
        CantSubtrips INT NULL,
        EsProcesado BIT NOT NULL,
        standingTime DECIMAL(18, 9) NULL,
        tripStart DATETIME NOT NULL,
        tripEnd DATETIME NOT NULL,
        EsActivo BIT NULL
    );

    ---=====================================================================================================================================
    -- Iteramos por cada periodo
    ---=====================================================================================================================================
    WHILE @PeriodoInicial <= @PeriodoFinal
    BEGIN
        ---=====================================================================================================================================
        -- Seteamos el periodo y validamos si es viaje o evento y procedemos a realizar el seteado a el script
        ---=====================================================================================================================================
        SET @PeriodoW
            = CAST(DATEPART(MONTH, @PeriodoInicial) AS VARCHAR) + CAST(DATEPART(YEAR, @PeriodoInicial) AS VARCHAR);
        IF (ISNULL(@EsViaje, 0) = 0)
        BEGIN
            ---=====================================================================================================================================
            -- Seteamos el script
            ---=====================================================================================================================================
            SET @SQLScript
                = N'
					INSERT INTO #TableEventBus
					(	 EventId
						,assetId
						,driverId
						,EventTypeId
						,TotalTimeSeconds
						,StartDateTime
						,EndDateTime
						,Value
						,EsActivo
					)
					SELECT
						EventId
						, assetId
						, driverId
						, EventTypeId
						, TotalTimeSeconds
						, StartDateTime
						, EndDateTime
						, Value
						,ISNULL(EsActivo,0)
				
					FROM [PORTAL].[TB_Event_' + @PeriodoW + N'_' + CAST(@ClienteIdS AS VARCHAR)
                  + N'] WHERE (StartDateTime between @FechaI and  @FechaF) and (assetId = @AssetId)';

            ---=====================================================================================================================================
            -- Se ejecuta el script
            ---=====================================================================================================================================
            EXEC sp_executesql @SQLScript,
                               N'@FechaI datetime, @FechaF DateTime, @AssetId BIGINT',
                               @FechaInicial,
                               @FechaFinal,
                               @AssetId;
            ---=====================================================================================================================================
            -- Consultamos la informacion
            ---=====================================================================================================================================			
            SELECT DISTINCT
                   EventId = CAST(TEB.EventId AS VARCHAR(60)),
                   assetId = CAST(TEB.assetId AS VARCHAR(60)),
                   driverId = CAST(TEB.driverId AS VARCHAR(60)),
                   EventTypeId = CAST(TEB.EventTypeId AS VARCHAR(60)),
                   TEB.TotalTimeSeconds,
                   TEB.StartDateTime,
                   TEB.EndDateTime,
                   TEB.Value,
                   TEB.EsActivo,
                   Conductor = D.name,
                   A.assetsDescription,
                   E.descriptionEvent
            FROM #TableEventBus AS TEB
                INNER JOIN PORTAL.TB_Drivers AS D WITH (NOLOCK)
                    ON TEB.driverId = D.DriverId
                INNER JOIN dbo.TB_Assets AS A WITH (NOLOCK)
                    ON TEB.assetId = A.assetId
                INNER JOIN dbo.TB_EventType AS E WITH (NOLOCK)
                    ON TEB.EventTypeId = CAST(E.eventTypeId AS BIGINT)
                       AND E.clienteIdS = @ClienteIdS
                INNER JOIN PORTAL.ConfiguracionEventosIMG AS CVI WITH (NOLOCK)
                    ON TEB.EventTypeId = CVI.EventTypeId
            WHERE (
                      @DriverId IS NULL
                      OR TEB.driverId = @DriverId
                  )
                  AND
                  (
                      @EventTypeId IS NULL
                      OR TEB.EventTypeId = @EventTypeId
                  );
        END;
        IF (ISNULL(@EsViaje, 0) = 1)
        BEGIN
            ---=====================================================================================================================================
            -- Seteamos el script
            ---=====================================================================================================================================
            SET @SQLScript
                = N'
					INSERT INTO #TableStripBus
					(TripId
					,assetId
					,driverId
					,distanceKilometers
					,startEngineSeconds
					,endEngineSeconds
					,fechasistema
					,CantSubtrips
					,EsProcesado
					,standingTime
					,tripStart
					,tripEnd
					,EsActivo
					)
					SELECT 
						TripId
						,assetId
						,driverId
						,distanceKilometers
						,startEngineSeconds
						,endEngineSeconds
						,fechasistema
						,CantSubtrips
						,EsProcesado
						,standingTime
						,tripStart
						,tripEnd
						,ISNULL(EsActivo,0)
					FROM [PORTAL].[TB_Trips_' + @PeriodoW + N'_' + CAST(@ClienteIdS AS VARCHAR)
                  + N'] 
					WHERE (tripStart between @FechaInicial and @FechaFinal)AND	  
					 EsProcesado = 1 AND assetId = @AssetId';
            ---=====================================================================================================================================
            -- Se ejecuta el script
            ---=====================================================================================================================================		
            EXEC sp_executesql @SQLScript,
                               N'@FechaInicial datetime, @FechaFinal DateTime, @AssetId BIGINT',
                               @FechaInicial,
                               @FechaFinal,
                               @AssetId;
            ---=====================================================================================================================================
            -- Consultamos la informacion
            ---=====================================================================================================================================
            SELECT TripId = CAST(TSB.TripId AS VARCHAR(60)),
                   assetId = CAST(TSB.assetId AS VARCHAR(60)),
                   driverId = CAST(TSB.driverId AS VARCHAR(60)),
                   TSB.distanceKilometers,
                   TSB.startEngineSeconds,
                   TSB.endEngineSeconds,
                   TSB.fechasistema,
                   TSB.CantSubtrips,
                   TSB.EsProcesado,
                   TSB.standingTime,
                   TSB.tripStart,
                   TSB.tripEnd,
                   TSB.EsActivo,
                   Conductor = D.name,
                   A.assetsDescription
            FROM #TableStripBus AS TSB
                INNER JOIN PORTAL.TB_Drivers AS D WITH (NOLOCK)
                    ON TSB.driverId = D.DriverId
                INNER JOIN dbo.TB_Assets AS A WITH (NOLOCK)
                    ON TSB.assetId = A.assetId
            WHERE (
                      @DriverId IS NULL
                      OR TSB.driverId = @DriverId
                  );
        END;
        SET @PeriodoInicial = DATEADD(MONTH, 1, @PeriodoInicial);
    END;
END;