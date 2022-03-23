﻿CREATE TABLE [PORTAL].[TB_TripsMetrics_92021_857] (
    [TripId]       BIGINT   NOT NULL,
    [NIdleTime]    INT      NULL,
    [NIdleOccurs]  INT      NULL,
    [TripStart]    DATETIME NOT NULL,
    [ClienteIds]   INT      NULL,
    [fechasistema] DATETIME NOT NULL,
    PRIMARY KEY CLUSTERED ([TripId] ASC)
);

