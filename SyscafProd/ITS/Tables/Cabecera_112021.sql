CREATE TABLE [ITS].[Cabecera_112021] (
    [versionTrama]         VARCHAR (20)  NULL,
    [idRegistro]           INT           NOT NULL,
    [idOperador]           VARCHAR (50)  NULL,
    [idVehiculo]           VARCHAR (20)  NULL,
    [idRuta]               VARCHAR (50)  NULL,
    [idConductor]          VARCHAR (200) NULL,
    [fechaHoraLecturaDato] VARCHAR (50)  NULL,
    [fechaHoraEnvioDato]   VARCHAR (50)  NULL,
    [tipoBus]              VARCHAR (10)  NULL,
    [latitud]              FLOAT (53)    NULL,
    [longitud]             FLOAT (53)    NULL,
    [tipoTrama]            INT           NULL,
    [tecnologiaMotor]      INT           NULL,
    [tramaRetransmitida]   BIT           NULL,
    [tipoFreno]            INT           NULL,
    [codigoPeriodica]      VARCHAR (10)  NULL,
    [Esprocesado]          BIT           DEFAULT ((0)) NOT NULL,
    [fechasistema]         DATETIME      NOT NULL,
    PRIMARY KEY CLUSTERED ([idRegistro] ASC)
);

