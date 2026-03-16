CREATE TABLE [dbo].[CohortClass] (
    [CohortClassId]    INT           IDENTITY (1, 1) NOT NULL,
    [CohortId]         INT           NOT NULL,
    [TrainingClassId]  INT           NOT NULL,
    CONSTRAINT [PK_CohortClass] PRIMARY KEY CLUSTERED ([CohortClassId] ASC),
    CONSTRAINT [UQ_CohortClass] UNIQUE ([CohortId], [TrainingClassId])
);
