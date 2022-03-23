
-- Eliminamos los eventos por un rango de fechas
CREATE PROCEDURE SP_DeleteSubviajesByFecha
(
    @FechaInicio DATETIME,
    @FechaFinal DATETIME
)
AS
BEGIN

    BEGIN TRY

        BEGIN TRANSACTION;

        DELETE FROM dbo.TB_SubViaje
        WHERE TripStart
        BETWEEN @FechaInicio AND @FechaFinal;

        COMMIT TRANSACTION;


    END TRY
    BEGIN CATCH

        ROLLBACK TRANSACTION;

    END CATCH;


END;