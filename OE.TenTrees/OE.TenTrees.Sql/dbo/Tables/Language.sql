CREATE TABLE [dbo].[Language] (
    [LanguageId] INT            IDENTITY (1, 1) NOT NULL,
    [SiteId]     INT            NOT NULL,
    [Code]       NVARCHAR (10)  NOT NULL,
    [IsDefault]  BIT            NOT NULL,
    [CreatedBy]  NVARCHAR (256) NOT NULL,
    [CreatedOn]  DATETIME2 (7)  NOT NULL,
    [ModifiedBy] NVARCHAR (256) NOT NULL,
    [ModifiedOn] DATETIME2 (7)  NOT NULL,
    [Name]       NVARCHAR (100) NULL,
    CONSTRAINT [PK_Language] PRIMARY KEY CLUSTERED ([LanguageId] ASC),
    CONSTRAINT [FK_Language_Site] FOREIGN KEY ([SiteId]) REFERENCES [dbo].[Site] ([SiteId]) ON DELETE CASCADE
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Language]
    ON [dbo].[Language]([SiteId] ASC, [Code] ASC) WHERE ([SiteId] IS NOT NULL AND [Code] IS NOT NULL);

