 create procedure SP_ConsultarUsuariosGSeguridad	
 as 
 begin 
 select tu.usuarioIdS,
 tu.usuario,
 tu.nombre,
 tu.apellido
 from TB_Usuarios tu
 end