CREATE TABLE [dbo].[ApplicationStatusHistory] (
    [HistoryId]       INT             IDENTITY (1, 1) NOT NULL,
    [ApplicationId]   INT             NOT NULL,
    [Status]          INT             NOT NULL,
    [ChangedByUserId] NVARCHAR (256)  NULL,
    [Comment]         NVARCHAR (1000) NULL,
    [CreatedBy]       NVARCHAR (256)  NOT NULL,
    [CreatedOn]       DATETIME2 (7)   NOT NULL,
    [ModifiedBy]      NVARCHAR (256)  NOT NULL,
    [ModifiedOn]      DATETIME2 (7)   NOT NULL,
    CONSTRAINT [PK_ApplicationStatusHistory] PRIMARY KEY CLUSTERED ([HistoryId] ASC),
    CONSTRAINT [FK_ApplicationStatusHistory_Application] FOREIGN KEY ([ApplicationId]) REFERENCES [dbo].[TreePlantingApplication] ([ApplicationId]) ON DELETE CASCADE
);

