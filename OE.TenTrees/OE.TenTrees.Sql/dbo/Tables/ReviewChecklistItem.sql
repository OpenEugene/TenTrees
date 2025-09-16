CREATE TABLE [dbo].[ReviewChecklistItem] (
    [ChecklistItemId] INT             IDENTITY (1, 1) NOT NULL,
    [ReviewId]        INT             NOT NULL,
    [Item]            NVARCHAR (500)  NOT NULL,
    [IsComplete]      BIT             NOT NULL,
    [Notes]           NVARCHAR (1000) NULL,
    [CreatedBy]       NVARCHAR (256)  NOT NULL,
    [CreatedOn]       DATETIME2 (7)   NOT NULL,
    [ModifiedBy]      NVARCHAR (256)  NOT NULL,
    [ModifiedOn]      DATETIME2 (7)   NOT NULL,
    CONSTRAINT [PK_ReviewChecklistItem] PRIMARY KEY CLUSTERED ([ChecklistItemId] ASC),
    CONSTRAINT [FK_ReviewChecklistItem_Review] FOREIGN KEY ([ReviewId]) REFERENCES [dbo].[ApplicationReview] ([ReviewId]) ON DELETE CASCADE
);

