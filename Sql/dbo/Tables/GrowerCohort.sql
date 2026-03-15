CREATE TABLE [dbo].[GrowerCohort] (
    [GrowerCohortId] INT           IDENTITY (1, 1) NOT NULL,
    [GrowerId]       INT           NOT NULL,
    [CohortId]       INT           NOT NULL,
    [JoinedOn]       DATETIME2 (7) NOT NULL,
    CONSTRAINT [PK_GrowerCohort] PRIMARY KEY CLUSTERED ([GrowerCohortId] ASC),
    CONSTRAINT [FK_GrowerCohort_Grower] FOREIGN KEY ([GrowerId]) REFERENCES [dbo].[Grower] ([GrowerId]),
    CONSTRAINT [FK_GrowerCohort_Cohort] FOREIGN KEY ([CohortId]) REFERENCES [dbo].[Cohort] ([CohortId]),
    CONSTRAINT [UQ_GrowerCohort] UNIQUE ([GrowerId], [CohortId])
);
