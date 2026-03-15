CREATE TABLE [dbo].[Cohort] (
    [CohortId]    INT            IDENTITY (1, 1) NOT NULL,
    [VillageId]   INT            NOT NULL,
    [Name]        NVARCHAR (200) NOT NULL,
    [Status]      INT            NOT NULL DEFAULT 0,
    [ActivatedOn] DATETIME2 (7)  NULL,
    [CreatedBy]   NVARCHAR (256) NOT NULL,
    [CreatedOn]   DATETIME2 (7)  NOT NULL,
    [ModifiedBy]  NVARCHAR (256) NOT NULL,
    [ModifiedOn]  DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_Cohort] PRIMARY KEY CLUSTERED ([CohortId] ASC),
    CONSTRAINT [FK_Cohort_Village] FOREIGN KEY ([VillageId]) REFERENCES [dbo].[Village] ([VillageId])
);
