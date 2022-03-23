CREATE TABLE [dbo].[TB_DiasExcepciones] (
    [DiasExcepcionId] INT      IDENTITY (1, 1) NOT NULL,
    [Dia]             DATETIME NULL,
    [Hora]            TIME (7) NULL,
    [TipoExcepcionId] INT      NOT NULL,
    [FechaSistema]    DATETIME NOT NULL,
    [EsActivo]        BIT      DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([DiasExcepcionId] ASC),
    FOREIGN KEY ([TipoExcepcionId]) REFERENCES [dbo].[TB_DetalleListas] ([DetalleListaId])
);

