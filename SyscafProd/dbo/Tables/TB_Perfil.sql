CREATE TABLE [dbo].[TB_Perfil] (
    [perfilIdS]         INT           IDENTITY (1, 1) NOT NULL,
    [descripcionPerfil] VARCHAR (100) COLLATE Modern_Spanish_CI_AS NOT NULL,
    CONSTRAINT [PK_TB_Perfil] PRIMARY KEY CLUSTERED ([perfilIdS] ASC)
);

