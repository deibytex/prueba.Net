CREATE TABLE [dbo].[TB_GrupoSeguridadSites] (
    [GrupoSeguridadSiteId] INT      IDENTITY (1, 1) NOT NULL,
    [GrupoSeguridadId]     INT      NOT NULL,
    [ClienteIds]           INT      NULL,
    [SiteIds]              INT      NULL,
    [TipoSeguridadId]      INT      NULL,
    [FechaSistema]         DATETIME NOT NULL,
    [EsActivo]             BIT      CONSTRAINT [DF_GrupoSeguridadSites_EsActivo] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_GrupoSeguridadSites] PRIMARY KEY CLUSTERED ([GrupoSeguridadSiteId] ASC),
    CONSTRAINT [DF_GrupoSeguridadSites_DetallesListas] FOREIGN KEY ([TipoSeguridadId]) REFERENCES [dbo].[TB_DetalleListas] ([DetalleListaId]),
    CONSTRAINT [DF_GrupoSeguridadSites_Site] FOREIGN KEY ([SiteIds]) REFERENCES [dbo].[TB_Site] ([siteIdS]),
    CONSTRAINT [FK_TB_GrupoSeguridadSites_TB_Cliente] FOREIGN KEY ([ClienteIds]) REFERENCES [dbo].[TB_Cliente] ([clienteIdS]),
    CONSTRAINT [FK_TB_GrupoSeguridadSites_TB_GruposDeSeguridad] FOREIGN KEY ([GrupoSeguridadId]) REFERENCES [dbo].[TB_GruposDeSeguridad] ([GrupoSeguridadId])
);

