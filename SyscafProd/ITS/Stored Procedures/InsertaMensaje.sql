-- author = yulibeth gonzalez
-- 17.11.2021
-- ingresa y segrega por tipo de datos la información recibida por el 
-- evento pus/pub
CREATE PROCEDURE ITS.InsertaMensaje
(
    @jsondata NVARCHAR(MAX),
    @Periodo NVARCHAR(6),
    @FechaRecibido DATETIME,
    @ProfileData VARCHAR(1000)
)
AS
BEGIN
    -- verifica que existan las tablas y si no las crea
    EXEC ITS.CreateTableIts @Periodo; -- nvarchar(6)

    --DECLARE @Periodo NVARCHAR(MAX) = N'112021';
    DECLARE @CabeceraCampos NVARCHAR(MAX);
    -- validamossi la cabecera existe

    SELECT @CabeceraCampos = CT.Campos
    FROM ITS.ConfiguracionTablas AS CT
    WHERE CT.TipoTrama IS NULL;

    DECLARE @cmd NVARCHAR(MAX) = N'';

    DECLARE @NombreTabla VARCHAR(MAX),
            @Campos NVARCHAR(MAX);
    DECLARE @CamposInsert NVARCHAR(MAX),
            @trama INT;

    -- insertamos la informacion en la tabla de mensajes para tener log de los datos recibidos 
    -- antes de ser procesados
    SET @cmd
        = N'
	INSERT INTO ITS.Messages_' + @Periodo + N'
	(
	    FechaHora,	    Mensaje,	    ProfileData
	)
	VALUES
	(   @FechaRecibido,  @jsondata,     @ProfileData      );';

    EXEC sp_executesql @cmd,
                       N'@FechaRecibido datetime,@jsondata varchar(max),@ProfileData varchar(max) ',
                       @FechaRecibido,
                       @jsondata,
                       @ProfileData;

    -- declaramos el curso
    DECLARE cmds CURSOR FOR
    SELECT CT.Codigo,
           CT.Campos + ISNULL(CT.CamposAdicionales, ''),
           CT.TipoTrama
    FROM ITS.ConfiguracionTablas AS CT
    WHERE CT.EsCabDetalle = 1; -- trae la informacion de los campos juntos con los campos adicionales que sirven para validar informacion


    OPEN cmds;
    WHILE 1 = 1
    BEGIN
        FETCH cmds
        INTO @NombreTabla,
             @Campos,
             @trama;
        IF @@fetch_status != 0
            BREAK;


        -- traemos los campos a insertar  segun tipo
        -- cabecera y detalles que son las alarmas ,eventos , p20 , p60
        SELECT @CamposInsert
            = CAST(STRING_AGG(SUBSTRING(TRIM(Value), 0, CHARINDEX(' ', TRIM(Value))), ',') AS NVARCHAR(MAX))
        FROM dbo.Split(@Campos, ', ');

        -- eliminamos  los caracteres innecesarios para poder realizar la extracion 
        -- de informacion de la fuente json 
        SET @Campos = REPLACE(@Campos, '/*', '');
        SET @Campos = REPLACE(@Campos, '*/', '');
        SET @Campos = REPLACE(@Campos, 'Primary', '');
        SET @Campos = REPLACE(@Campos, 'Key', '');
        SET @Campos = REPLACE(@Campos, 'Not null', '');


        -- armamos el script de insert  con la tabla y abriendo la informacion json que viene del server
        SET @cmd = N'
	  INSERT INTO ITS.' + @NombreTabla + N'_' + @Periodo + N'
	  (
	     ' + @CamposInsert + N', fechasistema
	  )
	 SELECT
	 ' + @CamposInsert + N', getdate()
		FROM
			OPENJSON(''' + @jsondata + N''', ''$.data'')
			WITH
			(
			   ' + @Campos + N'
			)	
	'   ;
        -- si la trama es nula significa que toda la informacion es de la cabecera
        -- de lo contrario filtrara por trama para que se distribuya correctamente 
        -- en las tablas destinadas para dicho fin  Cabecera, P20, P60, EV, ALA
        IF (@trama IS NOT NULL)
        BEGIN
            SET @cmd = @cmd + N' Where tipoTrama = ' + CAST(@trama AS NVARCHAR(MAX));
            IF (@trama = 1) -- si la trama es 1 significa que debera filtrarse por periocidad P20, P60
                SET @cmd = @cmd + N' and codigoPeriodica = ''' + @NombreTabla + N'''';
        END;


        EXEC sp_executesql @cmd;
    END;
    CLOSE cmds;
    DEALLOCATE cmds;

END;
