CREATE TABLE [dbo].[TB_Log] (
    [logId]          INT           IDENTITY (1, 1) NOT NULL,
    [usuarioIdS]     INT           NOT NULL,
    [fechaRegistro]  DATETIME      NOT NULL,
    [descripcionLog] VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_TB_Log] PRIMARY KEY CLUSTERED ([logId] ASC)
);

