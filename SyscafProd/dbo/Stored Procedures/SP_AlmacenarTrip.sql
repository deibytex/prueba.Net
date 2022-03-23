      
CREATE PROCEDURE SP_AlmacenarTrip                
@AssetId VARCHAR(50)=null,            
@DistanceKilometers NUMERIC(18,4)=null,             
@DriverId VARCHAR(50)=null,             
@DrivingTime NUMERIC(18,4) =null,              
@Duration NUMERIC(18,4)=null,             
@EndEngineSeconds INT=null,            
@EndOdometerKilometers NUMERIC(18,4)=null,            
@EndPositionId VARCHAR(50)=null,             
@EngineSeconds INT=null,              
@FirstDepart DATETIME=null,             
@FuelUsedLitres NUMERIC(18,4)=null,            
@LastHalt DATETIME=null,            
@MaxAccelerationKilometersPerHourPerSecond NUMERIC(18,4)=null,             
@MaxDecelerationKilometersPerHourPerSecond NUMERIC(18,4)=null,             
@MaxRpm NUMERIC(18,4)=null,             
@MaxSpeedKilometersPerHour NUMERIC(18,4)=null,                 
@StandingTime NUMERIC(18,4)=null,             
@StartEngineSeconds NUMERIC(18,4)=null,              
@StartOdometerKilometers NUMERIC(18,4)=null,              
@StartPositionId VARCHAR(50)=null,              
@TripEnd DATETIME=null,             
@TripId VARCHAR(50)=NULL,              
@TripStart DATETIME  =null          
AS                          
BEGIN                             
   IF NOT EXISTS (SELECT * FROM TB_Trips                       
   WHERE TB_Trips.TripId = @TripId and TB_Trips.assetId=@AssetId)                
BEGIN               
 INSERT INTO TB_Trips                   
 VALUES             
 (@AssetId            
,@DistanceKilometers            
,@DriverId            
,@DrivingTime            
,@Duration            
,@EndEngineSeconds            
,@EndOdometerKilometers            
,@EndPositionId            
,@EngineSeconds            
,DATEADD(hour,-5,@FirstDepart)            
,@FuelUsedLitres            
,DATEADD(hour,-5,@LastHalt)         
,@MaxAccelerationKilometersPerHourPerSecond            
,@MaxDecelerationKilometersPerHourPerSecond            
,@MaxRpm            
,@MaxSpeedKilometersPerHour                
,@StandingTime            
,@StartEngineSeconds            
,@StartOdometerKilometers            
,@StartPositionId            
,DATEADD(hour,-5,@TripEnd)          
,@TripId            
,DATEADD(hour,-5,@TripStart)       
)                           
END                          
SELECT TOP 1 * FROM TB_Trips WHERE TB_Trips.TripId = @TripId and TB_Trips.assetId=@AssetId                
END