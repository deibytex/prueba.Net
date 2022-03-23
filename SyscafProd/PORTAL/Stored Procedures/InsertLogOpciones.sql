-- =============================================
-- Author:      ygonzalez
-- Create Date: 2021.11.22
-- Description: guarla las interraciones dentro de la plataforma por el momento nombre opcion
-- =============================================
CREATE PROCEDURE PORTAL.InsertLogOpciones
(
    @UsuarioIds INT,
    @OpcionId INT,
    @Nombre VARCHAR(100),
    @Date DATETIME
)
AS
BEGIN

    INSERT INTO PORTAL.LogUsuarioOpcion
    (
        Usuarioid,
        Date,
        OpcionId,
        Opcion
    )
    VALUES
    (   @UsuarioIds, -- Usuarioid - int
        @Date,       -- Date - datetime
        @OpcionId,   -- OpcionId - int
        @Nombre      -- Opcion - varchar(100)
        );
END;
