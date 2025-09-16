using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using OE.TenTrees.Migrations.EntityBuilders;
using OE.TenTrees.Repository;

namespace OE.TenTrees.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("OE.TenTrees.01.03.00.00")]
    public class AddGardenTables : MultiDatabaseMigration
    {
        public AddGardenTables(IDatabase database) : base(database)
        {
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create Garden table
            var gardenEntityBuilder = new GardenEntityBuilder(migrationBuilder, ActiveDatabase);
            gardenEntityBuilder.Create();

            // Create TreePlanting table
            var treePlantingEntityBuilder = new TreePlantingEntityBuilder(migrationBuilder, ActiveDatabase);
            treePlantingEntityBuilder.Create();

            // Create GardenPhoto table
            var gardenPhotoEntityBuilder = new GardenPhotoEntityBuilder(migrationBuilder, ActiveDatabase);
            gardenPhotoEntityBuilder.Create();

            // Add GardenId to MonitoringSession table to link monitoring to gardens instead of applications
            migrationBuilder.AddColumn<int>(
                name: "GardenId",
                table: "MonitoringSession",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MonitoringSession_GardenId",
                table: "MonitoringSession",
                column: "GardenId");

            migrationBuilder.AddForeignKey(
                name: "FK_MonitoringSession_Garden",
                table: "MonitoringSession",
                column: "GardenId",
                principalTable: "Garden",
                principalColumn: "GardenId",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove foreign key and index from MonitoringSession
            migrationBuilder.DropForeignKey(
                name: "FK_MonitoringSession_Garden",
                table: "MonitoringSession");

            migrationBuilder.DropIndex(
                name: "IX_MonitoringSession_GardenId",
                table: "MonitoringSession");

            migrationBuilder.DropColumn(
                name: "GardenId",
                table: "MonitoringSession");

            // Drop Garden tables
            migrationBuilder.DropTable(name: "GardenPhoto");
            migrationBuilder.DropTable(name: "TreePlanting");
            migrationBuilder.DropTable(name: "Garden");
        }
    }
}