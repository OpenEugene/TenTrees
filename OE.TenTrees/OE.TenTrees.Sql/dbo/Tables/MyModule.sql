CREATE TABLE [dbo].[MyModule] (
    [MyModuleId] INT            IDENTITY (1, 1) NOT NULL,
    [ModuleId]   INT            NOT NULL,
    [Name]       NVARCHAR (MAX) NOT NULL,
    [CreatedBy]  NVARCHAR (256) NOT NULL,
    [CreatedOn]  DATETIME2 (7)  NOT NULL,
    [ModifiedBy] NVARCHAR (256) NOT NULL,
    [ModifiedOn] DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_MyModule] PRIMARY KEY CLUSTERED ([MyModuleId] ASC),
    CONSTRAINT [FK_MyModule_Module] FOREIGN KEY ([ModuleId]) REFERENCES [dbo].[Module] ([ModuleId]) ON DELETE CASCADE
);

