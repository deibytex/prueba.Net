CREATE TABLE [dbo].[TB_Cliente] (
    [clienteIdS]       INT           IDENTITY (1, 1) NOT NULL,
    [clienteNombre]    VARCHAR (100) COLLATE Modern_Spanish_CI_AS NOT NULL,
    [usuario]          INT           NOT NULL,
    [countryIdS]       INT           NOT NULL,
    [telefono]         INT           NOT NULL,
    [planComercial]    VARCHAR (100) COLLATE Modern_Spanish_CI_AS NULL,
    [nit]              VARCHAR (50)  COLLATE Modern_Spanish_CI_AS NULL,
    [fechaIngreso]     DATETIME      NOT NULL,
    [estadoClienteIdS] INT           NOT NULL,
    [clienteId]        VARCHAR (200) COLLATE Modern_Spanish_CI_AS NULL,
    [notificacion]     BIT           CONSTRAINT [DF_TB_Cliente_notificacion] DEFAULT ((0)) NOT NULL,
    [GeneraIMG]        BIT           NULL,
    [Trips]            BIT           CONSTRAINT [DF_TB_Cliente_Trips] DEFAULT ((0)) NOT NULL,
    [Metrics]          BIT           CONSTRAINT [DF_TB_Cliente_Metrics] DEFAULT ((0)) NOT NULL,
    [Event]            BIT           CONSTRAINT [DF_TB_Cliente_Event] DEFAULT ((0)) NOT NULL,
    [Position]         BIT           CONSTRAINT [DF_TB_Cliente_Position] DEFAULT ((0)) NOT NULL,
    [ActiveEvent]      BIT           NULL,
    CONSTRAINT [PK_TB_Cliente] PRIMARY KEY CLUSTERED ([clienteIdS] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [MixIndex]
    ON [dbo].[TB_Cliente]([clienteId] ASC);

