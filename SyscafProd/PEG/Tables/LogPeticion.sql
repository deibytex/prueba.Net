CREATE TABLE [PEG].[LogPeticion] (
    [Id]             BIGINT        IDENTITY (1, 1) NOT NULL,
    [Peticion]       VARCHAR (MAX) NOT NULL,
    [Respuesta]      VARCHAR (MAX) NULL,
    [FechaRespuesta] DATETIME      NULL,
    [FechaSistema]   DATETIME      NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

