CREATE TABLE [TRACE].[UltimoOdometro] (
    [Id]             INT             IDENTITY (1, 1) NOT NULL,
    [Fecha]          DATETIME        NOT NULL,
    [AssetIds]       INT             NOT NULL,
    [MaximoOdometro] DECIMAL (11, 4) NULL,
    [FechaSistema]   DATETIME        NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

