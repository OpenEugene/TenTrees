CREATE TABLE [dbo].[MentorProfile] (
    [MentorId]   NVARCHAR (256) NOT NULL,   -- Oqtane username (matches Grower.MentorId)
    [VillageId]  INT            NOT NULL,
    [CreatedBy]  NVARCHAR (256) NOT NULL,
    [CreatedOn]  DATETIME2 (7)  NOT NULL,
    [ModifiedBy] NVARCHAR (256) NOT NULL,
    [ModifiedOn] DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_MentorProfile] PRIMARY KEY CLUSTERED ([MentorId] ASC)
);
