CREATE TABLE [dbo].[TB_Operaciones] (
    [operacionesId]          INT           IDENTITY (1, 1) NOT NULL,
    [descripcionOperaciones] VARCHAR (100) NOT NULL,
    [moduloId]               INT           NOT NULL,
    CONSTRAINT [PK_TB_Operaciones] PRIMARY KEY CLUSTERED ([operacionesId] ASC),
    CONSTRAINT [FK_TB_Operaciones_TB_Modulo] FOREIGN KEY ([moduloId]) REFERENCES [dbo].[TB_Modulo] ([moduloId])
);

