CREATE TABLE [EBUS].[ClientesUsuarios] (
    [ClienteUsuarioId] INT           IDENTITY (1, 1) NOT NULL,
    [UsuarioIdS]       INT           NOT NULL,
    [clienteIdS]       VARCHAR (300) NULL,
    [EsActivo]         BIT           NOT NULL,
    [FechaSistema]     DATETIME      NOT NULL,
    CONSTRAINT [PK_ClientesUsuarios] PRIMARY KEY CLUSTERED ([ClienteUsuarioId] ASC),
    CONSTRAINT [FK_ClientesUsuarios_TB_Usuarios] FOREIGN KEY ([UsuarioIdS]) REFERENCES [dbo].[TB_Usuarios] ([usuarioIdS])
);

