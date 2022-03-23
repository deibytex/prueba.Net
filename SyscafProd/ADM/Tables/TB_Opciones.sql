CREATE TABLE [ADM].[TB_Opciones] (
    [OpcionId]              INT           IDENTITY (1, 1) NOT NULL,
    [Nombre]                VARCHAR (100) COLLATE Modern_Spanish_CI_AS NOT NULL,
    [Descripcion]           VARCHAR (200) COLLATE Modern_Spanish_CI_AS NULL,
    [Controlador]           VARCHAR (100) COLLATE Modern_Spanish_CI_AS NULL,
    [Accion]                VARCHAR (100) COLLATE Modern_Spanish_CI_AS NULL,
    [ParametrosAdicionales] VARCHAR (500) COLLATE Modern_Spanish_CI_AS NULL,
    [Logo]                  VARCHAR (100) COLLATE Modern_Spanish_CI_AS NULL,
    [Orden]                 INT           NOT NULL,
    [OpcionPadreId]         INT           NULL,
    [EsVisible]             BIT           NOT NULL,
    [FechaSistema]          DATETIME      NOT NULL,
    [EsActivo]              BIT           CONSTRAINT [DF__TB_Opcion__EsAct__2CBDA3B5] DEFAULT ((1)) NOT NULL,
    [OrganizacionId]        INT           NULL,
    CONSTRAINT [PK__TB_Opcio__77CD08635C38AF16] PRIMARY KEY CLUSTERED ([OpcionId] ASC),
    CONSTRAINT [FK__TB_Opcion__Opcio__2EA5EC27] FOREIGN KEY ([OpcionPadreId]) REFERENCES [ADM].[TB_Opciones] ([OpcionId]),
    CONSTRAINT [UQ__TB_Opcio__ABA992D63C33A300] UNIQUE NONCLUSTERED ([Nombre] ASC, [OpcionPadreId] ASC)
);

