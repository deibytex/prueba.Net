CREATE TABLE [dbo].[TB_PruebaSimCard] (
    [Id]                       INT           IDENTITY (1, 1) NOT NULL,
    [Placa]                    VARCHAR (150) NOT NULL,
    [UltimoAvl]                DATETIME      NOT NULL,
    [FechaSistema]             DATETIME      NOT NULL,
    [ProcesoGeneracionDatosId] INT           NULL,
    [Latitud]                  FLOAT (53)    NULL,
    [Longitud]                 FLOAT (53)    NULL,
    CONSTRAINT [PK_TB_PruebasMovistar] PRIMARY KEY CLUSTERED ([Id] ASC)
);

