CREATE TABLE [dbo].[TB_AssetType] (
    [assetTypeIdS]    INT           IDENTITY (1, 1) NOT NULL,
    [assetTypeNombre] VARCHAR (100) COLLATE Modern_Spanish_CI_AS NULL,
    [estadoBase]      BIT           NULL,
    [fechaSistema]    DATETIME      NULL,
    CONSTRAINT [PK_TB_AssetType] PRIMARY KEY CLUSTERED ([assetTypeIdS] ASC)
);

