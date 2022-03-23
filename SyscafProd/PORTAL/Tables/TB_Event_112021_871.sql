﻿CREATE TABLE [PORTAL].[TB_Event_112021_871] (
    [EventId]                 BIGINT          NOT NULL,
    [assetId]                 BIGINT          NOT NULL,
    [driverId]                BIGINT          NOT NULL,
    [EventTypeId]             BIGINT          NOT NULL,
    [TotalTimeSeconds]        INT             NOT NULL,
    [TotalOccurances]         INT             NOT NULL,
    [StartDateTime]           DATETIME        NOT NULL,
    [EndDateTime]             DATETIME        NULL,
    [FuelUsedLitres]          DECIMAL (10, 4) NULL,
    [Value]                   DECIMAL (11, 4) NULL,
    [Latitud]                 FLOAT (53)      NULL,
    [Longitud]                FLOAT (53)      NULL,
    [StartOdometerKilometres] DECIMAL (18, 4) NULL,
    [EndOdometerKilometres]   DECIMAL (18, 4) NULL,
    [AltitudMeters]           INT             NULL,
    [ClienteIds]              INT             NULL,
    [fechasistema]            DATETIME        NOT NULL,
    [EsProcesado]             BIT             DEFAULT ((0)) NOT NULL,
    [EsActivo]                BIT             DEFAULT ((1)) NULL,
    [IseBus]                  BIT             DEFAULT ((0)) NULL,
    PRIMARY KEY CLUSTERED ([EventId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [nci_wi_TB_Event_112021_871]
    ON [PORTAL].[TB_Event_112021_871]([ClienteIds] ASC, [EventTypeId] ASC)
    INCLUDE([assetId], [FuelUsedLitres], [StartDateTime], [TotalOccurances], [TotalTimeSeconds]);


GO
CREATE NONCLUSTERED INDEX [nci_wi_TB_EventTypeId_112021_871]
    ON [PORTAL].[TB_Event_112021_871]([EventTypeId] ASC);


GO
CREATE NONCLUSTERED INDEX [nci_wi_TB_startdatetime_112021_871]
    ON [PORTAL].[TB_Event_112021_871]([StartDateTime] ASC);


GO
CREATE NONCLUSTERED INDEX [idx_esprocesado_event_112021_871]
    ON [PORTAL].[TB_Event_112021_871]([EsProcesado] ASC);


GO
CREATE NONCLUSTERED INDEX [TB_Event_112021_871_IsEbus]
    ON [PORTAL].[TB_Event_112021_871]([IseBus] ASC);

