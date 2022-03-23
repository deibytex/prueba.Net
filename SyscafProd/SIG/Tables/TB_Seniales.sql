CREATE TABLE [SIG].[TB_Seniales] (
    [SenialId]     BIGINT        IDENTITY (1, 1) NOT NULL,
    [Nombre]       VARCHAR (255) NOT NULL,
    [Descripcion]  VARCHAR (255) NULL,
    [EsActivo]     BIT           CONSTRAINT [DF_TB_Seniales_EsActivo] DEFAULT ((1)) NOT NULL,
    [FechaSistema] DATETIME      NOT NULL,
    CONSTRAINT [PK_TB_Seniales] PRIMARY KEY CLUSTERED ([SenialId] ASC)
);

