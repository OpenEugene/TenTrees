using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using OpenEug.TenTrees.Module.Village.Migrations.EntityBuilders;

namespace OpenEug.TenTrees.Module.Village.Migrations
{
    [DbContext(typeof(OpenEug.TenTrees.Repository.TenTreesContext))]
    [Migration("OpenEug.TenTrees.01.00.00.00")]
    public class InitializeModule : MultiDatabaseMigration
    {
        public InitializeModule(IDatabase database) : base(database)
        {
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var villageEntityBuilder = new VillageEntityBuilder(migrationBuilder, ActiveDatabase);
            villageEntityBuilder.Create();
            
            // Seed initial villages
            migrationBuilder.Sql(@"
                INSERT INTO [OpenEug.TenTreesVillage] (VillageName, ContactName, ContactPhone, ContactEmail, Notes, IsActive, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn)
                VALUES 
                ('Orpen Gate Village', NULL, NULL, NULL, 'Initial village', 1, 'System', GETDATE(), 'System', GETDATE()),
                ('Londelozzi', NULL, NULL, NULL, 'Initial village', 1, 'System', GETDATE(), 'System', GETDATE())
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var villageEntityBuilder = new VillageEntityBuilder(migrationBuilder, ActiveDatabase);
            villageEntityBuilder.Drop();
        }
    }
}
