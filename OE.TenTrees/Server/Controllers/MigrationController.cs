using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oqtane.Controllers;
using OE.TenTrees.Repository;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Oqtane.Shared;

namespace OE.TenTrees.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MigrationController : ControllerBase
    {
        private readonly IDbContextFactory<Context> _contextFactory;

        public MigrationController(IDbContextFactory<Context> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        [HttpPost("force-monitoring-migration")]
        [Authorize(Roles = RoleNames.Registered)]
        public async Task<IActionResult> ForceMonitoringMigration()
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                
                // Check if MonitoringSession table exists
                var tableExists = await TableExists(context, "MonitoringSession");
                
                if (!tableExists)
                {
                    // Force create the monitoring tables manually
                    await CreateMonitoringTables(context);
                    
                    // Add the migration to history to prevent it from running again
                    await context.Database.ExecuteSqlRawAsync(
                        "INSERT INTO __EFMigrationsHistory (MigrationId, ProductVersion, AppliedDate, AppliedVersion) VALUES ({0}, {1}, {2}, {3})",
                        "OE.TenTrees.01.02.00.00", "9.0.8", DateTime.UtcNow, "6.2.0");
                    
                    return Ok(new { Success = true, Message = "Monitoring migration executed successfully" });
                }
                else
                {
                    return Ok(new { Success = true, Message = "Monitoring tables already exist" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Error = ex.Message });
            }
        }

        private async Task<bool> TableExists(Context context, string tableName)
        {
            try
            {
                var result = await context.Database.ExecuteSqlRawAsync(
                    "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = {0}", tableName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task CreateMonitoringTables(Context context)
        {
            var sql = @"
                -- Create MonitoringSession table
                CREATE TABLE [MonitoringSession] (
                    [MonitoringSessionId] int IDENTITY(1,1) NOT NULL,
                    [ApplicationId] int NOT NULL,
                    [ModuleId] int NOT NULL,
                    [SessionDate] datetime2 NOT NULL,
                    [ObserverUserId] nvarchar(256) NULL,
                    [EvaluatorName] nvarchar(200) NULL,
                    [BeneficiaryName] nvarchar(200) NULL,
                    [HouseNumber] nvarchar(100) NULL,
                    [IdOrBirthdate] nvarchar(100) NULL,
                    [WeatherConditions] nvarchar(500) NULL,
                    [Notes] nvarchar(2000) NULL,
                    [HasWaterInPlot] bit NULL,
                    [HasWaterCatchmentSystem] bit NULL,
                    [NumberOfExistingTrees] int NULL,
                    [NumberOfIndigenousTrees] int NULL,
                    [NumberOfFruitNutTrees] int NULL,
                    [HasSpaceForMoreTrees] bit NULL,
                    [IsPropertyFenced] bit NULL,
                    [HasCompostOrMulchResources] bit NULL,
                    [TreesPlanted] int NULL,
                    [TreesAlive] int NULL,
                    [TreesLookingHealthy] bit NULL,
                    [UsingChemicalFertilizers] bit NULL,
                    [UsingPesticides] bit NULL,
                    [TreesBeingMulched] bit NULL,
                    [MakingCompost] bit NULL,
                    [CollectingWater] bit NULL,
                    [ProblemsIdentified] nvarchar(2000) NULL,
                    [InterventionNeeded] nvarchar(2000) NULL,
                    [MortalityRate] decimal(5,2) NULL,
                    [MortalityReason] nvarchar(500) NULL,
                    [CreatedBy] nvarchar(256) NULL,
                    [CreatedOn] datetime2 NOT NULL,
                    [ModifiedBy] nvarchar(256) NULL,
                    [ModifiedOn] datetime2 NOT NULL,
                    CONSTRAINT [PK_MonitoringSession] PRIMARY KEY ([MonitoringSessionId]),
                    CONSTRAINT [FK_MonitoringSession_Application] FOREIGN KEY ([ApplicationId]) REFERENCES [TreePlantingApplication] ([ApplicationId]) ON DELETE CASCADE
                );

                -- Create MonitoringMetric table
                CREATE TABLE [MonitoringMetric] (
                    [MonitoringMetricId] int IDENTITY(1,1) NOT NULL,
                    [MonitoringSessionId] int NOT NULL,
                    [MetricType] int NOT NULL,
                    [Value] decimal(10,3) NULL,
                    [Unit] nvarchar(50) NULL,
                    [Notes] nvarchar(1000) NULL,
                    [CreatedBy] nvarchar(256) NULL,
                    [CreatedOn] datetime2 NOT NULL,
                    [ModifiedBy] nvarchar(256) NULL,
                    [ModifiedOn] datetime2 NOT NULL,
                    CONSTRAINT [PK_MonitoringMetric] PRIMARY KEY ([MonitoringMetricId]),
                    CONSTRAINT [FK_MonitoringMetric_Session] FOREIGN KEY ([MonitoringSessionId]) REFERENCES [MonitoringSession] ([MonitoringSessionId]) ON DELETE CASCADE
                );

                -- Create MonitoringPhoto table
                CREATE TABLE [MonitoringPhoto] (
                    [MonitoringPhotoId] int IDENTITY(1,1) NOT NULL,
                    [MonitoringSessionId] int NOT NULL,
                    [Url] nvarchar(500) NULL,
                    [Caption] nvarchar(500) NULL,
                    [FileName] nvarchar(255) NULL,
                    [ContentType] nvarchar(100) NULL,
                    [FileSize] int NULL,
                    [CreatedBy] nvarchar(256) NULL,
                    [CreatedOn] datetime2 NOT NULL,
                    [ModifiedBy] nvarchar(256) NULL,
                    [ModifiedOn] datetime2 NOT NULL,
                    CONSTRAINT [PK_MonitoringPhoto] PRIMARY KEY ([MonitoringPhotoId]),
                    CONSTRAINT [FK_MonitoringPhoto_Session] FOREIGN KEY ([MonitoringSessionId]) REFERENCES [MonitoringSession] ([MonitoringSessionId]) ON DELETE CASCADE
                );
            ";

            await context.Database.ExecuteSqlRawAsync(sql);
        }
    }
}