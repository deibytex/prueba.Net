CREATE TABLE [EBUS].[ConfiguracionDatatable] (
    [ConfiguracionDatatableId] INT           IDENTITY (1, 1) NOT NULL,
    [usuarioIdS]               INT           NOT NULL,
    [OpcionId]                 INT           NOT NULL,
    [IdTabla]                  VARCHAR (50)  NOT NULL,
    [Columna]                  INT           NOT NULL,
    [FechaSistema]             DATETIME      NOT NULL,
    [NombreReporte]            VARCHAR (100) NULL,
    [ReporteIdPBI]             VARCHAR (100) NULL,
    CONSTRAINT [PK_ConfiguracionDatatable] PRIMARY KEY CLUSTERED ([ConfiguracionDatatableId] ASC),
    CONSTRAINT [FK_ConfiguracionDatatable_TB_Opciones] FOREIGN KEY ([OpcionId]) REFERENCES [ADM].[TB_Opciones] ([OpcionId]),
    CONSTRAINT [FK_ConfiguracionDatatable_TB_Usuarios] FOREIGN KEY ([usuarioIdS]) REFERENCES [dbo].[TB_Usuarios] ([usuarioIdS])
);

