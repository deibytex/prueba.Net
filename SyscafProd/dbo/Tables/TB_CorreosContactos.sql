CREATE TABLE [dbo].[TB_CorreosContactos] (
    [contactoIdS]    INT           IDENTITY (1, 1) NOT NULL,
    [usuarioIdS]     INT           NULL,
    [correoContacto] VARCHAR (100) NULL,
    [clienteIdS]     INT           NULL
);

