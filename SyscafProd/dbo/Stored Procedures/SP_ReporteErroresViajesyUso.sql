CREATE PROCEDURE [dbo].[SP_ReporteErroresViajesyUso]
( 
	  @FechaInicial DATETIME
    , @FechaFinal   DATETIME
    , @clienteIdS   INT
)
AS
BEGIN
    SET NOCOUNT ON ;
    SELECT
        row_number() over (order by TripStart ) Consecutivo,
		Cliente, 
		US, 
		VehicleID, 
		Placa, 
		VehicleSiteID, 
		VehicleSiteName, 
		cast (TripNo as varchar(50)) + '_' TripNo, 
		DriverID, 
		DriverName, 
		DriverSiteID, 
		DriverSiteName, 
		OriginalDriverID, 
		OriginalDriverName, 
		convert(varchar(10), TripStart, 103)  +' ' + convert(varchar(10), TripStart, 108) TripStart, 
		convert(varchar(10), TripEnd, 103)  +' ' + convert(varchar(10), TripEnd, 108) TripEnd, 
		CategoryID, 
		Notes, 
		StartSubTripSeq, 
		EndSubTripSeq, 
		TripDistance = REPLACE(CAST(ISNULL(Convert(DECIMAL(13,2),TripDistance),0) AS VARCHAR(20)),'.',','),
		Odometer = REPLACE(CAST(ISNULL(Convert(DECIMAL(13,2),Odometer),0) AS VARCHAR(20)),'.',','),
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
		Litres = REPLACE(CAST(ISNULL(Convert(DECIMAL(13,2), Litres),0) AS VARCHAR(20)),'.',','),
		cast (StartGPSID as varchar(50)) + '_' StartGPSID, 
		cast (EndGPSID as varchar(50)) + '_' EndGPSID, 
		StartEngineSeconds, 
		EndEngineSeconds, 
		ClienteIdS
    FROM 
		TB_ErroresViajesyUso WITH( NOLOCK )
    WHERE 
		TripStart BETWEEN @FechaInicial AND @FechaFinal
            AND 
		ClienteIdS = @ClienteIds
    ORDER BY 
		TripStart;
END ;