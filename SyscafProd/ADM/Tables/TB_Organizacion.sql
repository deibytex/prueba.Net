CREATE TABLE [ADM].[TB_Organizacion] (
    [OrganizacionId] INT           IDENTITY (1, 1) NOT NULL,
    [Nombre]         VARCHAR (100) COLLATE Modern_Spanish_CI_AS NOT NULL,
    [Descripcion]    VARCHAR (200) COLLATE Modern_Spanish_CI_AS NOT NULL,
    [FechaSistema]   DATETIME      NOT NULL,
    [EsActivo]       BIT           CONSTRAINT [DF__TB_Organi__EsAct__1B9317B3] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK__TB_Organ__5B2781CF231BE63E] PRIMARY KEY CLUSTERED ([OrganizacionId] ASC)
);

