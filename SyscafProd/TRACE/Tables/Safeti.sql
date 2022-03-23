CREATE TABLE [TRACE].[Safeti] (
    [Id]                 INT             IDENTITY (1, 1) NOT NULL,
    [Fecha]              DATETIME        NOT NULL,
    [AssetIds]           INT             NOT NULL,
    [TotalTimeSeconds]   INT             NOT NULL,
    [distanceKilometers] DECIMAL (11, 4) NOT NULL,
    [TotalOccurances]    INT             NOT NULL,
    [EventTypeId]        BIGINT          NOT NULL,
    [FechaSistema]       DATETIME        NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

