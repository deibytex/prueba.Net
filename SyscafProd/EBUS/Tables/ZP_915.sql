CREATE TABLE [EBUS].[ZP_915] (
    [ZPId]         INT             IDENTITY (1, 1) NOT NULL,
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
    [EventId]      BIGINT          NULL,
    PRIMARY KEY CLUSTERED ([ZPId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [idxzp_915fecha]
    ON [EBUS].[ZP_915]([Fecha] ASC);

