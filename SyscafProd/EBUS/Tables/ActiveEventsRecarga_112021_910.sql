CREATE TABLE [EBUS].[ActiveEventsRecarga_112021_910] (
    [EventId]      BIGINT          NOT NULL,
    [FechaHora]    DATETIME        NOT NULL,
    [EventTypeID]  BIGINT          NULL,
    [Consecutivo]  INT             NULL,
    [Carga]        INT             NULL,
    [AssetId]      BIGINT          NOT NULL,
    [DriverId]     BIGINT          NOT NULL,
    [Soc]          DECIMAL (18, 4) NULL,
    [Corriente]    DECIMAL (18, 4) NULL,
    [Voltaje]      DECIMAL (18, 4) NULL,
    [Potencia]     DECIMAL (18, 4) NULL,
    [Energia]      DECIMAL (18, 4) NULL,
    [ETA]          DECIMAL (18, 4) NULL,
    [Odometer]     DECIMAL (18, 2) NULL,
    [EsProcesado]  BIT             NOT NULL,
    [fechasistema] DATETIME        NOT NULL,
    PRIMARY KEY CLUSTERED ([EventId] ASC)
);

