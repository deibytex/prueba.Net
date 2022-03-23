
	  
CREATE PROCEDURE SP_AlmacenarSubTrips              
@Depart datetime =null,     
@DistanceKilometres decimal(16,4) =null,     
@DrivingTime int =null,     
@Duration int =null,     
@EndEngineSeconds int =null,     
@EndOdometerKilometres decimal(16,4) =null,     
@EndPositionId varchar(50) =null,     
@EngineSeconds int =null,     
@FuelUsedLitres decimal(16,4) =null,     
@Halt datetime =null,     
@MaxAccelerationKilometersPerHourPerSecond decimal(16,4) =null,     
@MaxDecelerationKilometersPerHourPerSecond decimal(16,4) =null,     
@MaxRpm decimal(16,4) =null,     
@MaxSpeedKilometersPerHour decimal(16,4) =null,     
@PulseValue decimal(16,4) =null,     
@StandingTime int =null,     
@StartEngineSeconds int =null,     
@StartOdometerKilometres decimal(16,4) =null,     
@StartPositionId varchar(50) =null,     
@SubTripEnd datetime =null,     
@SubTripStart datetime =null,     
@TripId varchar(50) null      
AS                            
BEGIN                               
   IF NOT EXISTS (SELECT * FROM TB_SubTrips                         
   WHERE TB_SubTrips.TripId = @TripId and TB_SubTrips.EndPositionId=@EndPositionId and TB_SubTrips.StartPositionId=@StartPositionId)                  
BEGIN                 
 INSERT INTO TB_SubTrips                     
 VALUES               
 (  @Depart  ,      
@DistanceKilometres  ,      
@DrivingTime  ,      
@Duration  ,      
@EndEngineSeconds  ,      
@EndOdometerKilometres  ,      
@EndPositionId  ,      
@EngineSeconds  ,      
@FuelUsedLitres  ,      
@Halt  ,      
@MaxAccelerationKilometersPerHourPerSecond  ,      
@MaxDecelerationKilometersPerHourPerSecond  ,      
@MaxRpm  ,      
@MaxSpeedKilometersPerHour  ,      
@PulseValue  ,      
@StandingTime  ,      
@StartEngineSeconds  ,      
@StartOdometerKilometres  ,      
@StartPositionId  ,      
@SubTripEnd  ,      
@SubTripStart  ,      
@TripId  )                             
END                            
SELECT TOP 1 * FROM TB_SubTrips WHERE TB_SubTrips.TripId = @TripId  and TB_SubTrips.EndPositionId=@EndPositionId and TB_SubTrips.StartPositionId=@StartPositionId               
END