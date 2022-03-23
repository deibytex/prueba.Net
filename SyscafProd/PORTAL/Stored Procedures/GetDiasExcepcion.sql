-- =============================================
-- Author:      ygonzalez
-- Create Date: 2022.01.24
-- Description: Trae los dias exceptuados para cualquier evento que queriea, normalemnte son los festivos
-- =============================================
CREATE PROCEDURE PORTAL.GetDiasExcepcion
(
    @TipoExcepcionId AS INT,
    @Date DATE
)
AS
BEGIN

    SELECT TDE.Dia
    FROM dbo.TB_DiasExcepciones AS TDE
    WHERE TDE.Dia = @Date
          AND TDE.TipoExcepcionId = @TipoExcepcionId;
END;
