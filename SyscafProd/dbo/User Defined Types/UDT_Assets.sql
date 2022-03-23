CREATE TYPE [dbo].[UDT_Assets] AS TABLE (
    [assetId]            BIGINT        NOT NULL,
    [groupId]            BIGINT        NULL,
    [createdDate]        DATETIME      NULL,
    [registrationNumber] VARCHAR (100) NULL,
    [assetsDescription]  VARCHAR (500) NULL,
    [assetCodigo]        INT           NULL,
    [siteIds]            INT           NULL,
    PRIMARY KEY CLUSTERED ([assetId] ASC));

