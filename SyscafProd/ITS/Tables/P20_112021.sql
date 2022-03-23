CREATE TABLE [ITS].[P20_112021] (
    [idRegistro]          INT             NOT NULL,
    [velocidadVehiculo]   DECIMAL (18, 3) NULL,
    [aceleracionVehiculo] DECIMAL (18, 3) NULL,
    [codigoPeriodica]     VARCHAR (10)    NULL,
    [tipoTrama]           INT             NULL,
    [Esprocesado]         BIT             DEFAULT ((0)) NOT NULL,
    [fechasistema]        DATETIME        NOT NULL,
    PRIMARY KEY CLUSTERED ([idRegistro] ASC)
);

