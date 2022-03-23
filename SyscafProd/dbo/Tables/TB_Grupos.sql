CREATE TABLE [dbo].[TB_Grupos] (
    [grupoIdS]     INT           IDENTITY (1, 1) NOT NULL,
    [nombre]       VARCHAR (200) COLLATE Modern_Spanish_CI_AS NOT NULL,
    [FechaSistema] DATETIME      NULL,
    [EsActivo]     BIT           CONSTRAINT [DF_TB_Grupos_EsActivo] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_TB_Grupos] PRIMARY KEY CLUSTERED ([grupoIdS] ASC)
);

