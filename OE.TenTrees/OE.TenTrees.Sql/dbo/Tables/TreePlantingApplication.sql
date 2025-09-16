CREATE TABLE [dbo].[TreePlantingApplication] (
    [ApplicationId]             INT             IDENTITY (1, 1) NOT NULL,
    [ModuleId]                  INT             NOT NULL,
    [EvaluatorName]             NVARCHAR (200)  NULL,
    [ApplicantUserId]           NVARCHAR (256)  NULL,
    [BeneficiaryName]           NVARCHAR (200)  NULL,
    [HouseholdIdentifier]       NVARCHAR (100)  NULL,
    [Address]                   NVARCHAR (500)  NULL,
    [Village]                   NVARCHAR (200)  NULL,
    [HouseholdSize]             INT             NULL,
    [SubmissionDate]            DATETIME2 (7)   NULL,
    [HasExistingGardenInterest] BIT             NOT NULL,
    [GardenCurrentlyTended]     BIT             NOT NULL,
    [ChildHeadedHousehold]      BIT             NOT NULL,
    [WomanHeadedHousehold]      BIT             NOT NULL,
    [EmptyOrNearlyEmptyYard]    BIT             NOT NULL,
    [CommitNoChemicals]         BIT             NOT NULL,
    [CommitAttendTraining]      BIT             NOT NULL,
    [CommitNotCutTrees]         BIT             NOT NULL,
    [CommitStandAgainstAbuse]   BIT             NOT NULL,
    [Status]                    INT             NOT NULL,
    [RejectionReason]           NVARCHAR (1000) NULL,
    [Notes]                     NVARCHAR (2000) NULL,
    [CreatedBy]                 NVARCHAR (256)  NOT NULL,
    [CreatedOn]                 DATETIME2 (7)   NOT NULL,
    [ModifiedBy]                NVARCHAR (256)  NOT NULL,
    [ModifiedOn]                DATETIME2 (7)   NOT NULL,
    CONSTRAINT [PK_TreePlantingApplication] PRIMARY KEY CLUSTERED ([ApplicationId] ASC),
    CONSTRAINT [FK_TreePlantingApplication_Module] FOREIGN KEY ([ModuleId]) REFERENCES [dbo].[Module] ([ModuleId]) ON DELETE CASCADE
);



