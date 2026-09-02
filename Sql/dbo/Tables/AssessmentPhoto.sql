CREATE TABLE [dbo].[AssessmentPhoto] (
    [AssessmentPhotoId] INT             IDENTITY (1, 1) NOT NULL,
    [AssessmentId]      INT             NOT NULL,
    [PhotoId]           INT             NOT NULL,
    [Url]               NVARCHAR (2048) NOT NULL,
    [CreatedBy]         NVARCHAR (256)  NOT NULL,
    [CreatedOn]         DATETIME2 (7)   NOT NULL,
    [ModifiedBy]        NVARCHAR (256)  NOT NULL,
    [ModifiedOn]        DATETIME2 (7)   NOT NULL,
    CONSTRAINT [PK_AssessmentPhoto] PRIMARY KEY CLUSTERED ([AssessmentPhotoId] ASC),
    CONSTRAINT [UQ_AssessmentPhoto_AssessmentId_PhotoId] UNIQUE ([AssessmentId], [PhotoId])
);

GO

CREATE NONCLUSTERED INDEX [IX_AssessmentPhoto_AssessmentId]
    ON [dbo].[AssessmentPhoto] ([AssessmentId] ASC);
