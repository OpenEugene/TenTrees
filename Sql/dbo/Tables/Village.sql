CREATE TABLE [dbo].[Village] (
    [VillageId]    INT            IDENTITY (1, 1) NOT NULL,
    [VillageName]  NVARCHAR (MAX) NOT NULL,
    [ContactName]  NVARCHAR (200) NULL,
    [ContactPhone] NVARCHAR (50)  NULL,
    [ContactEmail] NVARCHAR (200) NULL,
    [Notes]        NVARCHAR (MAX) NULL,
    [IsActive]     BIT            NOT NULL,
    [CreatedBy]    NVARCHAR (256) NOT NULL,
    [CreatedOn]    DATETIME2 (7)  NOT NULL,
    [ModifiedBy]   NVARCHAR (256) NOT NULL,
    [ModifiedOn]   DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_Village] PRIMARY KEY CLUSTERED ([VillageId] ASC)
);


