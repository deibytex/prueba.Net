CREATE TABLE [WS].[TB_Servicios] (
    [ServicioId]   INT           IDENTITY (1, 1) NOT NULL,
    [Nombre]       VARCHAR (100) NOT NULL,
    [Descripcion]  VARCHAR (500) NULL,
    [Sigla]        VARCHAR (10)  NULL,
    [FechaSistema] DATETIME      DEFAULT (getdate()) NOT NULL,
    [EsActivo]     BIT           DEFAULT ((1)) NOT NULL,
    [Frecuencia]   INT           NULL,
    PRIMARY KEY CLUSTERED ([ServicioId] ASC),
    UNIQUE NONCLUSTERED ([Nombre] ASC),
    UNIQUE NONCLUSTERED ([Sigla] ASC)
);

