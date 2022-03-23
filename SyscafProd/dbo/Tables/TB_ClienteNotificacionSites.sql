CREATE TABLE [dbo].[TB_ClienteNotificacionSites] (
    [ListaClienteSitesId]      INT      IDENTITY (1, 1) NOT NULL,
    [ListaClienteNotifacionId] INT      NOT NULL,
    [SiteIds]                  INT      NOT NULL,
    [EsActivo]                 BIT      CONSTRAINT [DF_TB_ClienteNotificacionSites_EsActivo] DEFAULT ((1)) NOT NULL,
    [FechaSistema]             DATETIME CONSTRAINT [DF_TB_ClienteNotificacionSites_FechaSistema] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_TB_ClienteNotificacionSites] PRIMARY KEY CLUSTERED ([ListaClienteSitesId] ASC)
);

