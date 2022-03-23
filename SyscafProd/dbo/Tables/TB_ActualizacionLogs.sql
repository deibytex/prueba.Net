CREATE TABLE [dbo].[TB_ActualizacionLogs] (
    [idLog]      INT           IDENTITY (1, 1) NOT NULL,
    [error]      VARCHAR (MAX) NULL,
    [indicador1] INT           NULL,
    [indicador2] INT           NULL,
    [indicador3] INT           NULL,
    [indicador4] INT           NULL,
    [clienteId]  VARCHAR (50)  NULL,
    [assetId]    VARCHAR (50)  NULL,
    [fechaError] DATETIME      NULL,
    [origen]     VARCHAR (50)  NULL,
    CONSTRAINT [PK_TB_ActualizacionLogs] PRIMARY KEY CLUSTERED ([idLog] ASC)
);

