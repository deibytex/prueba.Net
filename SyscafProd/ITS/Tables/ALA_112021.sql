CREATE TABLE [ITS].[ALA_112021] (
    [idRegistro]              INT             NOT NULL,
    [codigoAlarma]            VARCHAR (10)    NULL,
    [nivelAlarma]             VARCHAR (10)    NULL,
    [aceleracionVehiculo]     DECIMAL (18, 3) NULL,
    [velocidadVehiculo]       DECIMAL (18, 3) NULL,
    [peso]                    DECIMAL (18, 3) NULL,
    [tipoTrama]               INT             NULL,
    [codigoCamara]            VARCHAR (100)   NULL,
    [estadoCinturonSeguridad] BIT             NULL,
    [estadoInfoentertaiment]  BIT             NULL,
    [estadoDesgasteFrenos]    DECIMAL (18, 2) NULL,
    [Esprocesado]             BIT             DEFAULT ((0)) NOT NULL,
    [fechasistema]            DATETIME        NOT NULL,
    PRIMARY KEY CLUSTERED ([idRegistro] ASC)
);

