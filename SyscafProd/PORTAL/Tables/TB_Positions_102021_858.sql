CREATE TABLE [PORTAL].[TB_Positions_102021_858] (
    [PositionId]         BIGINT        NOT NULL,
    [assetId]            BIGINT        NOT NULL,
    [driverId]           BIGINT        NOT NULL,
    [Timestamp]          DATETIME      NULL,
    [Latitud]            FLOAT (53)    NULL,
    [Longitud]           FLOAT (53)    NULL,
    [FormattedAddress]   VARCHAR (250) NOT NULL,
    [AltitudeMetres]     INT           NULL,
    [NumberOfSatellites] INT           NULL,
    [ClienteIds]         INT           NULL,
    [fechasistema]       DATETIME      NOT NULL,
    PRIMARY KEY CLUSTERED ([PositionId] ASC)
);

