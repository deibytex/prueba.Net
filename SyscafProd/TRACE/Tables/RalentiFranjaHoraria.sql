CREATE TABLE [TRACE].[RalentiFranjaHoraria] (
    [Id]           INT      IDENTITY (1, 1) NOT NULL,
    [Fecha]        DATETIME NOT NULL,
    [AssetIds]     INT      NOT NULL,
    [Hour]         INT      NOT NULL,
    [TotalRalenti] INT      NOT NULL,
    [FechaSistema] DATETIME NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

