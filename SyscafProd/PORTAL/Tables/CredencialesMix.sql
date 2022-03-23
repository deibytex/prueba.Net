CREATE TABLE [PORTAL].[CredencialesMix] (
    [Id]                  INT           IDENTITY (1, 1) NOT NULL,
    [ClienteId]           INT           NULL,
    [ClientSecret]        VARCHAR (100) NOT NULL,
    [ClientId]            VARCHAR (100) NOT NULL,
    [UserId]              VARCHAR (100) NOT NULL,
    [Password]            VARCHAR (100) NOT NULL,
    [KeyPassword]         VARCHAR (MAX) NOT NULL,
    [IvPassword]          VARCHAR (MAX) NOT NULL,
    [EsActivo]            BIT           DEFAULT ((1)) NOT NULL,
    [UltimaActualizacion] DATETIME      NOT NULL,
    [clientesId]          VARCHAR (200) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK__Credencia__Clien__361203C5] FOREIGN KEY ([ClienteId]) REFERENCES [dbo].[TB_Cliente] ([clienteIdS])
);

