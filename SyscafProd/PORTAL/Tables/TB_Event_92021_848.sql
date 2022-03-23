CREATE TABLE [PORTAL].[TB_Event_92021_848] (
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
    PRIMARY KEY CLUSTERED ([EventId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [nci_wi_TB_Event_92021_848]
    ON [PORTAL].[TB_Event_92021_848]([ClienteIds] ASC, [EventTypeId] ASC)
    INCLUDE([assetId], [FuelUsedLitres], [StartDateTime], [TotalOccurances], [TotalTimeSeconds]);


GO
CREATE NONCLUSTERED INDEX [nci_wi_TB_EventTypeId_92021_848]
    ON [PORTAL].[TB_Event_92021_848]([EventTypeId] ASC);


GO
CREATE NONCLUSTERED INDEX [nci_wi_TB_startdatetime_92021_848]
    ON [PORTAL].[TB_Event_92021_848]([StartDateTime] ASC);

