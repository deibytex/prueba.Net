CREATE TABLE [PORTAL].[LogEndPoindMix] (
    [Id]              BIGINT        IDENTITY (1, 1) NOT NULL,
    [CrendencialesId] INT           NOT NULL,
    [Method]          VARCHAR (200) NOT NULL,
    [StatusResponse]  INT           NOT NULL,
    [Response]        VARCHAR (MAX) NULL,
    [FechaSistema]    DATETIME      NOT NULL,
    [ClienteIds]      INT           NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([CrendencialesId]) REFERENCES [PORTAL].[CredencialesMix] ([Id])
);

