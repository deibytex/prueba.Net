-- author = yulibeth gonzalez
-- crea las tablas divididas por meses segun configuracion
-- en la tabla de configuracion  para cada caso en especifico
CREATE PROCEDURE ITS.CreateTableIts @Periodo NVARCHAR(6)
AS
BEGIN


    DECLARE @CabeceraCampos NVARCHAR(200);

    -- validamossi la cabecera existe

    SELECT @CabeceraCampos = CT.Campos
    FROM ITS.ConfiguracionTablas AS CT
    WHERE CT.TipoTrama IS NULL;

    DECLARE @CabeceraNombre NVARCHAR(1000);

    DECLARE @cmd VARCHAR(4000),
            @Name NVARCHAR(100),
            @CamposAdicionales NVARCHAR(4000);

	-- declaramos el curso
    DECLARE cmds CURSOR FOR
    SELECT CT.Codigo,
           CT.Campos,
           CT.CamposAdicionales
    FROM ITS.ConfiguracionTablas AS CT
    WHERE CT.EsCabDetalle = 1;

    OPEN cmds;
    WHILE 1 = 1
    BEGIN
        FETCH cmds
        INTO @Name,
             @cmd,
             @CamposAdicionales;
        IF @@fetch_status != 0
            BREAK;

        SET @Name = @Name + N'_' + @Periodo;
        SET @CabeceraNombre
            = N' --drop table ITS.'+@Name+'
			IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N''ITS.' + @Name
              + N''') AND type in (N''U''))
 CREATE TABLE ITS.'           + @Name + N' ( 
 '                            + @cmd + ISNULL(@CamposAdicionales, '')
              + N',
 Esprocesado bit not null default(0) , 
 fechasistema datetime not null)';

        EXEC sp_executesql @CabeceraNombre;
    END;
    CLOSE cmds;
    DEALLOCATE cmds;
END;
