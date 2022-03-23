CREATE TABLE [ADM].[TB_Roles] (
    [RolId]          INT           IDENTITY (1, 1) NOT NULL,
    [OrganizacionId] INT           NOT NULL,
    [Nombre]         VARCHAR (100) COLLATE Modern_Spanish_CI_AS NOT NULL,
    [Descripcion]    VARCHAR (200) COLLATE Modern_Spanish_CI_AS NULL,
    [Sigla]          VARCHAR (50)  COLLATE Modern_Spanish_CI_AS NOT NULL,
    [FechaSistema]   DATETIME      NOT NULL,
    [EsActivo]       BIT           CONSTRAINT [DF__TB_Roles__EsActi__2334397B] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK__TB_Roles__F92302F1E848F3AB] PRIMARY KEY CLUSTERED ([RolId] ASC),
    CONSTRAINT [FK__TB_Roles__Organi__251C81ED] FOREIGN KEY ([OrganizacionId]) REFERENCES [ADM].[TB_Organizacion] ([OrganizacionId]),
    CONSTRAINT [UQ__TB_Roles__3199C5ED1CC9B0EE] UNIQUE NONCLUSTERED ([Sigla] ASC)
);

