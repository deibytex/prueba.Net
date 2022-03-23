CREATE TABLE [PORTAL].[TotalEventos_895] (
    [TotalEventosId] INT             IDENTITY (1, 1) NOT NULL,
    [FechaHora]      DATETIME        NOT NULL,
    [AssetId]        BIGINT          NOT NULL,
    [DriverId]       BIGINT          NOT NULL,
    [EventTypeId]    BIGINT          NOT NULL,
    [Ocurrencias]    INT             NOT NULL,
    [Tiempo]         INT             NOT NULL,
    [Distancia]      DECIMAL (18, 1) NULL,
    [FechaSistema]   DATETIME        NOT NULL,
    PRIMARY KEY CLUSTERED ([TotalEventosId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [portal_TotalEventos_895]
    ON [PORTAL].[TotalEventos_895]([FechaHora] ASC)
    INCLUDE([AssetId], [DriverId], [EventTypeId]);

