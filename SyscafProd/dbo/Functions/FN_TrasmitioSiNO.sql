   CREATE FUNCTION [dbo].[FN_TrasmitioSiNO] (@assetId varchar(50),@fecha varchar(10))
   RETURNS INT
   AS
   BEGIN
    DECLARE @trasmision INT=0 
	set @trasmision=(select 
	COUNT (DISTINCT ta.assetId) 	
	from TB_Assets ta, TB_Senales ts, TB_Trips tt
	where ta.assetIdS=ts.assetsIdS
	and tt.assetId=ta.assetId
	and ta.assetId=@assetId
	and tt.tripStart BETWEEN DATEADD(DAY,-3,@fecha) AND DATEADD(DAY,+1,@fecha))	
	RETURN @trasmision
	end