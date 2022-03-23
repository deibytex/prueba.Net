
-- =============================================
-- Author:      ygonzalez
-- Create Date: 26/05/2021
-- Description: trae las credenciales de todos los clientes para ejecucion
-- =============================================
CREATE PROCEDURE [PORTAL].[GetCredentialClients]
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON 
		select  Id ,  isnull( [ClienteId],0)  [ClienteId], [ClientSecret],[ClientId],[UserId] ,[Password]	, clientesid	
		from [PORTAL].[CredencialesMix] P		
		where p.esactivo = 1

END