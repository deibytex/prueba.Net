CREATE TABLE [PORTAL].[ConfiguracionEventosIMG] (
    [ConfigId]     INT      IDENTITY (1, 1) NOT NULL,
    [ClienteIds]   INT      NOT NULL,
    [EventTypeId]  BIGINT   NOT NULL,
    [FechaSistema] DATETIME DEFAULT (getdate()) NOT NULL,
    [EsActivo]     BIT      DEFAULT ((1)) NOT NULL,
    [UsuarioId]    INT      NOT NULL,
    PRIMARY KEY CLUSTERED ([ConfigId] ASC)
);

