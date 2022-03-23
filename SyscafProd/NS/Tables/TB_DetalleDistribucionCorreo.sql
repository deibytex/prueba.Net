CREATE TABLE [NS].[TB_DetalleDistribucionCorreo] (
    [DetalleDistribucionId] INT           IDENTITY (1, 1) NOT NULL,
    [ListaDistribucionId]   INT           NOT NULL,
    [UsuarioIds]            INT           NULL,
    [Nombres]               VARCHAR (200) COLLATE Modern_Spanish_CI_AS NULL,
    [Correo]                VARCHAR (200) COLLATE Modern_Spanish_CI_AS NULL,
    [TipoEnvioId]           INT           NOT NULL,
    [FechaSistema]          DATETIME      CONSTRAINT [DF__TB_Detall__Fecha__05A3D694] DEFAULT (getdate()) NOT NULL,
    [EsActivo]              BIT           CONSTRAINT [DF__TB_Detall__EsAct__0697FACD] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK__TB_Detal__0ECE7388DB1622F2] PRIMARY KEY CLUSTERED ([DetalleDistribucionId] ASC),
    CONSTRAINT [Pk_TB_DetalleDistribucionCorreo_TB_DetalleListas] FOREIGN KEY ([TipoEnvioId]) REFERENCES [dbo].[TB_DetalleListas] ([DetalleListaId]),
    CONSTRAINT [Pk_TB_DetalleDistribucionCorreo_usuarios] FOREIGN KEY ([UsuarioIds]) REFERENCES [dbo].[TB_Usuarios] ([usuarioIdS]),
    CONSTRAINT [UQ__TB_Detal__4CCF6587F3F89B42] UNIQUE NONCLUSTERED ([ListaDistribucionId] ASC, [Correo] ASC)
);

