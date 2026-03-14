CREATE TABLE [dbo].[SearchContentWord] (
    [SearchContentWordId] INT           IDENTITY (1, 1) NOT NULL,
    [SearchContentId]     INT           NOT NULL,
    [SearchWordId]        INT           NOT NULL,
    [Count]               INT           NOT NULL,
    [CreatedOn]           DATETIME2 (7) NOT NULL,
    [ModifiedOn]          DATETIME2 (7) NOT NULL,
    CONSTRAINT [PK_SearchContentWord] PRIMARY KEY CLUSTERED ([SearchContentWordId] ASC),
    CONSTRAINT [FK_SearchContentWord_SearchContent] FOREIGN KEY ([SearchContentId]) REFERENCES [dbo].[SearchContent] ([SearchContentId]) ON DELETE CASCADE,
    CONSTRAINT [FK_SearchContentWord_SearchWord] FOREIGN KEY ([SearchWordId]) REFERENCES [dbo].[SearchWord] ([SearchWordId]) ON DELETE CASCADE
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_SearchContentWord]
    ON [dbo].[SearchContentWord]([SearchContentId] ASC, [SearchWordId] ASC) WHERE ([SearchContentId] IS NOT NULL AND [SearchWordId] IS NOT NULL);

