CREATE TABLE [PORTAL].[TotalEventos_834] (
    [TotalEventosId] INT             IDENTITY (1, 1) NOT NULL,
    [FechaHora]      DATETIME        NOT NULL,
    [AssetId]        BIGINT          NOT NULL,
    [DriverId]       BIGINT          NOT NULL,
    [EventTypeId]    BIGINT          NOT NULL,
    [Ocurrencias]    INT             NOT NULL,
    [Distancia]      DECIMAL (18, 1) NULL,
    [FechaSistema]   DATETIME        NOT NULL,
    [Tiempo]         INT             NULL,
    PRIMARY KEY CLUSTERED ([TotalEventosId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [portal_TotalEventos_834]
    ON [PORTAL].[TotalEventos_834]([FechaHora] ASC)
    INCLUDE([AssetId], [DriverId], [EventTypeId]);

