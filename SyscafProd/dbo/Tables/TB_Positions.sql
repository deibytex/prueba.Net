CREATE TABLE [dbo].[TB_Positions] (
    [PositionIdS]                    INT             IDENTITY (1, 1) NOT NULL,
    [AgeOfReadingSeconds]            INT             NULL,
    [AltitudeMetres]                 INT             NULL,
    [AssetId]                        VARCHAR (50)    COLLATE Modern_Spanish_CI_AS NULL,
    [DistanceSinceReadingKilometres] DECIMAL (16, 4) NULL,
    [DriverId]                       VARCHAR (50)    COLLATE Modern_Spanish_CI_AS NULL,
    [FormattedAddress]               VARCHAR (50)    COLLATE Modern_Spanish_CI_AS NULL,
    [Hdop]                           BIT             NULL,
    [Heading]                        DECIMAL (16, 4) NULL,
    [IsAvl]                          BIT             NULL,
    [Latitude]                       DECIMAL (16, 4) NULL,
    [Longitude]                      DECIMAL (16, 4) NULL,
    [NumberOfSatellites]             INT             NULL,
    [OdometerKilometres]             DECIMAL (16, 4) NULL,
    [Pdop]                           BIT             NULL,
    [PositionId]                     VARCHAR (50)    COLLATE Modern_Spanish_CI_AS NULL,
    [SpeedKilometresPerHour]         DECIMAL (16, 4) NULL,
    [SpeedLimit]                     DECIMAL (16, 4) NULL,
    [Timestamp]                      DATETIME        NULL,
    [Vdop]                           BIT             NULL,
    [assetPositionId]                VARCHAR (50)    COLLATE Modern_Spanish_CI_AS NULL,
    [assetIdS]                       INT             NULL,
    [driverIdS]                      INT             NULL,
    [estadoBase]                     BIT             NULL,
    [fechaSistema]                   DATETIME        NULL,
    [grupoIdS]                       INT             NULL,
    [ClienteIds]                     INT             NULL,
    CONSTRAINT [PK__TB_Posit__6CCE6F888E4496F1] PRIMARY KEY CLUSTERED ([PositionIdS] ASC)
);


GO
CREATE NONCLUSTERED INDEX [TB_Positions_Fechasistema]
    ON [dbo].[TB_Positions]([fechaSistema] ASC)
    INCLUDE([PositionId]);

