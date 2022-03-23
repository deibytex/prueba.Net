CREATE TABLE [dbo].[TB_ListaClienteNotifacion] (
    [ListaClienteNotifacionId] INT           IDENTITY (1, 1) NOT NULL,
    [ClienteIds]               INT           NOT NULL,
    [NombreLista]              VARCHAR (100) NOT NULL,
    [EsActivo]                 BIT           CONSTRAINT [DF_ListaClienteNotifacion_EsActivo] DEFAULT ((1)) NOT NULL,
    [FechaSistema]             DATETIME      CONSTRAINT [DF_ListaClienteNotifacion_FechaSistema] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_ListaClienteNotifacion] PRIMARY KEY CLUSTERED ([ListaClienteNotifacionId] ASC)
);

