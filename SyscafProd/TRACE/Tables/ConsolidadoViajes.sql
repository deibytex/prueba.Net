CREATE TABLE [TRACE].[ConsolidadoViajes] (
    [Id]                 INT             IDENTITY (1, 1) NOT NULL,
    [Fecha]              DATETIME        NOT NULL,
    [AssetIds]           INT             NOT NULL,
    [distanceKilometers] DECIMAL (11, 4) NOT NULL,
    [FuelUsedLitres]     DECIMAL (11, 4) NOT NULL,
    [EngineSeconds]      INT             NOT NULL,
    [FechaSistema]       DATETIME        NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

