using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using Oqtane.Migrations.EntityBuilders;

namespace OpenEug.TenTrees.Module.Village.Migrations.EntityBuilders
{
    public class VillageEntityBuilder : AuditableBaseEntityBuilder<VillageEntityBuilder>
    {
        private const string _entityTableName = "OpenEug.TenTreesVillage";
        private readonly PrimaryKey<VillageEntityBuilder> _primaryKey = new("PK_OpenEug.TenTreesVillage", x => x.VillageId);

        public VillageEntityBuilder(MigrationBuilder migrationBuilder, IDatabase database) : base(migrationBuilder, database)
        {
            EntityTableName = _entityTableName;
            PrimaryKey = _primaryKey;
        }

        protected override VillageEntityBuilder BuildTable(ColumnsBuilder table)
        {
            VillageId = AddAutoIncrementColumn(table, "VillageId");
            VillageName = AddMaxStringColumn(table, "VillageName");
            ContactName = AddStringColumn(table, "ContactName", 200, true);
            ContactPhone = AddStringColumn(table, "ContactPhone", 50, true);
            ContactEmail = AddStringColumn(table, "ContactEmail", 200, true);
            Notes = AddMaxStringColumn(table, "Notes", true);
            IsActive = AddBooleanColumn(table, "IsActive");
            
            AddAuditableColumns(table);
            return this;
        }

        public OperationBuilder<AddColumnOperation> VillageId { get; set; }
        public OperationBuilder<AddColumnOperation> VillageName { get; set; }
        public OperationBuilder<AddColumnOperation> ContactName { get; set; }
        public OperationBuilder<AddColumnOperation> ContactPhone { get; set; }
        public OperationBuilder<AddColumnOperation> ContactEmail { get; set; }
        public OperationBuilder<AddColumnOperation> Notes { get; set; }
        public OperationBuilder<AddColumnOperation> IsActive { get; set; }
    }
}
