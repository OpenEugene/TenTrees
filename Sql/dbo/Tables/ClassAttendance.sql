CREATE TABLE [dbo].[ClassAttendance] (
    [ClassAttendanceId] INT            IDENTITY (1, 1) NOT NULL,
    [TrainingClassId]   INT            NOT NULL,
    [GrowerId]          INT            NOT NULL,
    [IsPresent]         BIT            NOT NULL,
    [MarkedByUserId]    INT            NULL,
    [CreatedBy]         NVARCHAR (256) NOT NULL,
    [CreatedOn]         DATETIME2 (7)  NOT NULL,
    [ModifiedBy]        NVARCHAR (256) NOT NULL,
    [ModifiedOn]        DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_ClassAttendance] PRIMARY KEY CLUSTERED ([ClassAttendanceId] ASC),
    CONSTRAINT [FK_ClassAttendance_TrainingClass] FOREIGN KEY ([TrainingClassId]) REFERENCES [dbo].[TrainingClass] ([TrainingClassId]),
    CONSTRAINT [FK_ClassAttendance_Grower] FOREIGN KEY ([GrowerId]) REFERENCES [dbo].[Grower] ([GrowerId])
);
