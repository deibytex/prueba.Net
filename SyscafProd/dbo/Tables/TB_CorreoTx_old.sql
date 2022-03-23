CREATE TABLE [dbo].[TB_CorreoTx_old] (
    [CorreoTxIdS]     INT          IDENTITY (1, 1) NOT NULL,
    [clienteIdS]      INT          NOT NULL,
    [siteIdS]         INT          NOT NULL,
    [correoPrincipal] VARCHAR (50) NOT NULL,
    [correosCopia]    VARCHAR (50) NOT NULL,
    [fechaSistema]    DATETIME     NOT NULL,
    [esActivo]        BIT          NOT NULL
);

