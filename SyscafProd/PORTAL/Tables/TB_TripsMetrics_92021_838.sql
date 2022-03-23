CREATE TABLE [PORTAL].[TB_TripsMetrics_92021_838] (
    [TripId]       BIGINT   NOT NULL,
    [NIdleTime]    INT      NULL,
    [NIdleOccurs]  INT      NULL,
    [TripStart]    DATETIME NOT NULL,
    [ClienteIds]   INT      NULL,
    [fechasistema] DATETIME NOT NULL,
    PRIMARY KEY CLUSTERED ([TripId] ASC)
);

