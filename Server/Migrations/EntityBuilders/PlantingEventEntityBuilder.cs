using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations.EntityBuilders;

namespace OE.TenTrees.Migrations.EntityBuilders
{
    public class PlantingEventEntityBuilder : AuditableBaseEntityBuilder<PlantingEventEntityBuilder>
    {
        private const string _entityTableName = "OETenTreesPlantingEvent";
        private readonly PrimaryKey<PlantingEventEntityBuilder> _primaryKey = new("PlantingEventId");

        public PlantingEventEntityBuilder(MigrationBuilder migrationBuilder, IDatabase database) : base(migrationBuilder, database)
        {
            EntityTableName = _entityTableName;
            PrimaryKey = _primaryKey;
        }

        protected override PlantingEventEntityBuilder BuildTable(ColumnsBuilder table)
        {
            PlantingEventId = AddAutoIncrementColumn(table,"PlantingEventId");
            ModuleId = AddIntegerColumn(table,"ModuleId");
            EventName = AddStringColumn(table,"EventName", 100, false);
            PlantingDate = AddDateTimeColumn(table,"PlantingDate", false);
            Latitude = AddDoubleColumn(table,"Latitude", false);
            Longitude = AddDoubleColumn(table,"Longitude", false);
            Location = AddStringColumn(table,"Location", 200);
            TreesPlanted = AddIntegerColumn(table,"TreesPlanted", false);
            Description = AddStringColumn(table,"Description", 500);
            Organizer = AddStringColumn(table,"Organizer", 100);
            Sponsor = AddStringColumn(table,"Sponsor", 100);
            ParticipantCount = AddIntegerColumn(table,"ParticipantCount");
            Notes = AddStringColumn(table,"Notes", 1000);

            AddAuditableColumns(table);

            return this;
        }

        public OperationBuilder<AddColumnOperation> PlantingEventId { get; set; }
        public OperationBuilder<AddColumnOperation> ModuleId { get; set; }
        public OperationBuilder<AddColumnOperation> EventName { get; set; }
        public OperationBuilder<AddColumnOperation> PlantingDate { get; set; }
        public OperationBuilder<AddColumnOperation> Latitude { get; set; }
        public OperationBuilder<AddColumnOperation> Longitude { get; set; }
        public OperationBuilder<AddColumnOperation> Location { get; set; }
        public OperationBuilder<AddColumnOperation> TreesPlanted { get; set; }
        public OperationBuilder<AddColumnOperation> Description { get; set; }
        public OperationBuilder<AddColumnOperation> Organizer { get; set; }
        public OperationBuilder<AddColumnOperation> Sponsor { get; set; }
        public OperationBuilder<AddColumnOperation> ParticipantCount { get; set; }
        public OperationBuilder<AddColumnOperation> Notes { get; set; }
    }
}