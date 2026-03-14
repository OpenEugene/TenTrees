CREATE TABLE [dbo].[SiteTask] (
    [SiteTaskId]  INT            IDENTITY (1, 1) NOT NULL,
    [SiteId]      INT            NOT NULL,
    [Name]        NVARCHAR (200) NOT NULL,
    [Type]        NVARCHAR (200) NOT NULL,
    [Parameters]  NVARCHAR (MAX) NULL,
    [IsCompleted] BIT            NULL,
    [Status]      NVARCHAR (MAX) NULL,
    [CreatedBy]   NVARCHAR (256) NOT NULL,
    [CreatedOn]   DATETIME2 (7)  NOT NULL,
    [ModifiedBy]  NVARCHAR (256) NOT NULL,
    [ModifiedOn]  DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_SiteTask] PRIMARY KEY CLUSTERED ([SiteTaskId] ASC),
    CONSTRAINT [FK_SiteTask_Site] FOREIGN KEY ([SiteId]) REFERENCES [dbo].[Site] ([SiteId]) ON DELETE CASCADE
);

