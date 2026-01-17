using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using Oqtane.Migrations.EntityBuilders;

namespace OpenEug.TenTrees.Module.Enrollment.Migrations.EntityBuilders
{
    public class EnrollmentEntityBuilder : AuditableBaseEntityBuilder<EnrollmentEntityBuilder>
    {
        private const string _entityTableName = "OpenEug.TenTreesEnrollment";
        private readonly PrimaryKey<EnrollmentEntityBuilder> _primaryKey = new("PK_OpenEug.TenTreesEnrollment", x => x.EnrollmentId);
        private readonly ForeignKey<EnrollmentEntityBuilder> _moduleForeignKey = new("FK_OpenEug.TenTreesEnrollment_Module", x => x.ModuleId, "Module", "ModuleId", ReferentialAction.Cascade);

        public EnrollmentEntityBuilder(MigrationBuilder migrationBuilder, IDatabase database) : base(migrationBuilder, database)
        {
            EntityTableName = _entityTableName;
            PrimaryKey = _primaryKey;
            ForeignKeys.Add(_moduleForeignKey);
        }

        protected override EnrollmentEntityBuilder BuildTable(ColumnsBuilder table)
        {
            EnrollmentId = AddAutoIncrementColumn(table,"EnrollmentId");
            ModuleId = AddIntegerColumn(table,"ModuleId");
            Name = AddMaxStringColumn(table,"Name");
            AddAuditableColumns(table);
            return this;
        }

        public OperationBuilder<AddColumnOperation> EnrollmentId { get; set; }
        public OperationBuilder<AddColumnOperation> ModuleId { get; set; }
        public OperationBuilder<AddColumnOperation> Name { get; set; }
    }
}
