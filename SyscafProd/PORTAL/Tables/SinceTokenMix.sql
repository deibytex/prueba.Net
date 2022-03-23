CREATE TABLE [PORTAL].[SinceTokenMix] (
    [Id]                  INT           IDENTITY (1, 1) NOT NULL,
    [ClienteId]           INT           NULL,
    [Method]              VARCHAR (100) NOT NULL,
    [SinceToken]          VARCHAR (20)  NOT NULL,
    [UltimaActualizacion] DATETIME      NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK__SinceToke__Clien__37FA4C37] FOREIGN KEY ([ClienteId]) REFERENCES [dbo].[TB_Cliente] ([clienteIdS])
);

