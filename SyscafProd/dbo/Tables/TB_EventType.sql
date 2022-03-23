CREATE TABLE [dbo].[TB_EventType] (
    [eventTypeIdS]     INT           IDENTITY (1, 1) NOT NULL,
    [eventTypeId]      VARCHAR (20)  COLLATE Modern_Spanish_CI_AS NOT NULL,
    [clienteIdS]       INT           NULL,
    [descriptionEvent] VARCHAR (200) COLLATE Modern_Spanish_CI_AS NULL,
    [eventType]        VARCHAR (100) NULL,
    [displayUnits]     VARCHAR (100) NULL,
    [formatType]       VARCHAR (100) NULL,
    [valueName]        VARCHAR (100) NULL,
    [SeTieneEnBase]    BIT           NULL,
    [clienteId]        VARCHAR (50)  COLLATE Modern_Spanish_CI_AS NULL,
    [grupoIdS]         INT           NULL,
    [estadoBase]       BIT           NULL,
    [fechaSistema]     DATETIME      NULL,
    CONSTRAINT [PK_TB_EventType] PRIMARY KEY CLUSTERED ([eventTypeIdS] ASC),
    CONSTRAINT [FK_TB_EventType_TB_Grupos] FOREIGN KEY ([grupoIdS]) REFERENCES [dbo].[TB_Grupos] ([grupoIdS]),
    CONSTRAINT [FK_TB_EventType_TB_Grupos1] FOREIGN KEY ([grupoIdS]) REFERENCES [dbo].[TB_Grupos] ([grupoIdS])
);

