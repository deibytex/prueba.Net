CREATE TABLE [ADM].[TB_RolesUsuario] (
    [RolUsuarioId] INT      IDENTITY (1, 1) NOT NULL,
    [RolId]        INT      NOT NULL,
    [UsuarioId]    INT      NOT NULL,
    [FechaSistema] DATETIME NOT NULL,
    [EsActivo]     BIT      CONSTRAINT [DF__TB_RolesU__EsAct__27F8EE98] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK__TB_Roles__8EC839D809A5449E] PRIMARY KEY CLUSTERED ([RolUsuarioId] ASC),
    CONSTRAINT [FK__TB_RolesU__RolId__28ED12D1] FOREIGN KEY ([RolId]) REFERENCES [ADM].[TB_Roles] ([RolId])
);

