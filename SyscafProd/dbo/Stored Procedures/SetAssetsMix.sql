
-- =============================================
-- Author:     ygonzalez
-- Create Date: 28/05/2021
-- Description: guarda los vehiculos si no existen y si existen actualiza la iformacion
-- =============================================

CREATE PROCEDURE dbo.SetAssetsMix
(
    @ClienteIds INT,
    @vehiculos dbo.UDT_Assets READONLY
)
AS
BEGIN


    -- INSERTAMOS LOS DATOS NUEVOS
    INSERT INTO TB_Assets
    (
        assetId,
		assetIdf,
        groupId,
        createdDate,
        registrationNumber,
        assetsDescription,
        assetCodigo,
        siteIdS,
        estadoClienteIdS,
        estadoSyscafIdS,
        clienteIdS,
        FechaSistema
    )
    SELECT CAST(assetId  AS VARCHAR),
		   assetId,
           CAST(groupId  AS VARCHAR),
           createdDate,
           registrationNumber,
           assetsDescription,
           assetCodigo,
           siteIds,
           1,
           3,
           @ClienteIds,
           GETDATE()
    FROM @vehiculos
    WHERE assetId NOT IN
          (
              SELECT CAST(assetId AS FLOAT)
              FROM TB_Assets
              WHERE clienteIdS = @ClienteIds
          );

    -- ACTUALIZAMOS LOS VEHICULOS QUE  EXISTEN

    UPDATE a
    SET a.siteIdS = v.siteIds,
	    a.AssetIdF = v.assetId,
        a.registrationNumber = v.registrationNumber,
        a.assetsDescription = v.assetsDescription,
        a.assetCodigo = v.assetCodigo
    FROM @vehiculos AS v
        INNER JOIN TB_Assets AS a
            ON v.assetId = CAST(a.assetId AS FLOAT);


    -- DESACTIVAMOS LOS VEHICULOS QUE NO EXISTEN EN MIX INTEGRATE

    UPDATE a
    SET a.estadoClienteIdS = 0
    FROM TB_Assets AS a
        INNER JOIN TB_Site AS s
            ON a.siteIdS = s.siteIdS
    WHERE a.clienteIdS = @ClienteIds
          AND siteName = 'Zona Decom';


    UPDATE a
    SET a.estadoClienteIdS = 1
    FROM TB_Assets AS a
        INNER JOIN TB_Site AS s
            ON a.siteIdS = s.siteIdS
    WHERE a.clienteIdS = @ClienteIds
          AND siteName <> 'Zona Decom';


END;
