CREATE TABLE [dbo].[ApplicationDocument] (
    [DocumentId]    INT            IDENTITY (1, 1) NOT NULL,
    [ApplicationId] INT            NOT NULL,
    [DocumentType]  INT            NOT NULL,
    [FileName]      NVARCHAR (255) NOT NULL,
    [ContentType]   NVARCHAR (100) NULL,
    [FileSize]      BIGINT         NOT NULL,
    [Url]           NVARCHAR (500) NULL,
    [Caption]       NVARCHAR (500) NULL,
    [CreatedBy]     NVARCHAR (256) NOT NULL,
    [CreatedOn]     DATETIME2 (7)  NOT NULL,
    [ModifiedBy]    NVARCHAR (256) NOT NULL,
    [ModifiedOn]    DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_ApplicationDocument] PRIMARY KEY CLUSTERED ([DocumentId] ASC),
    CONSTRAINT [FK_ApplicationDocument_Application] FOREIGN KEY ([ApplicationId]) REFERENCES [dbo].[TreePlantingApplication] ([ApplicationId]) ON DELETE CASCADE
);

GO

