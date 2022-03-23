CREATE TABLE [EBUS].[Alarmas_858] (
    [AlarmasId]    INT             IDENTITY (1, 1) NOT NULL,
    [Movil]        VARCHAR (MAX)   NOT NULL,
    [Operador]     VARCHAR (MAX)   NULL,
    [Fecha]        DATE            NOT NULL,
    [Inicio]       DATETIME        NOT NULL,
    [Fin]          DATETIME        NOT NULL,
    [Descripcion]  VARCHAR (MAX)   NOT NULL,
    [Duracion]     TIME (7)        NULL,
    [DuracionHora] DECIMAL (18, 4) NULL,
    [RutaCodigo]   VARCHAR (50)    NULL,
    [RutaNombre]   VARCHAR (MAX)   NULL,
    [fechasistema] DATETIME        NOT NULL,
    [EsProcesado]  BIT             NULL,
    [Valor]        DECIMAL (18, 4) NULL,
    [EventId]      BIGINT          NULL,
    PRIMARY KEY CLUSTERED ([AlarmasId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [idxAlarmas_858segevent]
    ON [EBUS].[Alarmas_858]([AlarmasId] ASC);


GO
CREATE NONCLUSTERED INDEX [idxAlarmas_858segfecha]
    ON [EBUS].[Alarmas_858]([Fecha] ASC);

