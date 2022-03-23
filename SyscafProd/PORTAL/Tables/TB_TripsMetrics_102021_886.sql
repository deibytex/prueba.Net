CREATE TABLE [PORTAL].[TB_TripsMetrics_102021_886] (
    [TripId]       BIGINT   NOT NULL,
    [NIdleTime]    INT      NULL,
    [NIdleOccurs]  INT      NULL,
    [TripStart]    DATETIME NOT NULL,
    [ClienteIds]   INT      NULL,
    [fechasistema] DATETIME NOT NULL,
    PRIMARY KEY CLUSTERED ([TripId] ASC)
);

