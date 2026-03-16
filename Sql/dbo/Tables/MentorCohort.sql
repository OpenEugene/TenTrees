CREATE TABLE [dbo].[MentorCohort] (
    [MentorCohortId]  INT            IDENTITY (1, 1) NOT NULL,
    [MentorUsername]  NVARCHAR (256) NOT NULL,
    [CohortId]        INT            NOT NULL,
    [AssignedOn]      DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_MentorCohort] PRIMARY KEY CLUSTERED ([MentorCohortId] ASC),
    CONSTRAINT [UQ_MentorCohort] UNIQUE ([MentorUsername], [CohortId])
);
