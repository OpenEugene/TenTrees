CREATE TABLE [dbo].[AssessmentPhoto] (
    [AssessmentPhotoId] INT            IDENTITY (1, 1) NOT NULL,
    [AssessmentId]      INT            NOT NULL,
    [Url]               NVARCHAR (500) NULL,
    [Caption]           NVARCHAR (500) NULL,
    [CreatedBy]         NVARCHAR (256) NOT NULL,
    [CreatedOn]         DATETIME2 (7)  NOT NULL,
    [ModifiedBy]        NVARCHAR (256) NOT NULL,
    [ModifiedOn]        DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_AssessmentPhoto] PRIMARY KEY CLUSTERED ([AssessmentPhotoId] ASC),
    CONSTRAINT [FK_AssessmentPhoto_Assessment] FOREIGN KEY ([AssessmentId]) REFERENCES [dbo].[SiteAssessment] ([AssessmentId]) ON DELETE CASCADE
);