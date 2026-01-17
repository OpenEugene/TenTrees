using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using OpenEug.TenTrees.Module.Enrollment.Migrations.EntityBuilders;
using OpenEug.TenTrees.Module.Enrollment.Repository;

namespace OpenEug.TenTrees.Module.Enrollment.Migrations
{
    [DbContext(typeof(EnrollmentContext))]
    [Migration("OpenEug.TenTrees.Module.Enrollment.01.00.00.00")]
    public class EnrollmentInitialize : MultiDatabaseMigration
    {
        public EnrollmentInitialize(IDatabase database) : base(database)
        {
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var entityBuilder = new EnrollmentEntityBuilder(migrationBuilder, ActiveDatabase);
            entityBuilder.Create();
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var entityBuilder = new EnrollmentEntityBuilder(migrationBuilder, ActiveDatabase);
            entityBuilder.Drop();
        }
    }
}
