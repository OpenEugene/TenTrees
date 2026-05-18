CREATE TABLE [dbo].[Orchard] (
	[OrchardId]  INT            IDENTITY (1, 1) NOT NULL,
	[GrowerId]   INT            NOT NULL,
	[Name]       NVARCHAR (200) NOT NULL,
	[Notes]      NVARCHAR (500) NULL,
	[CreatedBy]  NVARCHAR (256) NOT NULL,
	[CreatedOn]  DATETIME2 (7)  NOT NULL,
	[ModifiedBy] NVARCHAR (256) NOT NULL,
	[ModifiedOn] DATETIME2 (7)  NOT NULL,
	CONSTRAINT [PK_Orchard] PRIMARY KEY CLUSTERED ([OrchardId] ASC)
);
