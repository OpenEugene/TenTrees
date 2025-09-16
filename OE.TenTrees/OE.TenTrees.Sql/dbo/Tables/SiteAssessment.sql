-- Placeholder

CREATE TABLE [dbo].[SiteAssessment] (
    [AssessmentId]       INT             IDENTITY (1, 1) NOT NULL,
    [ApplicationId]      INT             NOT NULL,
    [AssessmentDate]     DATETIME2 (7)   NOT NULL,
    [AssessorUserId]     NVARCHAR (256)  NULL,
    [Outcome]            INT             NOT NULL,
    [SoilDescription]    NVARCHAR (1000) NULL,
    [SunExposure]        NVARCHAR (500)  NULL,
    [WaterAvailability]  NVARCHAR (500)  NULL,
    [Constraints]        NVARCHAR (1000) NULL,
    [RecommendedSpecies] NVARCHAR (1000) NULL,
    [RecommendedTreeCount] INT           NOT NULL,
    [Notes]              NVARCHAR (2000) NULL,
    [CreatedBy]          NVARCHAR (256)  NOT NULL,
    [CreatedOn]          DATETIME2 (7)   NOT NULL,
    [ModifiedBy]         NVARCHAR (256)  NOT NULL,
    [ModifiedOn]         DATETIME2 (7)   NOT NULL,
    CONSTRAINT [PK_SiteAssessment] PRIMARY KEY CLUSTERED ([AssessmentId] ASC),
    CONSTRAINT [FK_SiteAssessment_Application] FOREIGN KEY ([ApplicationId]) REFERENCES [dbo].[TreePlantingApplication] ([ApplicationId]) ON DELETE CASCADE
);