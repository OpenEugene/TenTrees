CREATE TABLE [dbo].[MentorCohort] (
    [MentorCohortId] INT            IDENTITY (1, 1) NOT NULL,
    [MentorId]       NVARCHAR (256) NOT NULL,
    [CohortId]       INT            NOT NULL,
    [AssignedOn]     DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_MentorCohort] PRIMARY KEY CLUSTERED ([MentorCohortId] ASC),
    CONSTRAINT [FK_MentorCohort_Cohort] FOREIGN KEY ([CohortId]) REFERENCES [dbo].[Cohort] ([CohortId]),
    CONSTRAINT [UQ_MentorCohort] UNIQUE ([MentorId], [CohortId])
);
