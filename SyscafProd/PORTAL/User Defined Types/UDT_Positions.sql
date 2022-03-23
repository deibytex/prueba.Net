CREATE TYPE [PORTAL].[UDT_Positions] AS TABLE (
    [PositionId]         BIGINT        NOT NULL,
    [assetId]            BIGINT        NOT NULL,
    [driverId]           BIGINT        NOT NULL,
    [Timestamp]          DATETIME      NULL,
    [Longitude]          FLOAT (53)    NULL,
    [Latitude]           FLOAT (53)    NULL,
    [FormattedAddress]   VARCHAR (250) NOT NULL,
    [AltitudeMetres]     INT           NULL,
    [NumberOfSatellites] INT           NULL,
    [ClienteIds]         INT           NULL,
    [fechasistema]       DATETIME      NOT NULL,
    PRIMARY KEY CLUSTERED ([PositionId] ASC));

