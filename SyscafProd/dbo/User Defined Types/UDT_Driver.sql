CREATE TYPE [dbo].[UDT_Driver] AS TABLE (
    [driverId]             BIGINT        NOT NULL,
    [fmDriverId]           INT           NULL,
    [siteIdS]              INT           NULL,
    [name]                 VARCHAR (200) NULL,
    [employeeNumber]       VARCHAR (100) NULL,
    [extendedDriverIdType] VARCHAR (150) NULL,
    [aditionalFields]      VARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([driverId] ASC));

