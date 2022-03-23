-- ygonzalez 
-- 02/08/2021
-- ejecuta cadas 4 minutos la actualizacion de la informacion segun las fechas indicadas 
CREATE PROCEDURE TRACE.ExecuteDashBoard
(
    @FechaInicial DATETIME,
    @Fechafinal DATETIME,
    @ClienteIds INT
)
AS
BEGIN





    --DECLARE @FechaInicial DATETIME = N'20211201',
    --        @Fechafinal DATETIME = N'20211215',
    --        @ClienteIds INT = N'909';

    DECLARE @month INT = DATEPART(MONTH, @FechaInicial);
    DECLARE @Year INT = DATEPART(YEAR, @FechaInicial);
    DECLARE @monthFinal INT = DATEPART(MONTH, @Fechafinal);
    DECLARE @YearFinal INT = DATEPART(YEAR, @Fechafinal);
    DECLARE @Period NVARCHAR(30);
    DECLARE @FechaI DATETIME = @FechaInicial;
    DECLARE @Sql NVARCHAR(4000);

    -- declaramos la tabla temporal para traernos los datos de las tablas marcadas con los periodos
    IF OBJECT_ID('tempdb..#Trace_Eventos') IS NOT NULL
        DROP TABLE #Trace_Eventos;
    CREATE TABLE #Trace_Eventos
    (
        EventId BIGINT,
        AssetId BIGINT,
        FuelUsedLitres DECIMAL(11, 4),
        TotalTimeSeconds INT,
        TotalOccurances INT,
        StartDateTime DATETIME,
        EventTypeId BIGINT
    );


    IF OBJECT_ID('tempdb..#Trace_Trips') IS NOT NULL
        DROP TABLE #Trace_Trips;
    CREATE TABLE #Trace_Trips
    (
        TripId BIGINT,
        AssetId BIGINT,
        FuelUsedLitres DECIMAL(11, 4),
        EngineSeconds DECIMAL(11, 4),
        DistanceKilometers DECIMAL(11, 4),
        TripStart DATETIME,
        endOdometerKilometers DECIMAL(11, 4)
    );
    IF OBJECT_ID('tempdb..#Trace_Metricas') IS NOT NULL
        DROP TABLE #Trace_Metricas;
    CREATE TABLE #Trace_Metricas
    (
        TripId BIGINT,
        IdleTime INT
    );


    WHILE (@month <= @monthFinal AND @Year <= @YearFinal)
    BEGIN

        -- armamos el periodo
        SET @Period = CAST(@month AS VARCHAR) + CAST(@Year AS VARCHAR) + N'_' + CAST(@ClienteIds AS VARCHAR);


        -- Si existe el period con datos , traemos la informacion
        SET @Sql
            = N'IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N''[Portal].[TB_Event_' + @Period
              + N']'') AND type in (N''U''))
			BEGIN
				insert into #Trace_Eventos (EventId, AssetId ,   FuelUsedLitres ,    TotalTimeSeconds ,    TotalOccurances ,    StartDateTime ,   EventTypeId )
				select EventId, AssetId ,   FuelUsedLitres ,    TotalTimeSeconds ,    TotalOccurances ,    StartDateTime ,   EventTypeId
				from [Portal].[TB_Event_' + @Period + N'] (NOLOCK)
				where  esprocesado = 0
			END
			';
        EXECUTE sp_executesql @Sql;

        SET @Sql
            = N'IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N''[Portal].[TB_Trips_' + @Period
              + N']'') AND type in (N''U''))
			BEGIN
				insert into #Trace_Trips (TripId , AssetId , FuelUsedLitres ,EngineSeconds , DistanceKilometers ,  TripStart , endOdometerKilometers )
				select TripId , AssetId , FuelUsedLitres ,([endEngineSeconds] - cast([startEngineSeconds] as int)) , DistanceKilometers ,  TripStart , endOdometerKilometers
				from [Portal].[TB_Trips_' + @Period + N']
				where EsProcesado  = 0 
			END			
			';


        EXECUTE sp_executesql @Sql;

        -- Si existe el period con datos , traemos la informacion
        SET @Sql
            = N'IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N''[Portal].[TB_TripsMetrics_'
              + @Period
              + N']'') AND type in (N''U''))
			BEGIN
				insert into #Trace_Metricas (TripId, IdleTime)
				select M.TripId, M.[NIdleTime]
				from [Portal].[TB_TripsMetrics_' + @Period + N'] M 
				inner join [Portal].[TB_Trips_' + @Period
              + N'] T on M.TripId = T.TripId 
				where T.EsProcesado  = 0 
			END
			';

        EXECUTE sp_executesql @Sql;

        SET @FechaI = DATEADD(MONTH, 1, @FechaI);

        SET @month = DATEPART(MONTH, @FechaI);
        SET @Year = DATEPART(YEAR, @FechaI);
    END;




    EXEC dbo.Trace_GetDataRalenti;
    EXEC dbo.Trace_GetDataRendimiento;
    EXEC dbo.Trace_GetDataZonaOperacion 1;
    EXEC dbo.Trace_GetDataZonaOperacion 2;
    EXEC dbo.Trace_GetUltimosOdometros;
    EXEC dbo.Trace_GetPuntuacionEVentos;
    EXEC dbo.Trace_TotalEventCount 46;
    EXEC dbo.Trace_TotalEventCount 47;
    EXEC dbo.Trace_TotalEventCount 49;

    -- actualizamos los procesados


    SET @month = DATEPART(MONTH, @FechaInicial);
    SET @Year = DATEPART(YEAR, @FechaInicial);
    SET @monthFinal = DATEPART(MONTH, @Fechafinal);
    SET @YearFinal = DATEPART(YEAR, @Fechafinal);
    SET @FechaI = @FechaInicial;
    WHILE (@month <= @monthFinal AND @Year <= @YearFinal)
    BEGIN

        -- armamos el periodo
        SET @Period = CAST(@month AS VARCHAR) + CAST(@Year AS VARCHAR) + N'_' + CAST(@ClienteIds AS VARCHAR);


        -- Si existe el period con datos , traemos la informacion
        SET @Sql
            = N'IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N''[Portal].[TB_Event_' + @Period
              + N']'') AND type in (N''U''))
			BEGIN
				update  [Portal].[TB_Event_' + @Period
              + N'] set Esprocesado = 1
				where  EventId in (select EventId From #Trace_Eventos )
			END
			';
        EXECUTE sp_executesql @Sql;

        SET @Sql
            = N'IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N''[Portal].[TB_Trips_' + @Period
              + N']'') AND type in (N''U''))
			BEGIN
				update [Portal].[TB_Trips_' + @Period
              + N'] set Esprocesado = 1
				where TripId in (Select TripID from #Trace_Trips )
			END			
			';

        EXECUTE sp_executesql @Sql;



        SET @FechaI = DATEADD(MONTH, 1, @FechaI);

        SET @month = DATEPART(MONTH, @FechaI);
        SET @Year = DATEPART(YEAR, @FechaI);
    END;


END;
