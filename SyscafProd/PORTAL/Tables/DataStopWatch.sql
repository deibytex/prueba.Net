CREATE TABLE [PORTAL].[DataStopWatch] (
    [Id]                  INT      IDENTITY (1, 1) NOT NULL,
    [ClienteCredencialId] INT      NULL,
    [TotalCalls]          INT      NOT NULL,
    [TotalCallsHour]      INT      NULL,
    [DateCall]            DATETIME NULL,
    [HourCall]            INT      NULL,
    [MinuteCall]          INT      NULL,
    [SecondMinute]        INT      NULL,
    [SecondHour]          INT      NULL,
    [dateHour]            DATETIME NULL,
    CONSTRAINT [PK__DataStop__3214EC072403673C] PRIMARY KEY CLUSTERED ([Id] ASC)
);

