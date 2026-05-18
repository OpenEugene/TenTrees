CREATE TABLE [dbo].[Tree] (
	[TreeId]     INT            IDENTITY (1, 1) NOT NULL,
	[OrchardId]  INT            NOT NULL,
	[TreeTypeId] INT            NOT NULL,
	[Quantity]   INT            NOT NULL DEFAULT 1,
	[Notes]      NVARCHAR (500) NULL,
	[CreatedBy]  NVARCHAR (256) NOT NULL,
	[CreatedOn]  DATETIME2 (7)  NOT NULL,
	[ModifiedBy] NVARCHAR (256) NOT NULL,
	[ModifiedOn] DATETIME2 (7)  NOT NULL,
	CONSTRAINT [PK_Tree] PRIMARY KEY CLUSTERED ([TreeId] ASC)
);
