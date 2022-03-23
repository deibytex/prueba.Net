CREATE TABLE [WS].[TB_ProcesoGeneracionDatos] (
    [ProcesoGeneracionDatosId]  INT          IDENTITY (1, 1) NOT NULL,
    [ServicioId]                INT          NOT NULL,
    [FechaGeneracion]           DATETIME     NOT NULL,
    [EstadoProcesoGeneracionId] INT          NOT NULL,
    [FechaSistema]              DATETIME     DEFAULT (getdate()) NOT NULL,
    [EsActivo]                  BIT          DEFAULT ((1)) NOT NULL,
    [FechaInicio]               DATETIME     NULL,
    [FechaFinal]                DATETIME     NULL,
    [UsuarioIdS]                INT          NULL,
    [Nombre]                    VARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([ProcesoGeneracionDatosId] ASC),
    CONSTRAINT [pk_ProcesoGeneracion_DetalleLista] FOREIGN KEY ([EstadoProcesoGeneracionId]) REFERENCES [dbo].[TB_DetalleListas] ([DetalleListaId]),
    CONSTRAINT [pk_ProcesoGeneracion_Servicio] FOREIGN KEY ([ServicioId]) REFERENCES [WS].[TB_Servicios] ([ServicioId]),
    UNIQUE NONCLUSTERED ([ServicioId] ASC, [FechaGeneracion] ASC)
);

