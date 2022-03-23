CREATE TABLE [dbo].[TB_AdministradorClientes] (
    [AdministradorId] INT      IDENTITY (1, 1) NOT NULL,
    [UsuarioIds]      INT      NOT NULL,
    [ClienteIds]      INT      NULL,
    [SiteIds]         INT      NULL,
    [FechaSistema]    DATETIME NOT NULL,
    [EsActivo]        BIT      CONSTRAINT [DF_TB_AdministradorClientes_EsActivo] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_TB_AdministradorClientes] PRIMARY KEY CLUSTERED ([AdministradorId] ASC),
    CONSTRAINT [FK_TB_AdministradorClientes_TB_AdministradorClientes] FOREIGN KEY ([AdministradorId]) REFERENCES [dbo].[TB_AdministradorClientes] ([AdministradorId]),
    CONSTRAINT [FK_TB_AdministradorClientes_TB_AdministradorClientes1] FOREIGN KEY ([AdministradorId]) REFERENCES [dbo].[TB_AdministradorClientes] ([AdministradorId]),
    CONSTRAINT [FK_TB_AdministradorClientes_TB_Site] FOREIGN KEY ([SiteIds]) REFERENCES [dbo].[TB_Site] ([siteIdS])
);

