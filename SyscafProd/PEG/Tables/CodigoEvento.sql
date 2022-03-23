CREATE TABLE [PEG].[CodigoEvento] (
    [Id]          INT           NOT NULL,
    [EventTypeId] BIGINT        NOT NULL,
    [Description] VARCHAR (100) NULL,
    [ClienteIds]  INT           NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

