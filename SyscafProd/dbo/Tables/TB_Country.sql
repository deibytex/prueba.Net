CREATE TABLE [dbo].[TB_Country] (
    [countryIdS]  INT          IDENTITY (1, 1) NOT NULL,
    [descripcion] VARCHAR (50) COLLATE Modern_Spanish_CI_AS NOT NULL,
    CONSTRAINT [PK_TB_Country] PRIMARY KEY CLUSTERED ([countryIdS] ASC)
);

