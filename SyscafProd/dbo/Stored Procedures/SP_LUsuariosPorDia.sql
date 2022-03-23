            
CREATE PROCEDURE [dbo].[SP_LUsuariosPorDia]                    
 @DESDE DATETIME =null,                  
 @HASTA DATETIME =null,               
 @INTERVALOS int =NULL              
AS                          
BEGIN                   
SELECT DISTINCT            
 tu.nombre          
               
   from TB_Usuarios tu,TB_Assets ta, TB_Cliente tc,TB_Trips tt                    
   where                     
   tc.clienteId=ta.groupId                    
   AND tu.usuarioIdS=tc.usuario                    
   and tt.assetId=ta.assetId                    
   AND ta.estadoClienteIdS=1                    
   AND  (CONVERT(VARCHAR(10),tt.tripStart, 111)) IN (SELECT fechas FROM FN_FechasIntervalos(@DESDE  ,@HASTA , @INTERVALOS  ))        
   group by tu.nombre             
 END