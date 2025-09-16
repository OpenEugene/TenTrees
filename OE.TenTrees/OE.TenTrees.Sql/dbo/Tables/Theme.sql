CREATE TABLE [dbo].[Theme] (
    [ThemeId]    INT            IDENTITY (1, 1) NOT NULL,
    [ThemeName]  NVARCHAR (200) NOT NULL,
    [CreatedBy]  NVARCHAR (256) NOT NULL,
    [CreatedOn]  DATETIME2 (7)  NOT NULL,
    [ModifiedBy] NVARCHAR (256) NOT NULL,
    [ModifiedOn] DATETIME2 (7)  NOT NULL,
    [Name]       NVARCHAR (200) NULL,
    [Version]    NVARCHAR (50)  NULL,
    CONSTRAINT [PK_Theme] PRIMARY KEY CLUSTERED ([ThemeId] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Theme]
    ON [dbo].[Theme]([ThemeName] ASC) WHERE ([ThemeName] IS NOT NULL);

