
-- Eliminamos los eventos por un rango de fechas
CREATE PROCEDURE DeleteEventsByDate
(
    @FechaInicio DATETIME,
    @FechaFinal DATETIME
)
AS
BEGIN

    BEGIN TRY

        BEGIN TRANSACTION;

        DELETE FROM dbo.TB_Event
        WHERE EventDateTime
        BETWEEN @FechaInicio AND @FechaFinal;


        COMMIT TRANSACTION;


    END TRY
    BEGIN CATCH

        ROLLBACK TRANSACTION;

    END CATCH;


END;