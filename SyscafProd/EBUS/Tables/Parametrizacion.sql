CREATE TABLE [EBUS].[Parametrizacion] (
    [ParametrizacionId]   INT          IDENTITY (1, 1) NOT NULL,
    [TipoParametroId]     INT          NOT NULL,
    [ClienteIds]          INT          NOT NULL,
    [Valor]               VARCHAR (50) NOT NULL,
    [UsuarioId]           INT          NOT NULL,
    [UltimaActualizacion] DATETIME     NULL,
    [FechaSistema]        DATETIME     NOT NULL,
    [EsActivo]            BIT          NOT NULL,
    PRIMARY KEY CLUSTERED ([ParametrizacionId] ASC),
    CONSTRAINT [FK_Parametrizacion_TB_Cliente] FOREIGN KEY ([ClienteIds]) REFERENCES [dbo].[TB_Cliente] ([clienteIdS]),
    CONSTRAINT [FK_Parametrizacion_TB_DetalleListas] FOREIGN KEY ([TipoParametroId]) REFERENCES [dbo].[TB_DetalleListas] ([DetalleListaId]),
    CONSTRAINT [FK_Parametrizacion_TB_Usuarios] FOREIGN KEY ([UsuarioId]) REFERENCES [dbo].[TB_Usuarios] ([usuarioIdS])
);

