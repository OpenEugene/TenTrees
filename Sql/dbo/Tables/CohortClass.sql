CREATE TABLE [dbo].[CohortClass] (
    [CohortClassId]    INT           IDENTITY (1, 1) NOT NULL,
    [CohortId]         INT           NOT NULL,
    [TrainingClassId]  INT           NOT NULL,
    CONSTRAINT [PK_CohortClass] PRIMARY KEY CLUSTERED ([CohortClassId] ASC),
    CONSTRAINT [FK_CohortClass_Cohort] FOREIGN KEY ([CohortId]) REFERENCES [dbo].[Cohort] ([CohortId]),
    CONSTRAINT [FK_CohortClass_TrainingClass] FOREIGN KEY ([TrainingClassId]) REFERENCES [dbo].[TrainingClass] ([TrainingClassId]),
    CONSTRAINT [UQ_CohortClass] UNIQUE ([CohortId], [TrainingClassId])
);
