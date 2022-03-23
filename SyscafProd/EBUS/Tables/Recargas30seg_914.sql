CREATE TABLE [EBUS].[Recargas30seg_914] (
    [Recargas30segId] INT             IDENTITY (1, 1) NOT NULL,
    [Muestra]         INT             NOT NULL,
    [NoCarga]         INT             NOT NULL,
    [AssetId]         BIGINT          NOT NULL,
    [Movil]           VARCHAR (MAX)   NOT NULL,
    [Fecha]           DATE            NOT NULL,
    [Hora]            TIME (7)        NOT NULL,
    [FechaHora]       DATETIME        NOT NULL,
    [SOC]             INT             NULL,
    [Corriente]       DECIMAL (18, 4) NULL,
    [Voltaje]         INT             NULL,
    [fechasistema]    DATETIME        NOT NULL,
    [EsProcesado]     BIT             NULL,
    [EventId]         BIGINT          NULL,
    PRIMARY KEY CLUSTERED ([Recargas30segId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [idxRecargas30seg_914event]
    ON [EBUS].[Recargas30seg_914]([Recargas30segId] ASC)
    INCLUDE([AssetId]);


GO
CREATE NONCLUSTERED INDEX [idxRecargas30seg_914fecha]
    ON [EBUS].[Recargas30seg_914]([Fecha] ASC)
    INCLUDE([AssetId], [Movil], [FechaHora], [EventId]);

