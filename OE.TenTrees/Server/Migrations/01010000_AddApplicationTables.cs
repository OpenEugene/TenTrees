using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using OE.TenTrees.Migrations.EntityBuilders;
using OE.TenTrees.Repository;

namespace OE.TenTrees.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("OE.TenTrees.01.01.00.00")]
    public class AddApplicationTables : MultiDatabaseMigration
    {
        public AddApplicationTables(IDatabase database) : base(database)
        {
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var applicationEntityBuilder = new ApplicationEntityBuilder(migrationBuilder, ActiveDatabase);
            applicationEntityBuilder.Create();

            var applicationDocumentEntityBuilder = new ApplicationDocumentEntityBuilder(migrationBuilder, ActiveDatabase);
            applicationDocumentEntityBuilder.Create();

            var applicationStatusHistoryEntityBuilder = new ApplicationStatusHistoryEntityBuilder(migrationBuilder, ActiveDatabase);
            applicationStatusHistoryEntityBuilder.Create();

            var applicationReviewEntityBuilder = new ApplicationReviewEntityBuilder(migrationBuilder, ActiveDatabase);
            applicationReviewEntityBuilder.Create();

            var reviewChecklistItemEntityBuilder = new ReviewChecklistItemEntityBuilder(migrationBuilder, ActiveDatabase);
            reviewChecklistItemEntityBuilder.Create();
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var reviewChecklistItemEntityBuilder = new ReviewChecklistItemEntityBuilder(migrationBuilder, ActiveDatabase);
            reviewChecklistItemEntityBuilder.Drop();

            var applicationReviewEntityBuilder = new ApplicationReviewEntityBuilder(migrationBuilder, ActiveDatabase);
            applicationReviewEntityBuilder.Drop();

            var applicationStatusHistoryEntityBuilder = new ApplicationStatusHistoryEntityBuilder(migrationBuilder, ActiveDatabase);
            applicationStatusHistoryEntityBuilder.Drop();

            var applicationDocumentEntityBuilder = new ApplicationDocumentEntityBuilder(migrationBuilder, ActiveDatabase);
            applicationDocumentEntityBuilder.Drop();

            var applicationEntityBuilder = new ApplicationEntityBuilder(migrationBuilder, ActiveDatabase);
            applicationEntityBuilder.Drop();
        }
    }
}