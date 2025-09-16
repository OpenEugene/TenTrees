CREATE TABLE [dbo].[MonitoringMetric] (
    [MonitoringMetricId]  INT             IDENTITY (1, 1) NOT NULL,
    [MonitoringSessionId] INT             NOT NULL,
    [MetricType]          INT             NOT NULL,
    [Value]               DECIMAL (10, 3) NULL,
    [Unit]                NVARCHAR (50)   NULL,
    [Notes]               NVARCHAR (1000) NULL,
    [CreatedBy]           NVARCHAR (256)  NULL,
    [CreatedOn]           DATETIME2 (7)   NOT NULL,
    [ModifiedBy]          NVARCHAR (256)  NULL,
    [ModifiedOn]          DATETIME2 (7)   NOT NULL,
    CONSTRAINT [PK_MonitoringMetric] PRIMARY KEY CLUSTERED ([MonitoringMetricId] ASC)
    );

