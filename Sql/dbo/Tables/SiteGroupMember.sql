CREATE TABLE [dbo].[SiteGroupMember] (
    [SiteGroupMemberId] INT            IDENTITY (1, 1) NOT NULL,
    [SiteGroupId]       INT            NOT NULL,
    [SiteId]            INT            NOT NULL,
    [SynchronizedOn]    DATETIME2 (7)  NULL,
    [CreatedBy]         NVARCHAR (256) NOT NULL,
    [CreatedOn]         DATETIME2 (7)  NOT NULL,
    [ModifiedBy]        NVARCHAR (256) NOT NULL,
    [ModifiedOn]        DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_SiteGroupMember] PRIMARY KEY CLUSTERED ([SiteGroupMemberId] ASC),
    CONSTRAINT [FK_SiteGroupMember_Site] FOREIGN KEY ([SiteId]) REFERENCES [dbo].[Site] ([SiteId]) ON DELETE CASCADE,
    CONSTRAINT [FK_SiteGroupMember_SiteGroup] FOREIGN KEY ([SiteGroupId]) REFERENCES [dbo].[SiteGroup] ([SiteGroupId]) ON DELETE CASCADE
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_SiteGroupMember]
    ON [dbo].[SiteGroupMember]([SiteId] ASC, [SiteGroupId] ASC) WHERE ([SiteId] IS NOT NULL AND [SiteGroupId] IS NOT NULL);

