CREATE TABLE [dbo].[TreePlanting] (
    [TreePlantingId]    INT             IDENTITY (1, 1) NOT NULL,
    [GardenSiteId]      INT             NOT NULL,
    [TreeSpecies]       NVARCHAR (200)  NULL,
    [TreeVariety]       NVARCHAR (200)  NULL,
    [Quantity]          INT             NOT NULL,
    [PlantingDate]      DATETIME2 (7)   NOT NULL,
    [PlantingLocation]  NVARCHAR (500)  NULL,
    [Status]            INT             NOT NULL DEFAULT (0),
    [Notes]             NVARCHAR (1000) NULL,
    [CreatedBy]         NVARCHAR (256)  NOT NULL,
    [CreatedOn]         DATETIME2 (7)   NOT NULL,
    [ModifiedBy]        NVARCHAR (256)  NOT NULL,
    [ModifiedOn]        DATETIME2 (7)   NOT NULL,
    CONSTRAINT [PK_TreePlanting] PRIMARY KEY CLUSTERED ([TreePlantingId] ASC),
    CONSTRAINT [FK_TreePlanting_GardenSite] FOREIGN KEY ([GardenSiteId]) REFERENCES [dbo].[GardenSite] ([GardenSiteId]) ON DELETE CASCADE
);