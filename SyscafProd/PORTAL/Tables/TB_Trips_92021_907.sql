﻿CREATE TABLE [PORTAL].[TB_Trips_92021_907] (
    [TripId]                                    BIGINT          NOT NULL,
    [assetId]                                   BIGINT          NOT NULL,
    [driverId]                                  BIGINT          NOT NULL,
    [notes]                                     VARCHAR (200)   NULL,
    [distanceKilometers]                        DECIMAL (18, 4) NULL,
    [StartOdometerKilometers]                   DECIMAL (18, 4) NULL,
    [endOdometerKilometers]                     DECIMAL (18, 4) NULL,
    [maxSpeedKilometersPerHour]                 DECIMAL (18, 4) NULL,
    [maxAccelerationKilometersPerHourPerSecond] DECIMAL (18, 4) NULL,
    [maxDecelerationKilometersPerHourPerSecond] DECIMAL (18, 4) NULL,
    [maxRpm]                                    DECIMAL (18, 4) NULL,
    [standingTime]                              DECIMAL (18, 4) NULL,
    [fuelUsedLitres]                            DECIMAL (18, 4) NULL,
    [startPositionId]                           VARCHAR (50)    NULL,
    [endPositionId]                             VARCHAR (50)    NULL,
    [startEngineSeconds]                        DECIMAL (18, 4) NULL,
    [endEngineSeconds]                          INT             NULL,
    [tripEnd]                                   DATETIME        NOT NULL,
    [tripStart]                                 DATETIME        NOT NULL,
    [ClienteIds]                                INT             NULL,
    [CantSubtrips]                              INT             NULL,
    [EsProcesado]                               BIT             DEFAULT ((0)) NOT NULL,
    [fechasistema]                              DATETIME        NOT NULL,
    PRIMARY KEY CLUSTERED ([TripId] ASC)
);

