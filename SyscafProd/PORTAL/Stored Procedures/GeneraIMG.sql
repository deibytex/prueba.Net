
--SET QUOTED_IDENTIFIER ON|OFF
--SET ANSI_NULLS ON|OFF
--GO
-- ygonzalez 31.08.2021  rellena IMG 
CREATE PROCEDURE PORTAL.GeneraIMG
(@Periodo VARCHAR(10))
AS
BEGIN

    -- eliminamos datos historicos

    --DELETE FROM dbo.TB_ErroresViajesyUso
    --WHERE EXISTS
    --(
    --    SELECT *
    --    FROM dbo.TB_ErroresViajesyUso AS TEVU
    --    WHERE TEVU.TripStart < DATEADD(MONTH, -2, GETDATE())
    --);

    DECLARE @cmd INT;
    DECLARE cmds CURSOR FOR
    SELECT clienteIdS
    FROM TB_Cliente
    WHERE GeneraIMG = 1
          AND clienteIdS <> 828;

    OPEN cmds;
    WHILE 1 = 1
    BEGIN
               FETCH cmds
        INTO @cmd;
        IF @@fetch_status != 0
            BREAK;
        EXEC dbo.SP_RellenoErroresViajesyUso -- datetime
            @Periodo = @Periodo, -- varchar(10)
            @clienteIdS = @cmd;

    END;
    CLOSE cmds;
    DEALLOCATE cmds;


END;

