CREATE TABLE [dbo].[AssessmentProblem] (
	[AssessmentProblemId] INT            IDENTITY (1, 1) NOT NULL,
	[AssessmentId]        INT            NOT NULL,
	[TreeId]              INT            NOT NULL,
	[ProblemType]         NVARCHAR (50)  NOT NULL,
	[NeedsHelp]           BIT            NOT NULL DEFAULT (0),
	[CreatedBy]           NVARCHAR (256) NOT NULL,
	[CreatedOn]           DATETIME2 (7)  NOT NULL,
	[ModifiedBy]          NVARCHAR (256) NOT NULL,
	[ModifiedOn]          DATETIME2 (7)  NOT NULL,
	CONSTRAINT [PK_AssessmentProblem] PRIMARY KEY CLUSTERED ([AssessmentProblemId] ASC)
);
