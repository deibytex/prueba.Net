CREATE TABLE [SIG].[TB_DataSeniales] (
    [ReporteSenialId] INT          IDENTITY (1, 1) NOT NULL,
    [CondicionId]     INT          NOT NULL,
    [FallaSenialId]   INT          NOT NULL,
    [Descripcion]     VARCHAR (50) NOT NULL,
    [AssetIds]        BIGINT       NOT NULL,
    [ClienteIds]      INT          NOT NULL,
    [FechaInicial]    DATETIME     NOT NULL,
    [FechaFinal]      DATETIME     NOT NULL,
    [Ocurrencias]     INT          NOT NULL,
    [FechaSistema]    DATETIME     NOT NULL,
    [EsActivo]        BIT          NOT NULL,
    PRIMARY KEY CLUSTERED ([ReporteSenialId] ASC),
    CONSTRAINT [FK_TB_DataSeniales_TB_Cliente] FOREIGN KEY ([ClienteIds]) REFERENCES [dbo].[TB_Cliente] ([clienteIdS])
);


GO
CREATE NONCLUSTERED INDEX [indx_cliente_TB_DataSeniales]
    ON [SIG].[TB_DataSeniales]([FallaSenialId] ASC)
    INCLUDE([ClienteIds], [FechaInicial], [FechaFinal]);

