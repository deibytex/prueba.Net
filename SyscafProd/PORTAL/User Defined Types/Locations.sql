CREATE TYPE [PORTAL].[Locations] AS TABLE (
    [LocationId]     BIGINT          NOT NULL,
    [OrganisationId] BIGINT          NOT NULL,
    [ClienteIds]     INT             NOT NULL,
    [Name]           VARCHAR (200)   NULL,
    [Address]        VARCHAR (200)   NULL,
    [ShapeWkt]       VARCHAR (MAX)   NULL,
    [Radius]         FLOAT (53)      NULL,
    [ColourOnMap]    VARCHAR (50)    NULL,
    [OpacityOnMap]   DECIMAL (18, 2) NULL,
    [LocationType]   VARCHAR (50)    NULL,
    [ShapeType]      VARCHAR (50)    NULL,
    [FechaSistema]   DATETIME        NOT NULL,
    [IsDeleted]      BIT             NOT NULL,
    PRIMARY KEY CLUSTERED ([LocationId] ASC));

