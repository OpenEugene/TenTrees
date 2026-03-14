CREATE TABLE [dbo].[TrainingClass] (
    [TrainingClassId] INT            IDENTITY (1, 1) NOT NULL,
    [ModuleId]        INT            NOT NULL,
    [VillageId]       INT            NOT NULL,
    [ClassName]       NVARCHAR (MAX) NOT NULL,
    [ClassDate]       DATETIME2 (7)  NOT NULL,
    [ClassNumber]     INT            NOT NULL,
    [Notes]           NVARCHAR (MAX) NULL,
    [CreatedBy]       NVARCHAR (256) NOT NULL,
    [CreatedOn]       DATETIME2 (7)  NOT NULL,
    [ModifiedBy]      NVARCHAR (256) NOT NULL,
    [ModifiedOn]      DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_TrainingClass] PRIMARY KEY CLUSTERED ([TrainingClassId] ASC),
    CONSTRAINT [FK_TrainingClass_Village] FOREIGN KEY ([VillageId]) REFERENCES [dbo].[Village] ([VillageId])
);
