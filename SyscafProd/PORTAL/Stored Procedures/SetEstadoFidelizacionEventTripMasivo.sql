CREATE PROCEDURE PORTAL.SetEstadoFidelizacionEventTripMasivo
(
    @EsViaje BIT,
    @ClienteIdS INT,
    @FechaInicial DATETIME,
    @Estado BIT,
    @EventId VARCHAR(MAX),
    @TripId VARCHAR(MAX)
)
AS
BEGIN
    SET DATEFORMAT YMD;
    DECLARE @PeriodoInicial DATETIME
        = CONVERT(VARCHAR(25), DATEADD(dd, - (DAY(@FechaInicial) - 1), @FechaInicial), 101);

    DECLARE @SQLScript NVARCHAR(MAX),
            @PeriodoW VARCHAR(7),
            @PeriodoantEvento DATETIME = @PeriodoInicial;
    SET @PeriodoW
        = CAST(DATEPART(MONTH, @PeriodoInicial) AS VARCHAR) + CAST(DATEPART(YEAR, @PeriodoInicial) AS VARCHAR);

    IF (ISNULL(@EsViaje, 0) = 0)
    BEGIN
        SET @SQLScript
            = N'UPDATE [PORTAL].[TB_Event_' + @PeriodoW + N'_' + CAST(@ClienteIdS AS VARCHAR)
              + N'] SET EsActivo = ~EsActivo  WHERE EventId IN (' + @EventId + N')';
        ---=====================================================================================================================================
        -- Se ejecuta el script
        ---=====================================================================================================================================
        EXEC sp_executesql @SQLScript, N' @EventId VARCHAR(MAX)', @EventId;
        PRINT ('Operacón Éxitosa');
    END;
    ELSE
    BEGIN
        SET @SQLScript
            = N'	UPDATE [PORTAL].[TB_Trips_' + @PeriodoW + N'_' + CAST(@ClienteIdS AS VARCHAR)
              + N'] SET EsActivo = ~EsActivo WHERE TripId IN (' + @TripId + N')';
        ---=====================================================================================================================================
        -- Se ejecuta el script
        ---=====================================================================================================================================		
        EXEC sp_executesql @SQLScript, N' @TripId VARCHAR(MAX)', @TripId;
        PRINT ('Operacón Éxitosa');
    END;
END;