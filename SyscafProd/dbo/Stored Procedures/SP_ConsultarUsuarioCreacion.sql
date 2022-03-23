CREATE PROCEDURE [dbo].[SP_ConsultarUsuarioCreacion]  
@usuarioIdS int = NULL
AS  
BEGIN  
 SELECT nombre, apellido, usuario, tp.perfilIdS, descripcionPerfil, te.estadoIdS as estadoUsuarioIdS, estado, usuarioIdS, documento, correo, telefono, fechaUltimaActualizacion FROM TB_Usuarios AS tu   
 INNER JOIN TB_Perfil AS tp ON tp.perfilIdS = tu.perfilIdS   
 INNER JOIN TB_Estados AS te ON te.estadoIdS = tu.estadoUsuarioIdS   
 WHERE tu.usuarioIdS = @usuarioIdS 
END