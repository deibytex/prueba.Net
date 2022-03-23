

CREATE procedure SP_ActualizacionesFinales  
as   
begin  
  
  UPDATE TB_Assets   
 SET estadoClienteIdS=0  
  where assetIdS IN (SELECT assetIdS FROM TB_Assets a  
 LEFT JOIN TB_Site AS s ON (s.siteIdS=a.siteIdS)        
 WHERE    
 s.tipoSitio <> 11  
 and s.tipoSitio <> 10 
 and estadoClienteIdS=1)  
UPDATE TB_Assets   
 SET estadoClienteIdS=1  
  where assetIdS IN (SELECT assetIdS FROM TB_Assets a  
 LEFT JOIN TB_Site AS s ON (s.siteIdS=a.siteIdS)        
 WHERE    
 s.tipoSitio <> 11 
 and s.tipoSitio=10  
 and estadoClienteIdS=0)  
end