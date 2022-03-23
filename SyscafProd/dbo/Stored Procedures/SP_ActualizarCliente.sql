
CREATE PROCEDURE SP_ActualizarCliente      
 @clienteIdS int,      
 @clienteNombre varchar(100) ,      
 @usuario int,      
 @countryIdS INT,      
 @telefono INT,      
 @planComercial varchar(100),      
 @nit varchar(50)      
AS      
BEGIN    
 UPDATE TB_Cliente      
 SET     
 TB_Cliente.clienteNombre=@clienteNombre,    
 TB_Cliente.usuario=@usuario,    
 TB_Cliente.countryIdS=@countryIdS,    
 TB_Cliente.telefono=@telefono,    
 TB_Cliente.planComercial=@planComercial,    
 TB_Cliente.nit=@nit    
 WHERE    
 TB_Cliente.clienteIdS=@clienteIdS    
 SELECT * FROM TB_Cliente WHERE  TB_Cliente.clienteIdS=@clienteIdS  
END