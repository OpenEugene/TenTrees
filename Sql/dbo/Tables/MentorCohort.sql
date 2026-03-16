CREATE TABLE [dbo].[MentorCohort] (
    [MentorCohortId] INT            IDENTITY (1, 1) NOT NULL,
    [MentorId]       NVARCHAR (256) NOT NULL,   -- mentor username
    [CohortId]       INT            NOT NULL,
    [AssignedOn]     DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_MentorCohort] PRIMARY KEY CLUSTERED ([MentorCohortId] ASC),
    CONSTRAINT [UQ_MentorCohort] UNIQUE ([MentorId], [CohortId])
);
