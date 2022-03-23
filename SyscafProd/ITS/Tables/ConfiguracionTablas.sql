CREATE TABLE [ITS].[ConfiguracionTablas] (
    [TablaId]           INT           IDENTITY (1, 1) NOT NULL,
    [TipoTablaId]       INT           NOT NULL,
    [TipoTrama]         INT           NULL,
    [Codigo]            VARCHAR (10)  NULL,
    [Nombre]            VARCHAR (100) NULL,
    [Campos]            VARCHAR (MAX) NOT NULL,
    [FechaSistema]      DATETIME      DEFAULT (getdate()) NOT NULL,
    [EsCabDetalle]      BIT           NULL,
    [CamposAdicionales] VARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([TablaId] ASC)
);

