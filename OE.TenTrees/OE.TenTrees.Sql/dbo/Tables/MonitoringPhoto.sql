CREATE TABLE [dbo].[MonitoringPhoto] (
    [MonitoringPhotoId]   INT            IDENTITY (1, 1) NOT NULL,
    [MonitoringSessionId] INT            NOT NULL,
    [Url]                 NVARCHAR (500) NULL,
    [Caption]             NVARCHAR (500) NULL,
    [FileName]            NVARCHAR (255) NULL,
    [ContentType]         NVARCHAR (100) NULL,
    [FileSize]            INT            NULL,
    [CreatedBy]           NVARCHAR (256) NULL,
    [CreatedOn]           DATETIME2 (7)  NOT NULL,
    [ModifiedBy]          NVARCHAR (256) NULL,
    [ModifiedOn]          DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_MonitoringPhoto] PRIMARY KEY CLUSTERED ([MonitoringPhotoId] ASC)
);

