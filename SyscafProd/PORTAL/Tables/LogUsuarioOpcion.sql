CREATE TABLE [PORTAL].[LogUsuarioOpcion] (
    [LogId]     INT           IDENTITY (1, 1) NOT NULL,
    [Usuarioid] INT           NOT NULL,
    [Date]      DATETIME      NOT NULL,
    [OpcionId]  INT           NOT NULL,
    [Opcion]    VARCHAR (100) NOT NULL,
    PRIMARY KEY CLUSTERED ([LogId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [idx_LogUsuarioOpcion]
    ON [PORTAL].[LogUsuarioOpcion]([Usuarioid] ASC)
    INCLUDE([Date], [OpcionId], [Opcion]);

