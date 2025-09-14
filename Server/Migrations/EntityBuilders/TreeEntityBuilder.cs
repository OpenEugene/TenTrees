using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations.EntityBuilders;

namespace OE.TenTrees.Migrations.EntityBuilders
{
    public class TreeEntityBuilder : AuditableBaseEntityBuilder<TreeEntityBuilder>
    {
        private const string _entityTableName = "OETenTreesTree";
        private readonly PrimaryKey<TreeEntityBuilder> _primaryKey = new("TreeId");

        public TreeEntityBuilder(MigrationBuilder migrationBuilder, IDatabase database) : base(migrationBuilder, database)
        {
            EntityTableName = _entityTableName;
            PrimaryKey = _primaryKey;
        }

        protected override TreeEntityBuilder BuildTable(ColumnsBuilder table)
        {
            TreeId = AddAutoIncrementColumn(table,"TreeId");
            ModuleId = AddIntegerColumn(table,"ModuleId");
            Species = AddStringColumn(table,"Species", 100, false);
            CommonName = AddStringColumn(table,"CommonName", 100);
            Latitude = AddDoubleColumn(table,"Latitude", false);
            Longitude = AddDoubleColumn(table,"Longitude", false);
            Location = AddStringColumn(table,"Location", 200);
            PlantedDate = AddDateTimeColumn(table,"PlantedDate", false);
            Status = AddStringColumn(table,"Status", 50, false, "Healthy");
            Notes = AddStringColumn(table,"Notes", 500);
            Height = AddDoubleColumn(table,"Height");
            Diameter = AddDoubleColumn(table,"Diameter");
            PlantedBy = AddStringColumn(table,"PlantedBy", 100);

            AddAuditableColumns(table);

            return this;
        }

        public OperationBuilder<AddColumnOperation> TreeId { get; set; }
        public OperationBuilder<AddColumnOperation> ModuleId { get; set; }
        public OperationBuilder<AddColumnOperation> Species { get; set; }
        public OperationBuilder<AddColumnOperation> CommonName { get; set; }
        public OperationBuilder<AddColumnOperation> Latitude { get; set; }
        public OperationBuilder<AddColumnOperation> Longitude { get; set; }
        public OperationBuilder<AddColumnOperation> Location { get; set; }
        public OperationBuilder<AddColumnOperation> PlantedDate { get; set; }
        public OperationBuilder<AddColumnOperation> Status { get; set; }
        public OperationBuilder<AddColumnOperation> Notes { get; set; }
        public OperationBuilder<AddColumnOperation> Height { get; set; }
        public OperationBuilder<AddColumnOperation> Diameter { get; set; }
        public OperationBuilder<AddColumnOperation> PlantedBy { get; set; }
    }
}