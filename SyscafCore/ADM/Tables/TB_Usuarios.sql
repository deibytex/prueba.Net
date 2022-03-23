CREATE TABLE [ADM].[TB_Usuarios] (
    [usuarioIdS]               INT              IDENTITY (1, 1) NOT NULL,
    [nombre]                   VARCHAR (100)    NOT NULL,
    [apellido]                 VARCHAR (100)    NOT NULL,
    [telefono]                 VARCHAR (20)     NOT NULL,
    [documento]                VARCHAR (20)     NOT NULL,
    [correo]                   VARCHAR (100)    NOT NULL,
    [key]                      VARBINARY (MAX)  NOT NULL,
    [IV]                       VARBINARY (MAX)  NOT NULL,
    [usuario]                  VARCHAR (50)     NOT NULL,
    [fechaCreacion]            DATETIME         NOT NULL,
    [fechaUltimaActualizacion] DATETIME         NOT NULL,
    [fechaUltimoIngreso]       DATETIME         NOT NULL,
    [perfilIdS]                INT              NOT NULL,
    [estadoUsuarioIdS]         INT              NOT NULL,
    [contrasena]               VARBINARY (MAX)  NOT NULL,
    [notificacion]             BIT              NULL,
    [imagen]                   VARBINARY (MAX)  NULL,
    [TokenRecuperacion]        UNIQUEIDENTIFIER NULL,
    [FechaExpiracion]          DATETIME         NULL,
    [Intentos]                 INT              NULL,
    CONSTRAINT [PK_TB_Usuarios] PRIMARY KEY CLUSTERED ([usuarioIdS] ASC)
);

