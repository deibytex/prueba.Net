CREATE TABLE [dbo].[TB_Cargos] (
    [cargosIdS]   INT           IDENTITY (1, 1) NOT NULL,
    [nombre]      VARCHAR (50)  NOT NULL,
    [descripcion] VARCHAR (200) NOT NULL,
    CONSTRAINT [PK_TB_Cargos] PRIMARY KEY CLUSTERED ([cargosIdS] ASC)
);

