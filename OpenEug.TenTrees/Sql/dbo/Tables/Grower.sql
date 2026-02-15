CREATE TABLE [dbo].[Grower] (
    [GrowerId]      INT            IDENTITY (1, 1) NOT NULL,
    [VillageId]     INT            NOT NULL,
    [MentorId]      INT            NULL,
    [GrowerName]    NVARCHAR (MAX) NOT NULL,
    [HouseNumber]   NVARCHAR (50)  NULL,
    [IdNumber]      NVARCHAR (50)  NULL,
    [BirthDate]     DATETIME2 (7)  NULL,
    [HouseholdSize] INT            NOT NULL,
    [OwnsHome]      BIT            NOT NULL,
    [Status]        INT            NOT NULL,
    [ExitDate]      DATETIME2 (7)  NULL,
    [ExitReason]    NVARCHAR (MAX) NULL,
    [CreatedBy]     NVARCHAR (256) NOT NULL,
    [CreatedOn]     DATETIME2 (7)  NOT NULL,
    [ModifiedBy]    NVARCHAR (256) NOT NULL,
    [ModifiedOn]    DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_Grower] PRIMARY KEY CLUSTERED ([GrowerId] ASC)
);
