  --==================================================================================
  -- ygonzalez 08/10/2020
  -- elimina los registro duplicados despues de traer las posiciones
  --==================================================================================

CREATE PROCEDURE [dbo].[SP_DeleteDuplicatePosition]                       
AS                                        
BEGIN                                             
    --==================================================================================
	-- Se borran los duplicados
	--==================================================================================
	DELETE FROM TB_Positions WHERE TB_Positions.PositionIdS 
	IN(
		SELECT 
			MIN(tp.PositionIdS) as PositionIdS
		FROM 
			TB_Positions tp
		GROUP BY 
			tp.AssetId,
	CONVERT(VARCHAR(10),tp.Timestamp, 111)
	HAVING COUNT(*)>1)
	 --==================================================================================
	-- vehiculos que ya transmite y deben cambiar estado activo
	--==================================================================================
	UPDATE 
		TA 
	SET 
		TA.estadoSyscafIdS=3
	FROM 
		dbo.TB_Assets TA
			INNER JOIN 
		dbo.TB_Estados TE 
				ON Te.estadoIdS = Ta.estadoSyscafIdS
	WHERE 
		TA.estadoClienteIdS = 1 
			AND  
		TE.tipoIds = 3 
			AND 
		TA.assetIdS NOT IN (SELECT assetIdS FROM VW_VehiculosSinTransmision vvst )

	--==================================================================================
	-- vehiculos que estan activos y pasaron sin tx
	--==================================================================================

	UPDATE
		dbo.TB_Assets
	SET
		estadoSyscafIdS=8
	WHERE 
		assetIdS IN (SELECT assetIdS FROM VW_VehiculosSinTransmision )
			AND 
		estadoSyscafIdS = 3;
END