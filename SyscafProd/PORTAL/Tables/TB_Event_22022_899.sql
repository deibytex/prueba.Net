CREATE TABLE [PORTAL].[TB_Event_22022_899] (
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
    [EsActivo]                BIT             DEFAULT ((1)) NOT NULL,
    [EsProcesado]             BIT             DEFAULT ((0)) NOT NULL,
    [IseBus]                  BIT             DEFAULT ((0)) NULL,
    PRIMARY KEY CLUSTERED ([EventId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [nci_wi_TB_Event_22022_899]
    ON [PORTAL].[TB_Event_22022_899]([EventTypeId] ASC)
    INCLUDE([assetId], [StartDateTime], [driverId], [EsProcesado], [IseBus], [EsActivo]);


GO
CREATE NONCLUSTERED INDEX [nci_wi_TB_startdatetime_22022_899]
    ON [PORTAL].[TB_Event_22022_899]([StartDateTime] ASC)
    INCLUDE([EventTypeId], [assetId], [driverId], [EsProcesado], [IseBus], [EsActivo]);

