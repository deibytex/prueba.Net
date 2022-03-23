-- =============================================
-- Author:     ygonzalez
-- Create Date: 03.02.2022
-- Description: ingresa y actualiza los tipos de eventos por clientes
-- =============================================

CREATE PROCEDURE PORTAL.InsertSites
(
    @Clienteids INT,
    @Sites PORTAL.UDT_Sites READONLY
)
AS
BEGIN




    UPDATE TET
    SET TET.siteName = E.SiteName,
        TET.sitePadreId = E.SitePadreId
    FROM dbo.TB_Site AS TET
        INNER JOIN @Sites AS E
            ON E.SiteId = TET.siteId
    WHERE TET.clienteIdS = @Clienteids;

	
    INSERT INTO dbo.TB_Site
    (
        siteId,
		SiteIdBigInt,
        siteName,
        clienteIdS,
        sitePadreId,
        tipoSitio,
        grupoIdS,
        estadoBase,       
        fechaSistema
    )
    SELECT CAST(SiteId AS VARCHAR),
	        SiteId,
           UPPER(SiteName),
           @Clienteids,
           CAST(SitePadreId AS VARCHAR),
           CASE LOWER(SiteName)
               WHEN 'zona decom' THEN
                   9
               WHEN 'nuevo' THEN
                   11
               ELSE
                   10
           END,
           1,
           1,          
           GETDATE()
    FROM @Sites AS E
    WHERE SiteId NOT IN
          (
              SELECT siteId FROM dbo.TB_Site WHERE clienteIdS = @Clienteids
          );



END;
