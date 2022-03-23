CREATE TABLE [dbo].[TB_HistorialAcumulado] (
    [historialAcumuladoIdS] INT          IDENTITY (1, 1) NOT NULL,
    [engineHours]           INT          NULL,
    [odometer]              REAL         NULL,
    [lastTrip]              DATETIME     NULL,
    [lastAVL]               DATETIME     NULL,
    [assetId]               VARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([historialAcumuladoIdS] ASC)
);

