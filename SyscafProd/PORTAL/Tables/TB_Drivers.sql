CREATE TABLE [PORTAL].[TB_Drivers] (
    [DriverId]             BIGINT        NOT NULL,
    [fmDriverId]           INT           NOT NULL,
    [extendedDriverIdType] VARCHAR (150) NULL,
    [employeeNumber]       VARCHAR (100) NULL,
    [name]                 VARCHAR (200) NULL,
    [siteIdS]              INT           NULL,
    [ClienteIds]           INT           NULL,
    [FechaSistema]         DATETIME      NULL,
    [aditionalFields]      VARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([DriverId] ASC),
    CONSTRAINT [FK__TB_Driver__Clien__4924D839] FOREIGN KEY ([ClienteIds]) REFERENCES [dbo].[TB_Cliente] ([clienteIdS]),
    CONSTRAINT [FK_TB_Drivers_TB_Site] FOREIGN KEY ([siteIdS]) REFERENCES [dbo].[TB_Site] ([siteIdS])
);

