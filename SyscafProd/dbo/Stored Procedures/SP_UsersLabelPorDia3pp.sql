CREATE PROCEDURE [dbo].[SP_UsersLabelPorDia3pp]                          
 
AS                                
BEGIN
	select usuario as nombre from TB_Usuarios as U	inner join TB_Empleados as E on (U.usuarioIdS = E.usuarioIdS)
	where E.cargoIdS = 1
END