CREATE TABLE [TRACE].[Rendimiento] (
    [Id]                 INT             IDENTITY (1, 1) NOT NULL,
    [Fecha]              DATETIME        NOT NULL,
    [AssetIds]           INT             NOT NULL,
    [FuelUsedLitres]     DECIMAL (11, 4) NOT NULL,
    [HorasMotor]         DECIMAL (11, 4) NOT NULL,
    [DistanciaRecorrida] DECIMAL (11, 4) NOT NULL,
    [PorRalenti]         DECIMAL (11, 4) NOT NULL,
    [FechaSistema]       DATETIME        NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

