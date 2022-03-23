CREATE TABLE [SIG].[TB_Parametros] (
    [ParametroId]   INT           IDENTITY (1, 1) NOT NULL,
    [FallaSenialId] INT           NOT NULL,
    [ClienteIds]    INT           NOT NULL,
    [Descripcion]   VARCHAR (255) NULL,
    [EsActivo]      BIT           NOT NULL,
    [FechaSistema]  DATETIME      NOT NULL,
    CONSTRAINT [PK_TB_PARAMETROS] PRIMARY KEY CLUSTERED ([ParametroId] ASC),
    CONSTRAINT [FK_TB_Parametros_TB_Cliente] FOREIGN KEY ([ClienteIds]) REFERENCES [dbo].[TB_Cliente] ([clienteIdS])
);

