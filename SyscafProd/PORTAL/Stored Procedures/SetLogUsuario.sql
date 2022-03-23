-- =============================================
-- Author:      ygonzalez
-- Create Date: 2021.11.22
-- Description: Guarda la informacion de inicio y finalizacion de session
-- =============================================
CREATE
PROCEDURE PORTAL.SetLogUsuario
(
    @UsuarioId INT,
    @Fecha DATETIME,
    @Login BIT
)
AS
BEGIN


    IF @Login = 1
    BEGIN
        INSERT INTO PORTAL.LogUsuarios
        (
            Usuarioid,
            StartDate
        )
        VALUES
        (   @UsuarioId, -- Usuarioid - int
            @Fecha      -- EndDate - datetime
            );
    END;
    ELSE
    BEGIN

		--actualiza la ultima fecha final que encuentre cuando se dispara el session login
		-- o cierra por cerrar session
        UPDATE PORTAL.LogUsuarios
        SET EndDate = @Fecha
        WHERE LogId =
        (
            SELECT TOP 1
                   LU.LogId
            FROM PORTAL.LogUsuarios AS LU
            WHERE LU.Usuarioid = @UsuarioId
                  AND LU.EndDate IS NULL
            ORDER BY LU.StartDate DESC
        );

    END;


END;
