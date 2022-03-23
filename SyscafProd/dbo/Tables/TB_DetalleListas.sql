CREATE TABLE [dbo].[TB_DetalleListas] (
    [DetalleListaId] INT           IDENTITY (1, 1) NOT NULL,
    [ListaId]        INT           NOT NULL,
    [Nombre]         VARCHAR (100) COLLATE Modern_Spanish_CI_AS NOT NULL,
    [Valor]          VARCHAR (200) COLLATE Modern_Spanish_CI_AS NULL,
    [Sigla]          VARCHAR (50)  COLLATE Modern_Spanish_CI_AS NOT NULL,
    [FechaSistema]   DATETIME      CONSTRAINT [DF__TB_Detall__Fecha__7A3223E8] DEFAULT (getdate()) NOT NULL,
    [EsActivo]       BIT           CONSTRAINT [DF__TB_Detall__EsAct__7B264821] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK__TB_Detal__892577CBC03221DC] PRIMARY KEY CLUSTERED ([DetalleListaId] ASC),
    CONSTRAINT [Pk_DetalleListas_Listas] FOREIGN KEY ([ListaId]) REFERENCES [dbo].[TB_Listas] ([ListaId]),
    CONSTRAINT [UQ__TB_Detal__3199C5ED024BCEF0] UNIQUE NONCLUSTERED ([Sigla] ASC)
);

