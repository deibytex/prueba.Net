CREATE TABLE [SIG].[TB_Condiciones] (
    [CondicionId]     INT             IDENTITY (1, 1) NOT NULL,
    [TipoValorId]     INT             NOT NULL,
    [EventTypeId]     VARCHAR (MAX)   NULL,
    [TipoCondicionId] INT             NOT NULL,
    [Valor]           DECIMAL (18)    NULL,
    [Ocurrencias]     INT             NULL,
    [Distancia]       DECIMAL (18, 2) NULL,
    [Tiempo]          INT             NULL,
    [ValorTrips]      VARCHAR (200)   NULL,
    [EsActivo]        BIT             NOT NULL,
    [FechaSistema]    DATETIME        NOT NULL,
    [OperadorId]      INT             NULL,
    [Clienteids]      VARCHAR (MAX)   NULL,
    [FallaSenialId]   INT             NULL,
    [Descripcion]     VARCHAR (MAX)   NULL,
    [CondTrips]       VARCHAR (MAX)   NULL,
    [CondEvent]       VARCHAR (MAX)   NULL,
    [CondRef]         INT             NULL,
    CONSTRAINT [PK_TB_CONDICIONES_test1] PRIMARY KEY CLUSTERED ([CondicionId] ASC)
);

