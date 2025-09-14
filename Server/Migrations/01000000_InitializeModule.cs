using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using OE.TenTrees.Migrations.EntityBuilders;
using OE.TenTrees.Repository;

namespace OE.TenTrees.Migrations
{
    [DbContext(typeof(TenTreesContext))]
    [Migration("01.00.00.00")]
    public class InitializeModule : MultiDatabaseMigration
    {
        public InitializeModule(IDatabase database) : base(database)
        {
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var entityBuilder = new TreeEntityBuilder(migrationBuilder, ActiveDatabase);
            entityBuilder.Create();

            var plantingEventEntityBuilder = new PlantingEventEntityBuilder(migrationBuilder, ActiveDatabase);
            plantingEventEntityBuilder.Create();

            var treeMonitoringEntityBuilder = new TreeMonitoringEntityBuilder(migrationBuilder, ActiveDatabase);
            treeMonitoringEntityBuilder.Create();
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var entityBuilder = new TreeEntityBuilder(migrationBuilder, ActiveDatabase);
            entityBuilder.Drop();

            var plantingEventEntityBuilder = new PlantingEventEntityBuilder(migrationBuilder, ActiveDatabase);
            plantingEventEntityBuilder.Drop();

            var treeMonitoringEntityBuilder = new TreeMonitoringEntityBuilder(migrationBuilder, ActiveDatabase);
            treeMonitoringEntityBuilder.Drop();
        }
    }
}