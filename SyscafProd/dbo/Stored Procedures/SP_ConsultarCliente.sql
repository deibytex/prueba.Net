CREATE PROCEDURE SP_ConsultarCliente      
@idCliente int      
AS       
BEGIN      
SELECT TOP 1    
tc.clienteIdS,    
tc.clienteNombre,    
tc.usuario,    
tu.usuario as usuarioNombre,    
tc.countryIdS,    
tp.descripcion as countryNombre,    
tc.telefono,    
tc.planComercial,    
tc.nit,    
tc.fechaIngreso ,  
tc.estadoClienteIdS,  
tec.estado  
FROM     
TB_Cliente tc  LEFT JOIN TB_Country tp on (tc.countryIdS=tp.countryIdS)     
 LEFT JOIN TB_Usuarios tu on (tc.usuario=tu.usuarioIdS)    
 LEFT JOIN TB_Estados tec on (tc.estadoClienteIdS=tec.estadoIdS)  
 WHERE TC.clienteIdS=@idCliente      
END