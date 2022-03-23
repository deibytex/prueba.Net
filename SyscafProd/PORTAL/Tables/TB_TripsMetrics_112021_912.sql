CREATE TABLE [PORTAL].[TB_TripsMetrics_112021_912] (
    [TripId]       BIGINT   NOT NULL,
    [NIdleTime]    INT      NULL,
    [NIdleOccurs]  INT      NULL,
    [TripStart]    DATETIME NOT NULL,
    [ClienteIds]   INT      NULL,
    [fechasistema] DATETIME NOT NULL,
    [EsProcesado]  BIT      DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([TripId] ASC)
);

