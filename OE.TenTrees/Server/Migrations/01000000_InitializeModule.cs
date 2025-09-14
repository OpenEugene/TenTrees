using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using OE.TenTrees.Migrations.EntityBuilders;
using OE.TenTrees.Repository;

namespace OE.TenTrees.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("OE.TenTrees.01.00.00.00")]
    public class InitializeModule : MultiDatabaseMigration
    {
        public InitializeModule(IDatabase database) : base(database)
        {
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var myModuleEntityBuilder = new MyModuleEntityBuilder(migrationBuilder, ActiveDatabase);
            myModuleEntityBuilder.Create();
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var myModuleEntityBuilder = new MyModuleEntityBuilder(migrationBuilder, ActiveDatabase);
            myModuleEntityBuilder.Drop();
        }
    }
}
