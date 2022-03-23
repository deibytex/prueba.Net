CREATE TABLE [WS].[TB_DetalleProcesoGeneracionDatos] (
    [DetalleProcesoGeneracionDatosId] INT           IDENTITY (1, 1) NOT NULL,
    [ProcesoGeneracionDatosId]        INT           NOT NULL,
    [Descripcion]                     VARCHAR (MAX) NOT NULL,
    [EstadoDetallenId]                INT           NOT NULL,
    [FechaSistema]                    DATETIME      DEFAULT (getdate()) NOT NULL,
    [ClienteIdS]                      INT           NULL,
    PRIMARY KEY CLUSTERED ([DetalleProcesoGeneracionDatosId] ASC),
    CONSTRAINT [FK__TB_Detall__Clien__3CBF0154] FOREIGN KEY ([ClienteIdS]) REFERENCES [dbo].[TB_Cliente] ([clienteIdS]),
    CONSTRAINT [pk_ProcesoGeneracion_detalleProceso] FOREIGN KEY ([ProcesoGeneracionDatosId]) REFERENCES [WS].[TB_ProcesoGeneracionDatos] ([ProcesoGeneracionDatosId])
);

