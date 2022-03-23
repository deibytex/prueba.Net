CREATE TABLE [NS].[TB_ListaDistribucionCorreo] (
    [ListaDistribucionId] INT           IDENTITY (1, 1) NOT NULL,
    [TipoListaId]         INT           NOT NULL,
    [Nombres]             VARCHAR (200) COLLATE Modern_Spanish_CI_AS NOT NULL,
    [Descripcion]         VARCHAR (500) COLLATE Modern_Spanish_CI_AS NULL,
    [FechaSistema]        DATETIME      CONSTRAINT [DF__TB_ListaD__Fecha__7FEAFD3E] DEFAULT (getdate()) NOT NULL,
    [EsActivo]            BIT           CONSTRAINT [DF__TB_ListaD__EsAct__00DF2177] DEFAULT ((1)) NOT NULL,
    [Sigla]               VARCHAR (50)  NOT NULL,
    CONSTRAINT [PK__TB_Lista__DAC9F027735CC70A] PRIMARY KEY CLUSTERED ([ListaDistribucionId] ASC),
    CONSTRAINT [Pk_TB_ListaDistribucionCorreo_TB_DetalleListas] FOREIGN KEY ([TipoListaId]) REFERENCES [dbo].[TB_DetalleListas] ([DetalleListaId]),
    CONSTRAINT [UQ__TB_Lista__8C968189686BF44C] UNIQUE NONCLUSTERED ([Nombres] ASC)
);

