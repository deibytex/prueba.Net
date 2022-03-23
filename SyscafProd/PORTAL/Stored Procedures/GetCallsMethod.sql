-- ygonzalez 09/05/2021
CREATE procedure [PORTAL].[GetCallsMethod](
@CredencialId int 
)
as
begin

 select	[ClienteCredencialId],[TotalCalls],[TotalCallsHour],[DateCall],[HourCall],[MinuteCall], datehour
 from PORTAL.DataStopWatch
 where  [ClienteCredencialId]= @CredencialId
end