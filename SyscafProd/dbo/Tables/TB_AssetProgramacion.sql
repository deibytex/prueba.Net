CREATE TABLE [dbo].[TB_AssetProgramacion] (
    [AssetProgramacionId]      INT      IDENTITY (1, 1) NOT NULL,
    [ProcesoGeneracionDatosId] INT      NOT NULL,
    [AssetIdS]                 INT      NOT NULL,
    [FechaSistema]             DATETIME NOT NULL,
    [EsActivo]                 BIT      NOT NULL,
    PRIMARY KEY CLUSTERED ([AssetProgramacionId] ASC),
    CONSTRAINT [FK_TB_AssetProgramacion_TB_Assets] FOREIGN KEY ([AssetIdS]) REFERENCES [dbo].[TB_Assets] ([assetIdS]),
    CONSTRAINT [FK_TB_AssetProgramacion_TB_ProcesoGeneracionDatos] FOREIGN KEY ([ProcesoGeneracionDatosId]) REFERENCES [WS].[TB_ProcesoGeneracionDatos] ([ProcesoGeneracionDatosId])
);

