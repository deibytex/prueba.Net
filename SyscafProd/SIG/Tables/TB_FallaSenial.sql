CREATE TABLE [SIG].[TB_FallaSenial] (
    [FallaSenialId] INT           IDENTITY (1, 1) NOT NULL,
    [Nombre]        VARCHAR (255) NOT NULL,
    [Descripcion]   VARCHAR (255) NULL,
    [EsActivo]      BIT           CONSTRAINT [DF_TB_FallaSenial_EsActivo] DEFAULT ((1)) NOT NULL,
    [SenialId]      BIGINT        NOT NULL,
    [FechaSistema]  DATETIME      NOT NULL,
    CONSTRAINT [PK_TB_FallaSenial] PRIMARY KEY CLUSTERED ([FallaSenialId] ASC)
);

