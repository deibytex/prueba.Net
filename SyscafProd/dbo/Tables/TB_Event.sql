CREATE TABLE [dbo].[TB_Event] (
    [EventIdS]           INT             IDENTITY (1, 1) NOT NULL,
    [EventId]            VARCHAR (30)    COLLATE Modern_Spanish_CI_AS NULL,
    [DriverId]           VARCHAR (30)    COLLATE Modern_Spanish_CI_AS NULL,
    [AssetId]            VARCHAR (30)    COLLATE Modern_Spanish_CI_AS NULL,
    [EventDateTime]      DATETIME        NOT NULL,
    [ReceivedDateTime]   DATETIME        NULL,
    [EventTypeId]        VARCHAR (30)    COLLATE Modern_Spanish_CI_AS NULL,
    [OdometerKilometres] DECIMAL (11, 4) NULL,
    [ValueUnits]         VARCHAR (30)    COLLATE Modern_Spanish_CI_AS NULL,
    [ValueType]          VARCHAR (30)    COLLATE Modern_Spanish_CI_AS NULL,
    [ValueEvent]         DECIMAL (11, 4) NULL,
    [SpeedLimit]         DECIMAL (11, 4) NULL,
    [PositionId]         VARCHAR (30)    COLLATE Modern_Spanish_CI_AS NULL,
    [PriorityEvent]      INT             NULL,
    [Armed]              BIT             NULL,
    [assetIdS]           INT             NULL,
    [driverIdS]          INT             NULL,
    [eventTypeIdS]       INT             NULL,
    [grupoIdS]           INT             NULL,
    [positionIdS]        INT             NULL,
    [fechaSistema]       DATETIME        NULL,
    [estadoBase]         BIT             NULL,
    [TotalOccurances]    INT             NULL,
    [TotalTimeSeconds]   INT             NULL,
    [ClienteIds]         INT             NULL,
    CONSTRAINT [PK_TB_Event] PRIMARY KEY CLUSTERED ([EventIdS] ASC),
    CONSTRAINT [FK_TB_Event_TB_Assets] FOREIGN KEY ([assetIdS]) REFERENCES [dbo].[TB_Assets] ([assetIdS]),
    UNIQUE NONCLUSTERED ([EventId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [idx_evendatetime]
    ON [dbo].[TB_Event]([EventDateTime] ASC);

