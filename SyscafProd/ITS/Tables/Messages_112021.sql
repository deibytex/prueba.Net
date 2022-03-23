CREATE TABLE [ITS].[Messages_112021] (
    [FechaHora]      DATETIME      NOT NULL,
    [Mensaje]        VARCHAR (MAX) NULL,
    [EsProcesado]    BIT           DEFAULT ((0)) NOT NULL,
    [FechaProcesado] DATETIME      NULL,
    [ProfileData]    VARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([FechaHora] ASC)
);

