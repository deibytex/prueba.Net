CREATE TABLE [ADM].[TB_RolesOpciones] (
    [RolOpcionId]    INT      IDENTITY (1, 1) NOT NULL,
    [RolId]          INT      NOT NULL,
    [OpcionId]       INT      NOT NULL,
    [OrganizacionId] INT      NOT NULL,
    [FechaSistema]   DATETIME NOT NULL,
    [EsActivo]       BIT      CONSTRAINT [DF__TB_RolesO__EsAct__3552E9B6] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK__TB_Roles__A702C5AB10114C2A] PRIMARY KEY CLUSTERED ([RolOpcionId] ASC),
    CONSTRAINT [FK__TB_RolesO__Opcio__382F5661] FOREIGN KEY ([OpcionId]) REFERENCES [ADM].[TB_Opciones] ([OpcionId]),
    CONSTRAINT [FK__TB_RolesO__Organ__39237A9A] FOREIGN KEY ([OrganizacionId]) REFERENCES [ADM].[TB_Organizacion] ([OrganizacionId]),
    CONSTRAINT [FK__TB_RolesO__RolId__373B3228] FOREIGN KEY ([RolId]) REFERENCES [ADM].[TB_Roles] ([RolId]),
    CONSTRAINT [UQ__TB_Roles__1004F5F750CB0214] UNIQUE NONCLUSTERED ([RolId] ASC, [OpcionId] ASC, [OrganizacionId] ASC)
);

