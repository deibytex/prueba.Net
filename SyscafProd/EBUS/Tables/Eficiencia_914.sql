CREATE TABLE [EBUS].[Eficiencia_914] (
    [EficienciaId]  INT             IDENTITY (1, 1) NOT NULL,
    [Movil]         VARCHAR (MAX)   NOT NULL,
    [Operador]      VARCHAR (MAX)   NULL,
    [Fecha]         DATE            NOT NULL,
    [Inicio]        DATETIME        NOT NULL,
    [Fin]           DATETIME        NOT NULL,
    [Carga]         DECIMAL (18, 4) NULL,
    [Descarga]      DECIMAL (18, 4) NULL,
    [Distancia]     DECIMAL (18, 4) NULL,
    [Duracion]      TIME (7)        NULL,
    [DuracionHora]  DECIMAL (18, 4) NULL,
    [TotalConsumo]  DECIMAL (18, 4) NULL,
    [MaxSOC]        INT             NULL,
    [MinSOC]        INT             NULL,
    [DSOC]          INT             NULL,
    [RutaCodigo]    VARCHAR (50)    NULL,
    [RutaNombre]    VARCHAR (MAX)   NULL,
    [fechasistema]  DATETIME        NOT NULL,
    [EsProcesado]   BIT             NULL,
    [StartOdometer] DECIMAL (18, 4) NULL,
    [EndOdometer]   DECIMAL (18, 4) NULL,
    [TripId]        BIGINT          NULL,
    PRIMARY KEY CLUSTERED ([EficienciaId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [idxteficiencia_914fecha]
    ON [EBUS].[Eficiencia_914]([Fecha] ASC)
    INCLUDE([TripId], [Movil], [Inicio], [Fin]);

