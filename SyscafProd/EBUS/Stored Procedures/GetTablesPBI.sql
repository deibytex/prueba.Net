-- author: dLopez
-- date: 21.10.2021
-- obtine la informacion para ser procesadas en powerbi
CREATE PROCEDURE [EBUS].[GetTablesPBI]
(
    --@FechaInicial DATETIME,
    --@FechaFinal DATETIME,
    @Reporte VARCHAR(50),
    @clienteIdS INT
)
AS
BEGIN

    -- DECLARE  @clienteIdS   INT = 858
    --                                                , @Reporte VARCHAR(10) = 'Eficiencia'
    --        set @FechaInicial = '20201029 00:00:00';
    --        set @FechaFinal = '20201029 12:00:00';



    DECLARE @SQLScript NVARCHAR(MAX)
        = N'
         SELECT TOP 3000 * FROM [EBUS].[' + @Reporte + N'_' + CAST(@clienteIdS AS VARCHAR)
          + N'] WHERE EsProcesado = 0';


    EXEC sp_executesql @SQLScript
;

END;
