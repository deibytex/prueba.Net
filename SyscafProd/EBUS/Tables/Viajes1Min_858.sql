CREATE TABLE [EBUS].[Viajes1Min_858] (
    [Viajes1MinId] INT             IDENTITY (1, 1) NOT NULL,
    [Movil]        VARCHAR (MAX)   NOT NULL,
    [Operador]     VARCHAR (MAX)   NULL,
    [Fecha]        DATE            NOT NULL,
    [Hora]         TIME (7)        NOT NULL,
    [FechaHora]    DATETIME        NOT NULL,
    [SOC]          INT             NOT NULL,
    [CargakWh]     DECIMAL (18, 4) NULL,
    [DescargakWh]  DECIMAL (18, 4) NULL,
    [Odometro]     DECIMAL (18, 4) NULL,
    [Distancia]    DECIMAL (18, 4) NULL,
    [RutaCodigo]   VARCHAR (50)    NULL,
    [RutaNombre]   VARCHAR (MAX)   NULL,
    [Latitud]      FLOAT (53)      NULL,
    [Longitud]     FLOAT (53)      NULL,
    [Altitud]      INT             NULL,
    [DeltaSOC]     INT             NULL,
    [fechasistema] DATETIME        NOT NULL,
    [EsProcesado]  BIT             NULL,
    [EventId]      BIGINT          NULL,
    PRIMARY KEY CLUSTERED ([Viajes1MinId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [idxViajes1Min_858fecha]
    ON [EBUS].[Viajes1Min_858]([Fecha] ASC);

