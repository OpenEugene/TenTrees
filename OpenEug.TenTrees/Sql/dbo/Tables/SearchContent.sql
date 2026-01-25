CREATE TABLE [dbo].[SearchContent] (
    [SearchContentId]   INT            IDENTITY (1, 1) NOT NULL,
    [SiteId]            INT            NOT NULL,
    [EntityName]        NVARCHAR (50)  NOT NULL,
    [EntityId]          NVARCHAR (50)  NOT NULL,
    [Title]             NVARCHAR (200) NOT NULL,
    [Description]       NVARCHAR (MAX) NOT NULL,
    [Body]              NVARCHAR (MAX) NOT NULL,
    [Url]               NVARCHAR (500) NOT NULL,
    [Permissions]       NVARCHAR (100) NOT NULL,
    [ContentModifiedBy] NVARCHAR (256) NOT NULL,
    [ContentModifiedOn] DATETIME2 (7)  NOT NULL,
    [AdditionalContent] NVARCHAR (MAX) NOT NULL,
    [CreatedOn]         DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_SearchContent] PRIMARY KEY CLUSTERED ([SearchContentId] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_SearchContent]
    ON [dbo].[SearchContent]([SiteId] ASC, [EntityName] ASC, [EntityId] ASC) WHERE ([SiteId] IS NOT NULL AND [EntityName] IS NOT NULL AND [EntityId] IS NOT NULL);

