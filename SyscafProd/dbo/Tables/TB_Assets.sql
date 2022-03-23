CREATE TABLE [dbo].[TB_Assets] (
    [assetIdS]           INT           IDENTITY (1, 1) NOT NULL,
    [assetId]            VARCHAR (50)  COLLATE Modern_Spanish_CI_AS NULL,
    [groupId]            VARCHAR (50)  COLLATE Modern_Spanish_CI_AS NULL,
    [createdDate]        DATETIME      NOT NULL,
    [siteIdS]            INT           NOT NULL,
    [registrationNumber] VARCHAR (200) COLLATE Modern_Spanish_CI_AS NOT NULL,
    [assetsDescription]  VARCHAR (200) COLLATE Modern_Spanish_CI_AS NOT NULL,
    [estadoClienteIdS]   INT           NULL,
    [estadoSyscafIdS]    INT           NULL,
    [assetCodigo]        INT           NULL,
    [clienteIdS]         INT           NULL,
    [FechaSistema]       DATETIME      NULL,
    [AssetIdF]           BIGINT        NULL,
    CONSTRAINT [PK__TB_Asset__CB8F7BE6FAE03368] PRIMARY KEY CLUSTERED ([assetIdS] ASC),
    CONSTRAINT [FK_TB_Assets_TB_cliente] FOREIGN KEY ([clienteIdS]) REFERENCES [dbo].[TB_Cliente] ([clienteIdS]),
    CONSTRAINT [FK_TB_Assets_TB_Site] FOREIGN KEY ([siteIdS]) REFERENCES [dbo].[TB_Site] ([siteIdS])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [MixIndex]
    ON [dbo].[TB_Assets]([assetId] ASC);

