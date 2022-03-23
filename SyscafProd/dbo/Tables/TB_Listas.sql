CREATE TABLE [dbo].[TB_Listas] (
    [ListaId]      INT           IDENTITY (1, 1) NOT NULL,
    [Nombre]       VARCHAR (100) COLLATE Modern_Spanish_CI_AS NOT NULL,
    [Descripcion]  VARCHAR (200) COLLATE Modern_Spanish_CI_AS NULL,
    [Sigla]        VARCHAR (10)  COLLATE Modern_Spanish_CI_AS NOT NULL,
    [FechaSistema] DATETIME      CONSTRAINT [DF__TB_Listas__Fecha__756D6ECB] DEFAULT (getdate()) NOT NULL,
    [EsActivo]     BIT           CONSTRAINT [DF__TB_Listas__EsAct__76619304] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK__TB_Lista__2B0A741FBD4213A0] PRIMARY KEY CLUSTERED ([ListaId] ASC),
    CONSTRAINT [UQ__TB_Lista__3199C5ED50666BC6] UNIQUE NONCLUSTERED ([Sigla] ASC)
);

