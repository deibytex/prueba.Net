
-- =============================================
-- Author:     ygonzalez
-- Create Date: 16/06/2021
-- Description: inserta o actualiza el expirationtoken
-- =============================================
CREATE PROCEDURE  Peg.setExpirationToken
(
   @token varchar(max)
)
AS
BEGIN
   

   if exists(select 1 from [PEG].[Token])
   begin
		update top(1) [PEG].[Token] set token = @token,  [ExpirationDate]= dateadd(hour, 11, getdate())
   end
   else
   begin

   insert into [PEG].[Token]([Token],[ExpirationDate])
   values(@token, dateadd(hour, 11, getdate()))
   end
END