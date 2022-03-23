CREATE TABLE [FIDELIZACION].[ConfiguracionGerencia] (
    [ConfiguracionGerenciaId] INT           IDENTITY (1, 1) NOT NULL,
    [usuarioIdS]              INT           NOT NULL,
    [GerenciaCondId]          VARCHAR (MAX) NULL,
    [GerenciaVehId]           VARCHAR (MAX) NULL,
    [FechaSistema]            DATETIME      NOT NULL,
    [EsActivo]                BIT           NOT NULL,
    PRIMARY KEY CLUSTERED ([ConfiguracionGerenciaId] ASC),
    CONSTRAINT [FK_ConfiguracionGerencia_TB_Usuarios] FOREIGN KEY ([usuarioIdS]) REFERENCES [dbo].[TB_Usuarios] ([usuarioIdS])
);

