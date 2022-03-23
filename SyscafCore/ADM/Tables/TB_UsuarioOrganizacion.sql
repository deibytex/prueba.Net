CREATE TABLE [ADM].[TB_UsuarioOrganizacion] (
    [UsuarioOrganizacionId] INT      IDENTITY (1, 1) NOT NULL,
    [OrganizacionId]        INT      NOT NULL,
    [UsuarioId]             INT      NOT NULL,
    [FechaSistema]          DATETIME NOT NULL,
    [EsActivo]              BIT      CONSTRAINT [DF__TB_Usuari__EsAct__1E6F845E] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK__TB_Usuar__0288CBF7A25BCBF2] PRIMARY KEY CLUSTERED ([UsuarioOrganizacionId] ASC),
    CONSTRAINT [FK__TB_Usuari__Organ__1F63A897] FOREIGN KEY ([OrganizacionId]) REFERENCES [ADM].[TB_Organizacion] ([OrganizacionId])
);

