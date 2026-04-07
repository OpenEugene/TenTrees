CREATE TABLE [dbo].[TreeType] (
    [TreeTypeId]  INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (200) NOT NULL,
    [Category]    NVARCHAR (200) NOT NULL,
    [SortOrder]   INT            NOT NULL DEFAULT 0,
    [IsActive]    BIT            NOT NULL DEFAULT 1,
    [CreatedBy]   NVARCHAR (256) NOT NULL,
    [CreatedOn]   DATETIME2 (7)  NOT NULL,
    [ModifiedBy]  NVARCHAR (256) NOT NULL,
    [ModifiedOn]  DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_TreeType] PRIMARY KEY CLUSTERED ([TreeTypeId] ASC)
);
