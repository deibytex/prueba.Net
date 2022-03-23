CREATE TABLE [dbo].[TB_GruposDeSeguridad] (
    [GrupoSeguridadId] INT           IDENTITY (1, 1) NOT NULL,
    [clienteIdS]       INT           NULL,
    [NombreGrupo]      VARCHAR (200) NOT NULL,
    [Descripcion]      VARCHAR (500) NULL,
    [FechaSistema]     DATETIME      NULL,
    [EsActivo]         BIT           CONSTRAINT [DF_TB_GruposDeSeguridad_EsActivo] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_TB_GruposDeSeguridad] PRIMARY KEY CLUSTERED ([GrupoSeguridadId] ASC)
);

