          
CREATE PROCEDURE [dbo].[SP_TransmisionRellenoTablas]                                      
@usuarioIdS int = null,          
@clienteIdS int = null          
AS                    
DECLARE  @TABLE TABLE (              
 assetCodigo INT                    
, registrationNumber  VARCHAR(200)           
, assetsDescription VARCHAR(200)          
, diffAVL INT              
, AVL datetime              
, clientenNombre VARCHAR(200)          
, clienteIdS INT          
, Sitio VARCHAR(200)
, siteIdS INT
, nombre   VARCHAR(200)                   
, estado VARCHAR(200)                 
)              
              

-- elimina la informacion de barras antes de llenarla
delete from TB_TransmisionLineayBarras
where fecha =  CONVERT(VARCHAR(10), GETDATE(), 111)


	 
INSERT INTO @TABLE              
SELECT                     
ta.assetCodigo                     
, ta.registrationNumber          
, ta.assetsDescription          
,TP.DiasSinTx
,TP.UltimoAvl            
,tc.clienteNombre             
,tc.clienteIdS          
, ts.siteName as 'Sitio'
, ts.siteIdS
, tu.nombre                 
, te.estado as estadoSyscaf                
from                  
dbo.TB_Assets AS ta WITH( NOLOCK )
				INNER JOIN  
		VW_VehiculosSinTransmision AS tp
					ON( ta.assetIdS=tp.assetIdS )   
			INNER JOIN
		TB_AdministradorClientes tac
				ON
				ta.clienteIdS = tac.ClienteIds
						AND
				ta.siteIdS = tac.SiteIds
			INNER JOIN  
		dbo.TB_Usuarios AS tu WITH( NOLOCK )
				ON( tu.usuarioIdS=tac.usuarioIdS )
				INNER JOIN 
		dbo.TB_Cliente AS tc WITH( NOLOCK )
				ON( tc.clienteIdS=ta.clienteIdS )
				INNER JOIN  
		dbo.TB_Site AS ts WITH( NOLOCK )
				ON( ts.siteIdS=ta.siteIdS )
			INNER JOIN  
		(SELECT
				*
			FROM
				VW_GrupoSeguridadSite vgsi
			WHERE
				(vgsi.usuarioIdS = @usuarioIdS)
					AND
				(@clienteIdS IS NULL OR vgsi.clienteIdS = @clienteIdS)
		) AS vgs
				ON
					ta.siteIdS = vgs.SiteIds
				INNER JOIN  
		dbo.TB_Estados AS te WITH( NOLOCK )
				ON( te.estadoIdS=ta.estadoSyscafIdS )
							
									                  
WHERE ta.estadoClienteIdS=1                    
and ts.tipoSitio =10   and                
ta.estadoClienteIdS=1 	AND 
tc.clienteIdS not in (856,842)                  
ORDER BY tc.clienteNombre DESC, ta.registrationNumber DESC              
    
IF (CONVERT(VARCHAR(10), GETDATE(), 111)<>(SELECT MAX(fecha) FROM TB_TransmisionLineayBarras))  
 BEGIN   
  INSERT INTO TB_TransmisionLineayBarras   
  SELECT    
  CONVERT(VARCHAR(10), GETDATE(), 111) AS fecha    
  , count(assetsDescription) as total        
  , clientenNombre    
  , clienteIdS  
  , nombre     
  ,@usuarioIdS
  , siteIdS
  FROM @TABLE              
  where clienteIdS=ISNULL(@clienteIdS, clienteIdS)    
  group by     
   clientenNombre    
  , clienteIdS  
  , nombre   
  , siteIdS
 END