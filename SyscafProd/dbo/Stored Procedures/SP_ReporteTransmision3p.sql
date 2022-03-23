
--===========================================================================================
---- ygonzalez mod = los vehiculos que vuelven a transmitir se les cambia el estado a activo
--===========================================================================================

CREATE PROCEDURE dbo.SP_ReporteTransmision3p 
    @usuarioIdS INT,
    @clienteIdS INT = NULL
AS
BEGIN
    DECLARE @TABLE TABLE
    (
        Id INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
        assetCodigo INT,
        registrationNumber VARCHAR(200) COLLATE Modern_Spanish_CI_AS,
        assetsDescription VARCHAR(200),
        diffAVL INT,
        AVL DATETIME,
        clientenNombre VARCHAR(200),
        clienteIdS INT,
        Sitio VARCHAR(200),
        nombre VARCHAR(200),
        apellido VARCHAR(200),
        estado VARCHAR(200)
    );

	DECLARE @FechaActual DATETIME =  DATEADD(MINUTE, 30, DATEADD(HOUR, 10 ,CAST(CAST(GETDATE() AS DATE) AS datetime)))


    INSERT INTO @TABLE
    SELECT DISTINCT
           ta.assetCodigo,
           ta.registrationNumber,
           ta.assetsDescription,
           tp.DiasSinTx,
           tp.UltimoAvl,
           tc.clienteNombre,
           tc.clienteIdS,
           ts.siteName AS 'Sitio',
           tu.nombre,
           tu.apellido,
           te.estado AS estadoSyscaf
    FROM dbo.TB_Assets AS ta WITH (NOLOCK)
        INNER JOIN (SELECT *	 FROM  dbo.fn_VehiculosSinTransmision(@FechaActual) )AS tp
            ON (ta.assetIdS = tp.assetIdS)
        INNER JOIN TB_AdministradorClientes AS tac
            ON ta.clienteIdS = tac.ClienteIds
               AND ta.siteIdS = tac.SiteIds
        INNER JOIN dbo.TB_Usuarios AS tu WITH (NOLOCK)
            ON (tu.usuarioIdS = tac.UsuarioIds)
        INNER JOIN dbo.TB_Cliente AS tc WITH (NOLOCK)
            ON (tc.clienteIdS = ta.clienteIdS)
        INNER JOIN dbo.TB_Site AS ts WITH (NOLOCK)
            ON (ts.siteIdS = ta.siteIdS)
        INNER JOIN
        (
            SELECT *
            FROM VW_GrupoSeguridadSite AS vgsi
            WHERE (vgsi.usuarioIdS = @usuarioIdS)
                  AND
                  (
                      @clienteIdS IS NULL
                      OR vgsi.ClienteIds = @clienteIdS
                  )
        ) AS vgs
            ON ta.siteIdS = vgs.SiteIds
        INNER JOIN dbo.TB_Estados AS te WITH (NOLOCK)
            ON (te.estadoIdS = ta.estadoSyscafIdS)
    WHERE ta.estadoClienteIdS = 1
          AND ts.tipoSitio <> 9
          AND ta.siteIdS NOT IN ( 5719, 5720, 5721, 5722 )
          AND tc.clienteIdS NOT IN ( 856, 842 )
    ORDER BY tc.clienteNombre,
             ts.siteName,
             tp.DiasSinTx;

    SELECT Id,
           assetCodigo,
           registrationNumber,
           assetsDescription,
           diffAVL,
           AVL,
           REPLACE(
                      REPLACE(REPLACE(REPLACE(clientenNombre, 'Col FV Itaú ', ''), 'Col FV Syscaf ', ''), 'Col FV ', ''),
                      'Itaú ',
                      ''
                  ) AS clientenNombre,
           clientenNombre Cliente,
           Sitio,
           CONCAT(   (CASE
                          WHEN (CHARINDEX(' ', nombre, 1)) = 0 THEN
                              nombre
                          ELSE
                              SUBSTRING(nombre, 1, CHARINDEX(' ', nombre, 1))
                      END
                     ),
                     ' ',
                     (CASE
                          WHEN (CHARINDEX(' ', apellido, 1)) = 0 THEN
                              apellido
                          ELSE
                              SUBSTRING(apellido, 1, CHARINDEX(' ', apellido, 1))
                      END
                     )
                 ) AS nombre,
           estado AS estadoSyscaf
    FROM @TABLE;

END;
