CREATE PROCEDURE [dbo].[SP_TransmisionVehiculosTorta3p]
    @usuarioIdS INT = NULL,
    @clienteIdS INT = NULL
AS
BEGIN
    DECLARE @TABLE TABLE
    (
        identificador INT IDENTITY(1, 1) NOT NULL,
        assetId VARCHAR(50) NULL,
        usuarioIdS INT NULL,
        diffAVL INT NULL
    );


	--===============================================================================================================
    -- ygonzalez 17/09/2020
    -- se quitaron las relaciones de las tablas  TB_Usuarios ,TB_Estados 
    -- se quitó agrupación de estados que no se estaba usando 
    -- se actualizó el where para que evalue de una manera diferente la condicion de clientes y usuarios
	--===============================================================================================================
    INSERT INTO @TABLE
    SELECT DISTINCT
           ta.assetId,
           tg.usuarioIdS,
           tp.DiasSinTx
    FROM 
		dbo.TB_Assets AS ta
			INNER JOIN  
		VW_VehiculosSinTransmision AS tp
				 ON ta.assetIdS = tp.assetIdS
			INNER JOIN
		TB_Site AS ts
            ON ts.siteIdS = ta.siteIdS
			INNER JOIN
		(
			 SELECT
				*
			 FROM
				VW_GruposSeguridad tgi
			 WHERE
				(@usuarioIdS IS NULL OR tgi.usuarioIdS = @usuarioIdS)
		 ) AS tg
		    	ON (tg.clienteIdS = ta.clienteIdS)
    WHERE 
		ta.estadoClienteIdS = 1
          AND
        (
            @clienteIdS IS NULL
            OR ta.clienteIdS = @clienteIdS
        )
			AND 
		ts.tipoSitio <> 9


    SELECT 
		'Mas de 200' AS ultimoViaje,
        COUNT(assetId) AS total
    FROM
		@TABLE
    WHERE 
	(
        diffAVL >= 200
        OR diffAVL IS NULL
    )
    UNION ALL
    SELECT 
		'Entre 100 y 200' AS ultimoViaje,
        COUNT(assetId) AS total
    FROM 
		@TABLE
    WHERE 
		diffAVL >= 100
          AND 
		diffAVL < 200
    UNION ALL
    SELECT 
		'Entre 100 y 50' AS ultimoViaje,
        COUNT(assetId) AS total
    FROM 
		@TABLE
    WHERE 
		diffAVL >= 50
		   AND 
		diffAVL < 100
    UNION ALL
    SELECT 
		'Menos de 50' AS ultimoViaje,
        COUNT(assetId) AS total
    FROM 
		@TABLE
    WHERE 
		diffAVL >= 0
          AND 
		diffAVL < 50;
END;