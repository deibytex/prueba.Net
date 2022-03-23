CREATE TABLE [NS].[TB_NotificacionesCorreo] (
    [NotificacionCorreoId] BIGINT        IDENTITY (1, 1) NOT NULL,
    [TipoNotificacionId]   INT           NOT NULL,
    [Descripcion]          VARCHAR (MAX) COLLATE Modern_Spanish_CI_AS NOT NULL,
    [ListaDistribucionId]  INT           NOT NULL,
    [EsNotificado]         BIT           CONSTRAINT [DF__TB_Notifi__EsNot__0C50D423] DEFAULT ((0)) NOT NULL,
    [FechaSistema]         DATETIME      CONSTRAINT [DF__TB_Notifi__Fecha__0D44F85C] DEFAULT (getdate()) NOT NULL,
    [EsActivo]             BIT           CONSTRAINT [DF__TB_Notifi__EsAct__0E391C95] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK__TB_Notif__720C7574DEC5B563] PRIMARY KEY CLUSTERED ([NotificacionCorreoId] ASC),
    CONSTRAINT [Pk_NotificacionesCorreo_DetalleListas] FOREIGN KEY ([TipoNotificacionId]) REFERENCES [dbo].[TB_DetalleListas] ([DetalleListaId])
);

