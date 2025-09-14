using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations.EntityBuilders;

namespace OE.TenTrees.Migrations.EntityBuilders
{
    public class TreeMonitoringEntityBuilder : AuditableBaseEntityBuilder<TreeMonitoringEntityBuilder>
    {
        private const string _entityTableName = "OETenTreesTreeMonitoring";
        private readonly PrimaryKey<TreeMonitoringEntityBuilder> _primaryKey = new("TreeMonitoringId");

        public TreeMonitoringEntityBuilder(MigrationBuilder migrationBuilder, IDatabase database) : base(migrationBuilder, database)
        {
            EntityTableName = _entityTableName;
            PrimaryKey = _primaryKey;
        }

        protected override TreeMonitoringEntityBuilder BuildTable(ColumnsBuilder table)
        {
            TreeMonitoringId = AddAutoIncrementColumn(table,"TreeMonitoringId");
            ModuleId = AddIntegerColumn(table,"ModuleId");
            TreeId = AddIntegerColumn(table,"TreeId", false);
            MonitoringDate = AddDateTimeColumn(table,"MonitoringDate", false);
            HealthStatus = AddStringColumn(table,"HealthStatus", 50, false, "Healthy");
            Height = AddDoubleColumn(table,"Height");
            Diameter = AddDoubleColumn(table,"Diameter");
            Observations = AddStringColumn(table,"Observations", 500);
            MonitoredBy = AddStringColumn(table,"MonitoredBy", 100);
            PhotoUrl = AddStringColumn(table,"PhotoUrl", 200);
            RequiresAttention = AddBooleanColumn(table,"RequiresAttention", false, false);
            RecommendedActions = AddStringColumn(table,"RecommendedActions", 300);

            AddAuditableColumns(table);

            return this;
        }

        public OperationBuilder<AddColumnOperation> TreeMonitoringId { get; set; }
        public OperationBuilder<AddColumnOperation> ModuleId { get; set; }
        public OperationBuilder<AddColumnOperation> TreeId { get; set; }
        public OperationBuilder<AddColumnOperation> MonitoringDate { get; set; }
        public OperationBuilder<AddColumnOperation> HealthStatus { get; set; }
        public OperationBuilder<AddColumnOperation> Height { get; set; }
        public OperationBuilder<AddColumnOperation> Diameter { get; set; }
        public OperationBuilder<AddColumnOperation> Observations { get; set; }
        public OperationBuilder<AddColumnOperation> MonitoredBy { get; set; }
        public OperationBuilder<AddColumnOperation> PhotoUrl { get; set; }
        public OperationBuilder<AddColumnOperation> RequiresAttention { get; set; }
        public OperationBuilder<AddColumnOperation> RecommendedActions { get; set; }
    }
}