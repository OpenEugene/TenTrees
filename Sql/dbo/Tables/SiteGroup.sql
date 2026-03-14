CREATE TABLE [dbo].[SiteGroup] (
    [SiteGroupId]   INT            IDENTITY (1, 1) NOT NULL,
    [Name]          NVARCHAR (200) NOT NULL,
    [Type]          NVARCHAR (50)  NOT NULL,
    [PrimarySiteId] INT            NOT NULL,
    [Synchronize]   BIT            NOT NULL,
    [CreatedBy]     NVARCHAR (256) NOT NULL,
    [CreatedOn]     DATETIME2 (7)  NOT NULL,
    [ModifiedBy]    NVARCHAR (256) NOT NULL,
    [ModifiedOn]    DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_SiteGroup] PRIMARY KEY CLUSTERED ([SiteGroupId] ASC)
);

