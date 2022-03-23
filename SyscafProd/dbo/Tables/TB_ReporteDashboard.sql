CREATE TABLE [dbo].[TB_ReporteDashboard] (
    [ReporteDashboradID]   INT           IDENTITY (1, 1) NOT NULL,
    [ClienteIds]           INT           NOT NULL,
    [FechaReporte]         DATETIME      NULL,
    [FechaFinal]           DATETIME      NULL,
    [InformacionExtendida] VARCHAR (MAX) NULL,
    [DataReporte]          VARCHAR (MAX) NOT NULL,
    [FechaSistema]         DATETIME      NOT NULL,
    [EsActivo]             BIT           CONSTRAINT [DF_ReporteDashborad_EsActivo] DEFAULT ((1)) NOT NULL,
    [TipoReporteId]        INT           NULL,
    [Anio]                 INT           NULL,
    [Mes]                  INT           NULL,
    [Dia]                  INT           NULL,
    [TipoFrecuenciaId]     INT           NULL,
    CONSTRAINT [PK_ReporteDashborad] PRIMARY KEY CLUSTERED ([ReporteDashboradID] ASC),
    CONSTRAINT [FK_TB_ReporteDashboard_TB_ReporteDashboard] FOREIGN KEY ([TipoReporteId]) REFERENCES [dbo].[TB_DetalleListas] ([DetalleListaId]),
    CONSTRAINT [FK_TB_ReporteDashborad_TB_Cliente] FOREIGN KEY ([ClienteIds]) REFERENCES [dbo].[TB_Cliente] ([clienteIdS])
);


GO
CREATE NONCLUSTERED INDEX [ReporteDashboard_fecha]
    ON [dbo].[TB_ReporteDashboard]([ClienteIds] ASC, [FechaReporte] ASC);

