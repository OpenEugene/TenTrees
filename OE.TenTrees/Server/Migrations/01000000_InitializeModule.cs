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
            // Create MyModule table
            var myModuleEntityBuilder = new MyModuleEntityBuilder(migrationBuilder, ActiveDatabase);
            myModuleEntityBuilder.Create();

            // Create Application-related tables
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

            var siteAssessmentEntityBuilder = new SiteAssessmentEntityBuilder(migrationBuilder, ActiveDatabase);
            siteAssessmentEntityBuilder.Create();

            var assessmentPhotoEntityBuilder = new AssessmentPhotoEntityBuilder(migrationBuilder, ActiveDatabase);
            assessmentPhotoEntityBuilder.Create();

            // Create Garden-related tables
            var gardenSiteEntityBuilder = new GardenSiteEntityBuilder(migrationBuilder, ActiveDatabase);
            gardenSiteEntityBuilder.Create();

            // Add unique constraint for ApplicationId in GardenSite
            migrationBuilder.CreateIndex(
                name: "UQ_GardenSite_ApplicationId",
                table: "GardenSite",
                column: "ApplicationId",
                unique: true);

            var treePlantingEntityBuilder = new TreePlantingEntityBuilder(migrationBuilder, ActiveDatabase);
            treePlantingEntityBuilder.Create();

            var gardenPhotoEntityBuilder = new GardenPhotoEntityBuilder(migrationBuilder, ActiveDatabase);
            gardenPhotoEntityBuilder.Create();

            // Create Monitoring-related tables
            var monitoringSessionEntityBuilder = new MonitoringSessionEntityBuilder(migrationBuilder, ActiveDatabase);
            monitoringSessionEntityBuilder.Create();

            var monitoringMetricEntityBuilder = new MonitoringMetricEntityBuilder(migrationBuilder, ActiveDatabase);
            monitoringMetricEntityBuilder.Create();

            var monitoringPhotoEntityBuilder = new MonitoringPhotoEntityBuilder(migrationBuilder, ActiveDatabase);
            monitoringPhotoEntityBuilder.Create();
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop in reverse order due to foreign key dependencies
            var monitoringPhotoEntityBuilder = new MonitoringPhotoEntityBuilder(migrationBuilder, ActiveDatabase);
            monitoringPhotoEntityBuilder.Drop();

            var monitoringMetricEntityBuilder = new MonitoringMetricEntityBuilder(migrationBuilder, ActiveDatabase);
            monitoringMetricEntityBuilder.Drop();

            var monitoringSessionEntityBuilder = new MonitoringSessionEntityBuilder(migrationBuilder, ActiveDatabase);
            monitoringSessionEntityBuilder.Drop();

            var gardenPhotoEntityBuilder = new GardenPhotoEntityBuilder(migrationBuilder, ActiveDatabase);
            gardenPhotoEntityBuilder.Drop();

            var treePlantingEntityBuilder = new TreePlantingEntityBuilder(migrationBuilder, ActiveDatabase);
            treePlantingEntityBuilder.Drop();

            // Drop unique constraint before dropping GardenSite table
            migrationBuilder.DropIndex(
                name: "UQ_GardenSite_ApplicationId",
                table: "GardenSite");

            var gardenSiteEntityBuilder = new GardenSiteEntityBuilder(migrationBuilder, ActiveDatabase);
            gardenSiteEntityBuilder.Drop();

            var assessmentPhotoEntityBuilder = new AssessmentPhotoEntityBuilder(migrationBuilder, ActiveDatabase);
            assessmentPhotoEntityBuilder.Drop();

            var siteAssessmentEntityBuilder = new SiteAssessmentEntityBuilder(migrationBuilder, ActiveDatabase);
            siteAssessmentEntityBuilder.Drop();

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

            var myModuleEntityBuilder = new MyModuleEntityBuilder(migrationBuilder, ActiveDatabase);
            myModuleEntityBuilder.Drop();
        }
    }
}
