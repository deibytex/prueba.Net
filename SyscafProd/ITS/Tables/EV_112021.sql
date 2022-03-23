CREATE TABLE [ITS].[EV_112021] (
    [idRegistro]                    INT             NOT NULL,
    [codigoEvento]                  VARCHAR (10)    NULL,
    [fotoConductor]                 VARCHAR (MAX)   NULL,
    [codigoComportamientoAnomalo]   INT             NULL,
    [estadoAperturaCierrePuertas]   BIT             NULL,
    [porcentajeCargaBaterias]       INT             NULL,
    [estadoSistemaVentilacion]      BIT             NULL,
    [estadoSistemaIluminacion]      BIT             NULL,
    [estadoSistemaLimpiaParabrisas] BIT             NULL,
    [tipoTrama]                     INT             NULL,
    [peso]                          DECIMAL (18, 2) NULL,
    [estimacionOcupacionSuben]      INT             NULL,
    [estimacionOcupacionBajan]      INT             NULL,
    [estimacionOcupacionAbordo]     INT             NULL,
    [Esprocesado]                   BIT             DEFAULT ((0)) NOT NULL,
    [fechasistema]                  DATETIME        NOT NULL,
    PRIMARY KEY CLUSTERED ([idRegistro] ASC)
);

