---- dlopez 24/11/2020 - trae la informacion de tx de los vehiculos por clientes 

CREATE PROCEDURE dbo.SP_ReporteTransmisionCorreo
    -- DECLARE
    @ListaClienteNotifacionId INT = NULL,
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
        clienteNombre VARCHAR(200),
        clienteIdS INT,
        Sitio VARCHAR(200),
        estado VARCHAR(200)
    );

    DECLARE @VehiculosSinTX TABLE
    (
        AssetIdS INT NOT NULL,
        diffAVL INT NOT NULL,
        AVL DATETIME NOT NULL
    );


    -- DANGER-- HAY QUE CAMBIARLO
    DECLARE @FechaActual DATETIME = CONVERT(DATETIME, SWITCHOFFSET(CONVERT(DATETIMEOFFSET, GETUTCDATE()), '-05:00'));

    DECLARE @minuteDia DECIMAL(18, 2) = 1440;

    --- YGONZALEZ 22/11/2020
    --- REALIZA LA CONSULTA DE LOS VEHICULOS QUE SE ENCUENTRAN SIN TRANSMISION Y CAMBIA LOS ESTADOS DE CADA /UNO
    INSERT INTO @VehiculosSinTX
    (
        AssetIdS,
        diffAVL,
        AVL
    )
    SELECT TP.assetIdS,
           DATEDIFF(DAY, MAX(TP.Timestamp), @FechaActual) DiasSinTx,
           MAX(TP.Timestamp) UltimoAvl
    FROM dbo.TB_Positions AS TP
        LEFT JOIN dbo.TB_Senales AS TS
            ON TS.assetsIdS = TP.assetIdS
    GROUP BY TP.assetIdS,
             TS.diasTransmision
    HAVING ((DATEDIFF(MINUTE, MAX(TP.Timestamp), @FechaActual) / @minuteDia) >= ISNULL(TS.diasTransmision, 1));


    -- vehiculos que ya transmite y deben cambiar estado activo
    UPDATE TA
    SET TA.estadoSyscafIdS = 3
    FROM dbo.TB_Assets AS TA
        INNER JOIN dbo.TB_Estados AS TE
            ON TE.estadoIdS = TA.estadoSyscafIdS
    WHERE TA.estadoClienteIdS = 1
          AND TE.tipoIdS = 3
          AND TA.assetIdS NOT IN
              (
                  SELECT AssetIdS FROM @VehiculosSinTX
              );

    -- vehiculos que estan activos y pasaron sin tx

    UPDATE dbo.TB_Assets
    SET estadoSyscafIdS = 8
    WHERE assetIdS IN
          (
              SELECT AssetIdS FROM @VehiculosSinTX
          )
          AND estadoSyscafIdS = 3;

    --- FIN RE VALIDACION

    -- Deiby López
    -- Se cambia la tabla de donde se optienen los sites

    INSERT INTO @TABLE
    SELECT DISTINCT
           ta.assetCodigo,
           ta.registrationNumber,
           ta.assetsDescription,
           tp.diffAVL,
           tp.AVL,
           tc.clienteNombre,
           tc.clienteIdS,
           ts.siteName AS 'Sitio',
           te.estado AS estadoSyscaf
    FROM dbo.TB_Assets AS ta WITH (NOLOCK)
        INNER JOIN @VehiculosSinTX AS tp
            ON (ta.assetIdS = tp.AssetIdS)
        INNER JOIN dbo.TB_Cliente AS tc WITH (NOLOCK)
            ON (tc.clienteIdS = ta.clienteIdS)
        INNER JOIN dbo.TB_Site AS ts WITH (NOLOCK)
            ON (ts.siteIdS = ta.siteIdS)
        INNER JOIN dbo.TB_ClienteNotificacionSites AS NS
            ON (NS.SiteIds = ts.siteIdS)
        INNER JOIN dbo.TB_Estados AS te WITH (NOLOCK)
            ON (te.estadoIdS = ta.estadoSyscafIdS)
    WHERE ta.estadoClienteIdS = 1
          AND
          (
              @ListaClienteNotifacionId IS NULL
              OR NS.ListaClienteNotifacionId = @ListaClienteNotifacionId
          )
          AND tc.clienteIdS NOT IN ( 856, 842 )
    ORDER BY tc.clienteNombre,
             ts.siteName,
             tp.diffAVL;

    SELECT Id,
           assetCodigo,
           registrationNumber,
           assetsDescription,
           diffAVL,
           AVL,
           REPLACE(
                      REPLACE(REPLACE(REPLACE(clienteNombre, 'Col FV Itaú ', ''), 'Col FV Syscaf ', ''), 'Col FV ', ''),
                      'Itaú ',
                      ''
                  ) AS clienteNombre,
           clienteNombre Cliente,
           clienteIdS,
           Sitio,
           estado
    FROM @TABLE
    WHERE (
              @clienteIdS IS NULL
              OR clienteIdS = @clienteIdS
          );


END;