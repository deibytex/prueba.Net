-- =============================================
-- Author:      MDiaz
-- Create Date: 22.02.2022
-- Description: Asigna automaticamente los adminitrador por defecto a los sites
-- =============================================

CREATE PROCEDURE PORTAL.InsertarSite
AS
BEGIN
    DECLARE @Clienteids INT;
    DECLARE cmds CURSOR FOR
    SELECT DISTINCT
           TS.clienteIdS
    FROM dbo.TB_AdministradorClientes AS TAC
        RIGHT OUTER JOIN dbo.TB_Site AS TS
            ON TS.siteIdS = TAC.SiteIds
    WHERE TS.tipoSitio <> 9
          AND TAC.AdministradorId IS NULL;

    OPEN cmds;
    WHILE 1 = 1
    BEGIN
        FETCH cmds
        INTO @Clienteids;
        IF @@fetch_status != 0
            BREAK;
        --=====================================================================================================
        --1 Si tiene un sólo administrador de flota por cliente será asignado a ese nuevo site.
        --=====================================================================================================

        DECLARE @Count INT =
                (
					SELECT COUNT(1) FROM (
                    SELECT (AC.UsuarioIds)
                    FROM dbo.TB_AdministradorClientes  AS AC
                    WHERE AC.ClienteIds IN ( @Clienteids )
					GROUP BY AC.UsuarioIds ) T

                );

		
     
        DECLARE @UsuarioId INT
            = CASE @Count
                  --=====================================================================================================
                  --1 se le asigna el administrador que tiene asignado por defecto
                  --=====================================================================================================
                  WHEN 1 THEN
                  (
                      SELECT DISTINCT
                             AC.UsuarioIds
                      FROM dbo.TB_AdministradorClientes AS AC
                      WHERE AC.ClienteIds IN ( @Clienteids )
                  )
                  ELSE
                      --=====================================================================================================
                      --1 No tiene administrador será asignado el usuario de soporte que es el 25.
                      --3. Si tiene mas de un admon de flota se asiganrá el  usuario de soporte que es el 25.
                      --=====================================================================================================
                      25
              END;


        --=====================================================================================================
        --Se insertan los sitios nuevos
        --=====================================================================================================
        INSERT INTO dbo.TB_AdministradorClientes
        (
            UsuarioIds,
            ClienteIds,
            SiteIds,
            FechaSistema,
            EsActivo
        )
        SELECT DISTINCT
               @UsuarioId,
               @Clienteids,
               TS.siteIdS,
               GETDATE(),
               1
        FROM dbo.TB_AdministradorClientes AS TAC
            RIGHT OUTER JOIN dbo.TB_Site AS TS
                ON TS.siteIdS = TAC.SiteIds
        WHERE TS.tipoSitio <> 9
              AND TAC.AdministradorId IS NULL
              AND TS.clienteIdS = @Clienteids;
    END;
    CLOSE cmds;
    DEALLOCATE cmds;
END;


