CREATE TABLE [EBUS].[RecargasActiveHistorical_914] (
    [EventId]            BIGINT          NOT NULL,
    [AssetId]            BIGINT          NOT NULL,
    [Movil]              VARCHAR (MAX)   NOT NULL,
    [FechaInicioRecarga] DATE            NOT NULL,
    [FechaHoraRecarga]   DATETIME        NOT NULL,
    [SOC]                INT             NULL,
    [Corriente]          DECIMAL (18, 4) NULL,
    [Voltaje]            INT             NULL,
    [SOCInicial]         INT             NULL,
    [Energia]            DECIMAL (18, 4) NULL,
    [TotalTime]          TIME (7)        NOT NULL,
    [Potencia]           DECIMAL (18, 4) NULL,
    [PotenciaPromedio]   DECIMAL (18, 4) NULL,
    [IsDisconected]      BIT             NOT NULL,
    [fechasistema]       DATETIME        NOT NULL,
    [EsProcesado]        BIT             NULL,
    CONSTRAINT [PK__RecargasActivo__92B70FF1B081207D] PRIMARY KEY CLUSTERED ([EventId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [idxRecargasActiveHistorical_914event]
    ON [EBUS].[RecargasActiveHistorical_914]([AssetId] ASC)
    INCLUDE([FechaHoraRecarga], [EsProcesado]);


GO
CREATE NONCLUSTERED INDEX [idxRecargasActiveHistorical_914fecha]
    ON [EBUS].[RecargasActiveHistorical_914]([FechaHoraRecarga] ASC)
    INCLUDE([AssetId], [Movil], [EventId], [EsProcesado]);

