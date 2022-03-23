CREATE TABLE [TRACE].[VelocidadOperacion] (
    [Id]               INT      IDENTITY (1, 1) NOT NULL,
    [Fecha]            DATETIME NOT NULL,
    [AssetIds]         INT      NOT NULL,
    [TotalTimeSeconds] INT      NOT NULL,
    [EventTypeId]      BIGINT   NOT NULL,
    [Tipo]             INT      NOT NULL,
    [FechaSistema]     DATETIME NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

