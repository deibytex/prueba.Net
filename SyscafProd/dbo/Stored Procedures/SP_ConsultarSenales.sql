 CREATE procedure  [dbo].[SP_ConsultarSenales]  
 @senalesIdS int=NULL
as   
begin   
SELECT 
ts.senalesIdS,
ts.assetsIdS,
ta.assetsDescription,
ta.registrationNumber as assetsRegistration,
ts.diasTransmision,
ts.maxVelFM,
ts.minVelFM,
ts.RPM_Max,
ts.RPM_Min,
ts.MaxAccelereciones,
ts.MinAccelereciones
FROM TB_Senales ts , TB_Assets ta
WHERE  ts.senalesIdS=ISNULL(@senalesIdS,ts.senalesIdS)
and ta.assetIdS=ts.assetsIdS
end