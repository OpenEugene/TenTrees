using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using OE.TenTrees.Migrations.EntityBuilders;
using OE.TenTrees.Repository;

namespace OE.TenTrees.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("OE.TenTrees.01.02.00.00")]
    public class AddMonitoringTables : MultiDatabaseMigration
    {
        public AddMonitoringTables(IDatabase database) : base(database)
        {
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create MonitoringSession table first (parent table)
            var monitoringSessionEntityBuilder = new MonitoringSessionEntityBuilder(migrationBuilder, ActiveDatabase);
            monitoringSessionEntityBuilder.Create();

            // Create child tables that reference MonitoringSession
            var monitoringMetricEntityBuilder = new MonitoringMetricEntityBuilder(migrationBuilder, ActiveDatabase);
            monitoringMetricEntityBuilder.Create();

            var monitoringPhotoEntityBuilder = new MonitoringPhotoEntityBuilder(migrationBuilder, ActiveDatabase);
            monitoringPhotoEntityBuilder.Create();
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop child tables first (in reverse order of creation)
            var monitoringPhotoEntityBuilder = new MonitoringPhotoEntityBuilder(migrationBuilder, ActiveDatabase);
            monitoringPhotoEntityBuilder.Drop();

            var monitoringMetricEntityBuilder = new MonitoringMetricEntityBuilder(migrationBuilder, ActiveDatabase);
            monitoringMetricEntityBuilder.Drop();

            // Drop parent table last
            var monitoringSessionEntityBuilder = new MonitoringSessionEntityBuilder(migrationBuilder, ActiveDatabase);
            monitoringSessionEntityBuilder.Drop();
        }
    }
}