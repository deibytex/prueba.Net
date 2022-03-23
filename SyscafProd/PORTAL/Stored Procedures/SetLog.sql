-- =============================================
-- Author:     ygonzalez
-- Create Date: 26/05/2021
-- Description: guarda el log de uso de las credenciales de mix 
-- =============================================
CREATE PROCEDURE [PORTAL].[SetLog]
(
	@CredencialId int,
	@Method  varchar(200),
	@StatusResponse int ,
	@Response varchar(max) ,
	@FechaSistema datetime,
	@Clienteids int = null
)
AS
BEGIN 

	Insert into PORTAL.LogEndPoindMix(CrendencialesId, Method, StatusResponse, Response ,FechaSistema, clienteids )
	values (@CredencialId ,	@Method  ,	@StatusResponse  ,	@Response  ,	@FechaSistema, @Clienteids )

END