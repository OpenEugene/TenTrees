CREATE TABLE [dbo].[SearchWord] (
    [SearchWordId] INT            IDENTITY (1, 1) NOT NULL,
    [Word]         NVARCHAR (255) NOT NULL,
    [CreatedOn]    DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_SearchWord] PRIMARY KEY CLUSTERED ([SearchWordId] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_SearchWord]
    ON [dbo].[SearchWord]([Word] ASC) WHERE ([Word] IS NOT NULL);

