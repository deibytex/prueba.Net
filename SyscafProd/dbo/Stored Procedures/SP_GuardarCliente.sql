
CREATE PROCEDURE SP_GuardarCliente          
 @clienteIdS int,          
 @clienteNombre varchar(100) ,          
 @usuario int,          
 @countryIdS INT,          
 @telefono INT,          
 @planComercial varchar(100),          
 @nit varchar(50),
 @estadoClienteSyscaf int
AS          
BEGIN        
 UPDATE TB_Cliente          
 SET         
 TB_Cliente.clienteNombre=@clienteNombre,        
 TB_Cliente.usuario=@usuario,        
 TB_Cliente.countryIdS=@countryIdS,        
 TB_Cliente.telefono=@telefono,        
 TB_Cliente.planComercial=@planComercial,        
 TB_Cliente.nit=@nit,
 TB_Cliente.estadoClienteIdS=@estadoClienteSyscaf
 WHERE        
 TB_Cliente.clienteIdS=@clienteIdS        
 SELECT * FROM TB_Cliente WHERE  TB_Cliente.clienteIdS=@clienteIdS      
END