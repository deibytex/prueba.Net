CREATE TABLE [dbo].[TB_Senales] (
    [senalesIdS]        INT IDENTITY (1, 1) NOT NULL,
    [assetsIdS]         INT NOT NULL,
    [diasTransmision]   INT NULL,
    [maxVelFM]          INT NULL,
    [minVelFM]          INT NULL,
    [RPM_Max]           INT NULL,
    [RPM_Min]           INT NULL,
    [MaxAccelereciones] INT NULL,
    [MinAccelereciones] INT NULL,
    PRIMARY KEY CLUSTERED ([senalesIdS] ASC),
    CONSTRAINT [FK_TB_Senales_TB_Assets] FOREIGN KEY ([assetsIdS]) REFERENCES [dbo].[TB_Assets] ([assetIdS])
);


GO
CREATE NONCLUSTERED INDEX [IDX_assetsIdS]
    ON [dbo].[TB_Senales]([assetsIdS] ASC);

