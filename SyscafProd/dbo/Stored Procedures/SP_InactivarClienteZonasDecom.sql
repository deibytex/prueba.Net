CREATE PROCEDURE SP_InactivarClienteZonasDecom                
AS                   
BEGIN     
    
 UPDATE TB_Cliente SET estadoClienteIdS=0 WHERE TB_Cliente.clienteIdS IN(    
 select tc.clienteIdS    
 from     
 TB_Cliente tc LEFT JOIN TB_Site ts ON (ts.clienteIdS=tc.clienteIdS)    
 LEFT JOIN TB_Assets ta ON (ta.siteIdS=ts.siteIdS)    
 where ts.tipoSitio!=9    
 GROUP BY tc.clienteIdS    
 HAVING COUNT(ta.assetId)=0)    
end