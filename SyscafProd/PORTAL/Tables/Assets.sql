CREATE TABLE [PORTAL].[Assets] (
    [AssetId]            BIGINT        NOT NULL,
    [ClienteIdS]         INT           NULL,
    [AssetCodigo]        INT           NULL,
    [RegistrationNumber] VARCHAR (200) NOT NULL,
    [AssetsDescription]  VARCHAR (200) NOT NULL,
    [CreatedDate]        DATETIME      NOT NULL,
    [SiteIdS]            INT           NOT NULL,
    [EstadoClienteIdS]   INT           NULL,
    [EstadoSyscafIdS]    INT           NULL,
    [FechaSistema]       DATETIME      NULL,
    CONSTRAINT [PK__Portal_TB_Asset__CB8F7BE6FAE03368] PRIMARY KEY CLUSTERED ([AssetId] ASC),
    CONSTRAINT [FK_TB_Assets_TB_cliente] FOREIGN KEY ([ClienteIdS]) REFERENCES [dbo].[TB_Cliente] ([clienteIdS]),
    CONSTRAINT [FK_TB_Assets_TB_Site] FOREIGN KEY ([SiteIdS]) REFERENCES [dbo].[TB_Site] ([siteIdS])
);


GO
CREATE NONCLUSTERED INDEX [nci_wi_TB_AssetsPortal_9C54A365B9AED837ABDA9242F30011C8]
    ON [PORTAL].[Assets]([ClienteIdS] ASC, [SiteIdS] ASC)
    INCLUDE([AssetCodigo], [AssetId], [AssetsDescription], [CreatedDate], [EstadoClienteIdS], [EstadoSyscafIdS], [RegistrationNumber]);

