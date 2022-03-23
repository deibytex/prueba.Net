CREATE TABLE [dbo].[TB_Viajes] (
    [ViajeId]                  BIGINT        NOT NULL,
    [SubViajeId]               BIGINT        NOT NULL,
    [AssetId]                  BIGINT        NOT NULL,
    [VehicleRegistrationPlate] VARCHAR (100) NOT NULL,
    [TripStart]                DATETIME      NOT NULL,
    [TripEnd]                  DATETIME      NOT NULL,
    [Longitud]                 INT           NULL,
    [Latitud]                  INT           NULL,
    [FormattedAdress]          VARCHAR (250) NULL,
    [FechaSistema]             DATETIME      NOT NULL,
    CONSTRAINT [IX_TB_Viajes] UNIQUE CLUSTERED ([ViajeId] ASC, [SubViajeId] ASC)
);

