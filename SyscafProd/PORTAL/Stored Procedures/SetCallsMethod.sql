
-- ygonzalez 09/05/2021
-- inserta e incrementa las llamadas 
CREATE procedure [PORTAL].[SetCallsMethod](
@CredencialId int ,
@date dateTIME,
@dateHour dateTIME,
@HourCall int,
@MinuteCall int
)
as
begin
		 if exists (select * from PORTAL.DataStopWatch
		 where  [ClienteCredencialId]= @CredencialId )
		 begin			
						update  PORTAL.DataStopWatch
							set DateCall = @date,
								[TotalCalls] = @MinuteCall,
								[TotalCallsHour] = @HourCall,
								[dateHour] =@dateHour
								where  [ClienteCredencialId]= @CredencialId
			end		
		 else 
		 begin 

			insert into [PORTAL].[DataStopWatch]([ClienteCredencialId],[TotalCalls],[TotalCallsHour],[DateCall],[dateHour])
			values (@CredencialId  ,1 ,1 ,@date , @dateHour	)

		 end
end

---select * from [PORTAL].[DataStopWatch]
-- update [PORTAL].[DataStopWatch] set datecall = null, totalcalls = 0, totalcallhour = 0
--alter table [PORTAL].[DataStopWatch] add dateHour datetime