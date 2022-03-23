﻿CREATE TABLE [dbo].[TB_ErroresViajesyUso] (
    [Consecutivo]        INT             IDENTITY (1, 1) NOT NULL,
    [Cliente]            VARCHAR (300)   NULL,
    [US]                 VARCHAR (3)     NULL,
    [VehicleID]          INT             NULL,
    [Placa]              VARCHAR (100)   NULL,
    [VehicleSiteID]      BIGINT          NULL,
    [VehicleSiteName]    VARCHAR (200)   NULL,
    [TripNo]             BIGINT          NULL,
    [DriverID]           BIGINT          NULL,
    [DriverName]         VARCHAR (300)   NULL,
    [DriverSiteID]       BIGINT          NULL,
    [DriverSiteName]     VARCHAR (200)   NULL,
    [OriginalDriverID]   INT             NULL,
    [OriginalDriverName] VARCHAR (50)    NULL,
    [TripStart]          DATETIME        NULL,
    [TripEnd]            DATETIME        NULL,
    [CategoryID]         INT             NULL,
    [Notes]              VARCHAR (50)    NULL,
    [StartSubTripSeq]    INT             NULL,
    [EndSubTripSeq]      INT             NULL,
    [TripDistance]       DECIMAL (18, 1) NULL,
    [Odometer]           DECIMAL (18, 1) NULL,
    [MaxSpeed]           INT             NULL,
    [SpeedTime]          INT             NULL,
    [SpeedOccurs]        INT             NULL,
    [MaxBrake]           INT             NULL,
    [BrakeTime]          INT             NULL,
    [BrakeOccurs]        INT             NULL,
    [MaxAccel]           INT             NULL,
    [AccelTime]          INT             NULL,
    [AccelOccurs]        INT             NULL,
    [MaxRPM]             INT             NULL,
    [RPMTime]            INT             NULL,
    [RPMOccurs]          INT             NULL,
    [GBTime]             INT             NULL,
    [ExIdleTime]         INT             NULL,
    [ExIdleOccurs]       INT             NULL,
    [NIdleTime]          INT             NULL,
    [NIdleOccurs]        INT             NULL,
    [StandingTime]       INT             NULL,
    [Litres]             DECIMAL (18, 1) NULL,
    [StartGPSID]         BIGINT          NULL,
    [EndGPSID]           BIGINT          NULL,
    [StartEngineSeconds] INT             NULL,
    [EndEngineSeconds]   INT             NULL,
    [ClienteIdS]         INT             NOT NULL,
    [FechaSistema]       DATETIME        NULL,
    CONSTRAINT [PK_TB_ErroresViajesyUso] PRIMARY KEY CLUSTERED ([Consecutivo] ASC)
);


GO
CREATE NONCLUSTERED INDEX [ErroresEventosUsos]
    ON [dbo].[TB_ErroresViajesyUso]([ClienteIdS] ASC, [TripStart] ASC);


GO
CREATE NONCLUSTERED INDEX [FechaSistemaErroresViajesUsos]
    ON [dbo].[TB_ErroresViajesyUso]([FechaSistema] ASC)
    INCLUDE([TripNo]);

