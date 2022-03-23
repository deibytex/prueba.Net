﻿CREATE TABLE [PORTAL].[TotalDistanciaRecorrida_0] (
    [TotalDistanciaId] INT             IDENTITY (1, 1) NOT NULL,
    [FechaHora]        DATETIME        NOT NULL,
    [AssetId]          BIGINT          NOT NULL,
    [DriverId]         BIGINT          NOT NULL,
    [Distancia]        DECIMAL (18, 1) NULL,
    [FechaSistema]     DATETIME        NOT NULL,
    PRIMARY KEY CLUSTERED ([TotalDistanciaId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [portal_TotalDistanciaRecorrida_0]
    ON [PORTAL].[TotalDistanciaRecorrida_0]([FechaHora] ASC)
    INCLUDE([AssetId], [DriverId]);

