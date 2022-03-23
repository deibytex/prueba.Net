CREATE TABLE [ADM].[TB_OperacionOpcion] (
    [OperacionOpcionId] INT           IDENTITY (1, 1) NOT NULL,
    [Nombre]            VARCHAR (100) COLLATE Modern_Spanish_CI_AS NOT NULL,
    [Descripcion]       VARCHAR (200) COLLATE Modern_Spanish_CI_AS NULL,
    [Sigla]             VARCHAR (10)  COLLATE Modern_Spanish_CI_AS NOT NULL,
    [FechaSistema]      DATETIME      NOT NULL,
    [EsActivo]          BIT           CONSTRAINT [DF__TB_Operac__EsAct__318258D2] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK__TB_Opera__198D2A0F71186D8F] PRIMARY KEY CLUSTERED ([OperacionOpcionId] ASC),
    CONSTRAINT [UQ__TB_Opera__3199C5ED67AB786B] UNIQUE NONCLUSTERED ([Sigla] ASC)
);

