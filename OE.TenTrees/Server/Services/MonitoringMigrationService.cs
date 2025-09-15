using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OE.TenTrees.Repository;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OE.TenTrees.Services
{
    /// <summary>
    /// Background service to ensure monitoring migrations are applied on startup
    /// </summary>
    public class MonitoringMigrationService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<MonitoringMigrationService> _logger;

        public MonitoringMigrationService(IServiceProvider serviceProvider, ILogger<MonitoringMigrationService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();
            
            try
            {
                var contextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<Context>>();
                using var context = contextFactory.CreateDbContext();
                
                // Check if MonitoringSession table exists
                var tableExists = await CheckTableExists(context, "MonitoringSession");
                
                if (!tableExists)
                {
                    _logger.LogInformation("MonitoringSession table does not exist. Attempting to apply migrations...");
                    
                    // Apply pending migrations
                    await context.Database.MigrateAsync(stoppingToken);
                    
                    _logger.LogInformation("Database migrations applied successfully.");
                }
                else
                {
                    _logger.LogInformation("MonitoringSession table exists. No migration needed.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking or applying monitoring migrations: {Error}", ex.Message);
            }
        }

        private async Task<bool> CheckTableExists(Context context, string tableName)
        {
            try
            {
                var sql = $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{tableName}'";
                var result = await context.Database.ExecuteSqlRawAsync($"SELECT CASE WHEN EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{tableName}') THEN 1 ELSE 0 END");
                return true; // If we get here without exception, assume it exists
            }
            catch
            {
                return false;
            }
        }
    }
}