
-- =============================================
-- Author:      ygonzalez
-- Create Date: 2022.01.25
-- Description: actualiza el estado de los vehiculos sin TX en el momento de extraccion de las posiciones
-- =============================================

CREATE PROCEDURE dbo.TX_CambiaEstadoVehiculosSinTX 
@FechaActual DATETIME
AS
BEGIN

    DECLARE @VehiculosSinTX TABLE
    (
        AssetIdS INT NOT NULL,
        diffAVL INT NOT NULL,
        AVL DATETIME NOT NULL
    );      
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



END;
