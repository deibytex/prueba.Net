CREATE TABLE [ADM].[TB_RolOpcionOperaciones] (
    [RolOpcionOperacionId] INT      IDENTITY (1, 1) NOT NULL,
    [RolOpcionId]          INT      NOT NULL,
    [OperacionOpcionId]    INT      NOT NULL,
    [FechaSistema]         DATETIME NOT NULL,
    [EsActivo]             BIT      CONSTRAINT [DF__TB_RolOpc__EsAct__3BFFE745] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK__TB_RolOp__8E081D431EF53E32] PRIMARY KEY CLUSTERED ([RolOpcionOperacionId] ASC),
    CONSTRAINT [FK__TB_RolOpc__Opera__3EDC53F0] FOREIGN KEY ([OperacionOpcionId]) REFERENCES [ADM].[TB_OperacionOpcion] ([OperacionOpcionId]),
    CONSTRAINT [FK__TB_RolOpc__RolOp__3DE82FB7] FOREIGN KEY ([RolOpcionId]) REFERENCES [ADM].[TB_RolesOpciones] ([RolOpcionId]),
    CONSTRAINT [UQ__TB_RolOp__469A170A4E277B4F] UNIQUE NONCLUSTERED ([RolOpcionId] ASC, [OperacionOpcionId] ASC)
);

