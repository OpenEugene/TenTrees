CREATE TABLE [dbo].[AssessmentPhoto] (
    [AssessmentPhotoId] INT             IDENTITY (1, 1) NOT NULL,
    [AssessmentId]      INT             NOT NULL,
    [FileName]          NVARCHAR (255)  NOT NULL,
    [ContentType]       NVARCHAR (50)   NOT NULL,
    [FileSize]          BIGINT          NOT NULL,
    [PhotoData]         VARBINARY (MAX) NOT NULL,
    [CreatedBy]         NVARCHAR (256)  NOT NULL,
    [CreatedOn]         DATETIME2 (7)   NOT NULL,
    [ModifiedBy]        NVARCHAR (256)  NOT NULL,
    [ModifiedOn]        DATETIME2 (7)   NOT NULL,
    CONSTRAINT [PK_AssessmentPhoto] PRIMARY KEY CLUSTERED ([AssessmentPhotoId] ASC)
);

GO

CREATE NONCLUSTERED INDEX [IX_AssessmentPhoto_AssessmentId]
    ON [dbo].[AssessmentPhoto] ([AssessmentId] ASC);
