CREATE TABLE [dbo].[TB_Modulo] (
    [moduloId]          INT           IDENTITY (1, 1) NOT NULL,
    [descripcionModulo] VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_TB_Modulo] PRIMARY KEY CLUSTERED ([moduloId] ASC)
);

