CREATE TABLE [TRACE].[Ralenti] (
    [Id]                INT             IDENTITY (1, 1) NOT NULL,
    [Fecha]             DATETIME        NOT NULL,
    [AssetIds]          INT             NOT NULL,
    [CombustibleEvento] DECIMAL (11, 4) NOT NULL,
    [CombustibleViaje]  DECIMAL (11, 4) NOT NULL,
    [FechaSistema]      DATETIME        NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

