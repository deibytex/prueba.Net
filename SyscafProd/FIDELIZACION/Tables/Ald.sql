CREATE TABLE [FIDELIZACION].[Ald] (
    [AldId]        INT      IDENTITY (1, 1) NOT NULL,
    [ClienteId]    INT      NOT NULL,
    [FechaInicial] DATETIME NOT NULL,
    [FechaFinal]   DATETIME NOT NULL,
    [FechaSistema] DATETIME NOT NULL,
    [EsActivo]     BIT      CONSTRAINT [DF_Ald_EsActivo] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Ald] PRIMARY KEY CLUSTERED ([AldId] ASC)
);

