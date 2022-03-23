-- author: dLopez
-- date: 21.10.2021
-- description = actualiza las tablas para marcarlas como procesadas
CREATE PROCEDURE [EBUS].[SETTablesPBI]
(
    --@FechaInicial DATETIME,
    --@FechaFinal DATETIME,
    @Reporte VARCHAR(50),
    @clienteIdS INT,
    @ReporteIds VARCHAR(MAX)
)
AS
BEGIN

    -- DECLARE  @clienteIdS   INT = 858
    --                                                , @Reporte VARCHAR(10) = 'Eficiencia'
    --                                                , @Reporteids VARCHAR(MAX) = '1,2,3'
    --        set @FechaInicial = '20201029 00:00:00';
    --        set @FechaFinal = '20201029 12:00:00';

    -- borramos la informacion del usuaro
    --DELETE FROM dbo.TB_ErroresViajesyUso
    --  WHERE ClienteIdS=@clienteIdS
    --        AND TripStart >= @FechaInicial AND TripStart < @FechaFinal ;

    DECLARE @SQLScript NVARCHAR(MAX)
        = N'
         UPDATE [EBUS].[' + @Reporte + N'_' + CAST(@clienteIdS AS VARCHAR)
          + N']  SET EsProcesado = 1
		  WHERE ' +@Reporte + N'Id in (' + @ReporteIds + '
		  )';

    EXEC sp_executesql @SQLScript

    -- EXEC sp_executesql @SQLScript,
    --                    N'@Fi as datetime,  @ff as datetime',
    --                    @FechaInicial,
    --                    @FechaFinal;

END;
