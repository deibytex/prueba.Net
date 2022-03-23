CREATE TABLE [ITS].[ConfigurationPubSub] (
    [ConfigurationId] INT           IDENTITY (1, 1) NOT NULL,
    [Sigla]           VARCHAR (50)  NOT NULL,
    [Name]            VARCHAR (200) NOT NULL,
    [Value]           VARCHAR (500) NOT NULL,
    [UserId]          INT           NOT NULL,
    [Date]            DATETIME      DEFAULT (getutcdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([ConfigurationId] ASC)
);

