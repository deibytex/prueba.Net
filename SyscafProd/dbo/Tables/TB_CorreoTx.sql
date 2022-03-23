CREATE TABLE [dbo].[TB_CorreoTx] (
    [CorreoTxIdS]              INT          IDENTITY (1, 1) NOT NULL,
    [ListaClienteNotifacionId] INT          NOT NULL,
    [UsuarioIds]               INT          NULL,
    [Correo]                   VARCHAR (50) NOT NULL,
    [TipoCorreo]               INT          NOT NULL,
    [FechaSistema]             DATETIME     NOT NULL,
    [EsActivo]                 BIT          NOT NULL,
    CONSTRAINT [PK_TB_CorreoTx] PRIMARY KEY CLUSTERED ([CorreoTxIdS] ASC)
);

