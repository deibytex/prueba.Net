CREATE TABLE [dbo].[TB_GrupoSeguridadUsuario] (
    [GruposSeguridadUsuarioId] INT      IDENTITY (1, 1) NOT NULL,
    [GrupoSeguridadId]         INT      NULL,
    [usuarioIdS]               INT      NOT NULL,
    [FechaSistema]             DATETIME NOT NULL,
    [EsActivo]                 BIT      DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([GruposSeguridadUsuarioId] ASC),
    CONSTRAINT [FK_TB_GrupoSeguridadUsuario_TB_GruposDeSeguridad] FOREIGN KEY ([GrupoSeguridadId]) REFERENCES [dbo].[TB_GruposDeSeguridad] ([GrupoSeguridadId]),
    CONSTRAINT [FK_TB_GrupoSeguridadUsuario_TB_Usuarios] FOREIGN KEY ([usuarioIdS]) REFERENCES [dbo].[TB_Usuarios] ([usuarioIdS])
);

