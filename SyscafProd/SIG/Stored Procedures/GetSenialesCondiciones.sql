-- =============================================
-- Author:      MDiaz
-- Create Date: 09.12.2021
-- Description: Trae la información de las señales y las ordenas por señal id
-- =============================================
CREATE PROCEDURE SIG.GetSenialesCondiciones
(@ClienteIds VARCHAR(MAX))
AS
BEGIN
    SELECT TFS.Nombre,
           TCT.CondicionId,
           TCT.EsActivo,
           TCT.EventTypeId,
           TCT.Distancia,
           TCT.Tiempo,
           TCT.TipoCondicionId,
           TCT.ValorTrips,
           TCT.FechaSistema,
           TCT.Ocurrencias,
           TCT.TipoValorId,
           TCT.Valor,
           TFS.FallaSenialId,
           TCT.Descripcion,
           TCT.Clienteids,
           TCT.CondRef,
           TipoValorNombre = DL.Nombre
    FROM SIG.TB_Condiciones AS TCT
        INNER JOIN SIG.TB_FallaSenial AS TFS
            ON TFS.FallaSenialId = TCT.FallaSenialId
        INNER JOIN dbo.TB_DetalleListas AS DL
            ON TCT.TipoValorId = DL.DetalleListaId
    WHERE (
              TCT.Clienteids IS NULL
              OR TCT.Clienteids LIKE '%' + @ClienteIds + '%'
          )
          AND TCT.CondicionId NOT IN
              (
                  SELECT CondRef
                  FROM SIG.TB_Condiciones AS TC
                  WHERE TC.Clienteids LIKE '%' + @ClienteIds + '%'
                        AND CondRef IS NOT NULL
              )
    ORDER BY TCT.FallaSenialId;
END;