CREATE TABLE [dbo].[ApplicationReview] (
    [ReviewId]         INT             IDENTITY (1, 1) NOT NULL,
    [ApplicationId]    INT             NOT NULL,
    [ReviewerUserId]   NVARCHAR (256)  NULL,
    [ReviewDate]       DATETIME2 (7)   NOT NULL,
    [Decision]         INT             NOT NULL,
    [Summary]          NVARCHAR (1000) NULL,
    [Comments]         NVARCHAR (2000) NULL,
    [RequiresFollowUp] BIT             NOT NULL,
    [CreatedBy]        NVARCHAR (256)  NOT NULL,
    [CreatedOn]        DATETIME2 (7)   NOT NULL,
    [ModifiedBy]       NVARCHAR (256)  NOT NULL,
    [ModifiedOn]       DATETIME2 (7)   NOT NULL,
    CONSTRAINT [PK_ApplicationReview] PRIMARY KEY CLUSTERED ([ReviewId] ASC),
    CONSTRAINT [FK_ApplicationReview_Application] FOREIGN KEY ([ApplicationId]) REFERENCES [dbo].[TreePlantingApplication] ([ApplicationId]) ON DELETE CASCADE
);

