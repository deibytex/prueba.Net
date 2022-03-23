 CREATE FUNCTION [dbo].[FN_CantidadTransmisionCliente] (@clienteId varchar(50),@fecha datetime)
   RETURNS INT
   AS
   BEGIN
    DECLARE @trasmision INT=0 
			set @trasmision=( select 
		 count (DISTINCT ta.assetId ) as totalAssetsTrans

		 from TB_Assets ta, TB_Trips tt,TB_Senales ts
		 WHERE tt.assetId=ta.assetId
		 and ts.assetsIdS=ta.assetIdS
		 AND DATEADD(DAY,-1*ts.diasTransmision,@fecha)<=tt.tripStart
		 and @fecha>=tt.tripStart
		 and ta.groupId=@clienteId
		 group by 
		 ta.groupId)	
	RETURN ISNULL(@trasmision, 0)    
	end