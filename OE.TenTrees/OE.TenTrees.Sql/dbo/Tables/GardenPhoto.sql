CREATE TABLE [dbo].[GardenPhoto] (
    [GardenPhotoId]     INT            IDENTITY (1, 1) NOT NULL,
    [GardenSiteId]      INT            NOT NULL,
    [Url]               NVARCHAR (500) NULL,
    [Caption]           NVARCHAR (500) NULL,
    [FileName]          NVARCHAR (255) NULL,
    [ContentType]       NVARCHAR (100) NULL,
    [FileSize]          INT            NULL,
    [PhotoType]         INT            NOT NULL DEFAULT (0),
    [PhotoDate]         DATETIME2 (7)  NOT NULL,
    [CreatedBy]         NVARCHAR (256) NOT NULL,
    [CreatedOn]         DATETIME2 (7)  NOT NULL,
    [ModifiedBy]        NVARCHAR (256) NOT NULL,
    [ModifiedOn]        DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_GardenPhoto] PRIMARY KEY CLUSTERED ([GardenPhotoId] ASC),
    CONSTRAINT [FK_GardenPhoto_GardenSite] FOREIGN KEY ([GardenSiteId]) REFERENCES [dbo].[GardenSite] ([GardenSiteId]) ON DELETE CASCADE
);