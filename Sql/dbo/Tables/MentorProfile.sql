CREATE TABLE [dbo].[MentorProfile] (
    [Username]   NVARCHAR (256) NOT NULL,
    [VillageId]  INT            NOT NULL,
    [CreatedBy]  NVARCHAR (256) NOT NULL,
    [CreatedOn]  DATETIME2 (7)  NOT NULL,
    [ModifiedBy] NVARCHAR (256) NOT NULL,
    [ModifiedOn] DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_MentorProfile] PRIMARY KEY CLUSTERED ([Username] ASC)
);
