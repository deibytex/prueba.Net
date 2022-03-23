CREATE TABLE [dbo].[TB_PreferenciasDescargarWS] (
    [PreferenciasIdS] INT           IDENTITY (1, 1) NOT NULL,
    [clienteIdS]      INT           NOT NULL,
    [eventTypeIdS]    INT           NULL,
    [usuarioIdS]      INT           NULL,
    [EsActivo]        BIT           NOT NULL,
    [FechaSistema]    DATETIME      NOT NULL,
    [TipoPreferencia] INT           NOT NULL,
    [EventTypeId]     BIGINT        NULL,
    [ClientesId]      VARCHAR (500) NULL,
    [isActive]        BIT           NULL,
    [Parametrizacion] VARCHAR (100) NULL,
    CONSTRAINT [PK_TB_PreferenciasDescargarWS] PRIMARY KEY CLUSTERED ([PreferenciasIdS] ASC),
    FOREIGN KEY ([eventTypeIdS]) REFERENCES [dbo].[TB_EventType] ([eventTypeIdS]),
    FOREIGN KEY ([usuarioIdS]) REFERENCES [dbo].[TB_Usuarios] ([usuarioIdS])
);

