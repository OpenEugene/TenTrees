CREATE TABLE [dbo].[AssessmentNote] (
    [AssessmentNoteId] INT            IDENTITY (1, 1) NOT NULL,
    [AssessmentId]     INT            NOT NULL,
    [NoteType]         NVARCHAR (20)  NOT NULL,
    [Text]             NVARCHAR (MAX) NOT NULL,
    [CreatedBy]        NVARCHAR (256) NOT NULL,
    [CreatedOn]        DATETIME2 (7)  NOT NULL,
    [ModifiedBy]       NVARCHAR (256) NOT NULL,
    [ModifiedOn]       DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_AssessmentNote] PRIMARY KEY CLUSTERED ([AssessmentNoteId] ASC)
);
