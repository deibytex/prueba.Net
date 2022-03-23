CREATE TYPE [dbo].[UDT_TiposEventos] AS TABLE (
    [eventTypeId]      BIGINT        NOT NULL,
    [clienteIdS]       INT           NULL,
    [descriptionEvent] VARCHAR (200) NULL,
    [eventType]        VARCHAR (100) NULL,
    [displayUnits]     VARCHAR (100) NULL,
    [formatType]       VARCHAR (100) NULL,
    [valueName]        VARCHAR (100) NULL,
    PRIMARY KEY CLUSTERED ([eventTypeId] ASC));

