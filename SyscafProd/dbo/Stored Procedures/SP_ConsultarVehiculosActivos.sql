     
        
CREATE PROCEDURE SP_ConsultarVehiculosActivos              
@clienteId VARCHAR(50)=null           
AS                          
BEGIN                   
SELECT assetId FROM TB_Assets ta, TB_Site ts where ta.groupId=@clienteId  and ts.siteIdS=ta.siteIdS   
and ts.tipoSitio!=9 
END