  
CREATE procedure SP_ConsultarEmpleados  
  @empleadoIdS int =NULL     
 as     
 begin    
 select  
 te.empleadoIdS,  
 te.usuarioIdS,  
 te.cargoIdS,  
 (select tu.nombre from TB_Usuarios tu where tu.usuarioIdS=te.usuarioIdS) as usuarioNombre,  
 (select tc.nombre from TB_Cargos tc where tc.cargosIdS=te.cargoIdS) as cargoNombre  
 from TB_Empleados te  
  where te.empleadoIdS=ISNULL(@empleadoIdS, te.empleadoIdS)       
 end