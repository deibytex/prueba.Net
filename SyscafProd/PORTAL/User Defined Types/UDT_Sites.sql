CREATE TYPE [PORTAL].[UDT_Sites] AS TABLE (
    [SiteId]      BIGINT        NOT NULL,
    [SiteName]    VARCHAR (200) NOT NULL,
    [SitePadreId] BIGINT        NULL,
    PRIMARY KEY CLUSTERED ([SiteId] ASC));

