CREATE TABLE [PORTAL].[LogUsuarios] (
    [LogId]     INT      IDENTITY (1, 1) NOT NULL,
    [Usuarioid] INT      NOT NULL,
    [StartDate] DATETIME NOT NULL,
    [EndDate]   DATETIME NULL,
    PRIMARY KEY CLUSTERED ([LogId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [idx_logusuario]
    ON [PORTAL].[LogUsuarios]([Usuarioid] ASC)
    INCLUDE([StartDate], [EndDate]);

