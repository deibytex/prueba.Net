CREATE TABLE [dbo].[TB_Estados] (
    [estadoIdS] INT          IDENTITY (1, 1) NOT NULL,
    [estado]    VARCHAR (50) NULL,
    [tipoIdS]   INT          NULL,
    CONSTRAINT [FK_TB_Estados_TB_EstadosTipos] FOREIGN KEY ([tipoIdS]) REFERENCES [dbo].[TB_EstadosTipos] ([estadoTipoIdS])
);

