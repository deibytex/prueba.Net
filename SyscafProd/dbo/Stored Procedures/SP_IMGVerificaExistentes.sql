
-- =============================================
-- Author:		ygonzalez
-- Create date: 30/03/2021
-- Description:	Devuelve los eventos que no se encuentren en la base de datos 
-- =============================================
CREATE PROCEDURE dbo.SP_IMGVerificaExistentes
(
@FInicioMes date,
@ClienteIds int,
@Ids [UDT_TableIdentity] READONLY
)
AS
BEGIN
	
		select Id from @Ids
		where Id  not in (    
		select TripNo from TB_ErroresViajesyUso e
		where TripStart > @FInicioMes and ClienteIdS =@ClienteIds)

END