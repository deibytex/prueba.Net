CREATE TABLE [dbo].[TB_Site] (
    [siteIdS]      INT          IDENTITY (1, 1) NOT NULL,
    [siteId]       VARCHAR (50) COLLATE Modern_Spanish_CI_AS NOT NULL,
    [siteName]     VARCHAR (50) COLLATE Modern_Spanish_CI_AS NOT NULL,
    [clienteIdS]   INT          NULL,
    [sitePadreId]  VARCHAR (50) COLLATE Modern_Spanish_CI_AS NULL,
    [tipoSitio]    INT          NULL,
    [grupoIdS]     INT          NULL,
    [fechaSistema] DATETIME     NULL,
    [estadoBase]   BIT          NULL,
    [sitePadreIdS] INT          NULL,
    [SiteIdBigInt] BIGINT       NULL,
    CONSTRAINT [PK_TB_Site] PRIMARY KEY CLUSTERED ([siteIdS] ASC),
    CONSTRAINT [FK__TB_Site__cliente__5F141958] FOREIGN KEY ([clienteIdS]) REFERENCES [dbo].[TB_Cliente] ([clienteIdS]),
    CONSTRAINT [FK_TB_Site_TB_Cliente] FOREIGN KEY ([clienteIdS]) REFERENCES [dbo].[TB_Cliente] ([clienteIdS]),
    CONSTRAINT [FK_TB_Site_TB_Grupos] FOREIGN KEY ([grupoIdS]) REFERENCES [dbo].[TB_Grupos] ([grupoIdS])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [MixIndex]
    ON [dbo].[TB_Site]([siteId] ASC);

