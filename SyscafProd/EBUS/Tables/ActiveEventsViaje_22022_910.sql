CREATE TABLE [EBUS].[ActiveEventsViaje_22022_910] (
    [EventId]           BIGINT          NOT NULL,
    [FechaHora]         DATETIME        NOT NULL,
    [EventTypeID]       BIGINT          NULL,
    [AssetId]           BIGINT          NOT NULL,
    [DriverId]          BIGINT          NOT NULL,
    [Altitud]           DECIMAL (18, 4) NULL,
    [EnergiaRegenerada] DECIMAL (18, 4) NULL,
    [EnergiaDescargada] DECIMAL (18, 4) NULL,
    [Soc]               DECIMAL (18, 4) NULL,
    [Energia]           DECIMAL (18, 4) NULL,
    [PorRegeneracion]   DECIMAL (18, 4) NULL,
    [Distancia]         DECIMAL (18, 4) NULL,
    [Localizacion]      VARCHAR (250)   NULL,
    [Latitud]           FLOAT (53)      NULL,
    [Longitud]          FLOAT (53)      NULL,
    [Autonomia]         DECIMAL (18, 4) NULL,
    [VelocidadPromedio] DECIMAL (18, 4) NULL,
    [EsProcesado]       BIT             NOT NULL,
    [fechasistema]      DATETIME        NOT NULL,
    PRIMARY KEY CLUSTERED ([EventId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [FechaHoraEvento_ActiveEventsViaje_22022_910]
    ON [EBUS].[ActiveEventsViaje_22022_910]([FechaHora] ASC, [EventTypeID] ASC);


GO
CREATE NONCLUSTERED INDEX [FechaHoraEvento_ActiveEventsRecarga_22022_910]
    ON [EBUS].[ActiveEventsViaje_22022_910]([FechaHora] ASC, [EventTypeID] ASC);

