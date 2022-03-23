

 CREATE procedure  [dbo].[SP_AgregarSenales]  
 @senalesIdS int=NULL, 
 @assetsIdS int=NULL, 
 @diasTransmision int=NULL, 
 @maxVelFM int=NULL, 
 @minVelFM int=NULL, 
 @RPM_Max int=NULL, 
 @RPM_Min int=NULL, 
 @MaxAccelereciones int=NULL, 
 @MinAccelereciones int=NULL
as   
begin   
IF (@senalesIdS!=0)  
	 BEGIN  
	 UPDATE TB_Senales
	 SET
	 TB_Senales.diasTransmision= @diasTransmision , 
	 TB_Senales.maxVelFM= @maxVelFM , 
	 TB_Senales.minVelFM= @minVelFM , 
	 TB_Senales.RPM_Max= @RPM_Max , 
	 TB_Senales.RPM_Min= @RPM_Min , 
	 TB_Senales.MaxAccelereciones= @MaxAccelereciones , 
	 TB_Senales.MinAccelereciones= @MinAccelereciones 
	 WHERE  TB_Senales.senalesIdS=@senalesIdS
	 END
SELECT * FROM TB_Senales WHERE  TB_Senales.senalesIdS=@senalesIdS
end