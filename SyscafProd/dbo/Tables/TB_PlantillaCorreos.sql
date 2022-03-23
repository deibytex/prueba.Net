CREATE TABLE [dbo].[TB_PlantillaCorreos] (
    [PlantillaCorreoId] INT           IDENTITY (1, 1) NOT NULL,
    [Sigla]             VARCHAR (50)  NOT NULL,
    [Nombre]            VARCHAR (150) NOT NULL,
    [Descripcion]       VARCHAR (500) NULL,
    [Asunto]            VARCHAR (200) NOT NULL,
    [Cuerpo]            VARCHAR (MAX) NOT NULL,
    [DynamicText]       VARCHAR (MAX) NULL,
    [FechaSistema]      DATETIME      CONSTRAINT [DF_TB_PlantillaCorreos_FechaSistema] DEFAULT (getdate()) NOT NULL,
    [EsActivo]          BIT           CONSTRAINT [DF_TB_PlantillaCorreos_EsActivo] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_TB_PlantillaCorreos] PRIMARY KEY CLUSTERED ([PlantillaCorreoId] ASC)
);

