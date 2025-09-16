using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using Oqtane.Migrations.EntityBuilders;

namespace OE.TenTrees.Migrations.EntityBuilders
{
    public class MonitoringSessionEntityBuilder : AuditableBaseEntityBuilder<MonitoringSessionEntityBuilder>
    {
        private const string _entityTableName = "MonitoringSession";
        private readonly PrimaryKey<MonitoringSessionEntityBuilder> _primaryKey = new("PK_MonitoringSession", x => x.MonitoringSessionId);
        private readonly ForeignKey<MonitoringSessionEntityBuilder> _applicationForeignKey = new("FK_MonitoringSession_Application", x => x.ApplicationId, "TreePlantingApplication", "ApplicationId", ReferentialAction.Restrict);
        private readonly ForeignKey<MonitoringSessionEntityBuilder> _gardenSiteForeignKey = new("FK_MonitoringSession_GardenSite", x => x.GardenSiteId, "GardenSite", "GardenSiteId", ReferentialAction.SetNull);

        public MonitoringSessionEntityBuilder(MigrationBuilder migrationBuilder, IDatabase database) : base(migrationBuilder, database)
        {
            EntityTableName = _entityTableName;
            PrimaryKey = _primaryKey;
            ForeignKeys.Add(_applicationForeignKey);
            ForeignKeys.Add(_gardenSiteForeignKey);
        }

        protected override MonitoringSessionEntityBuilder BuildTable(ColumnsBuilder table)
        {
            MonitoringSessionId = AddAutoIncrementColumn(table, "MonitoringSessionId");
            ApplicationId = AddIntegerColumn(table, "ApplicationId");
            GardenSiteId = AddIntegerColumn(table, "GardenSiteId", true); // New field for garden site reference
            ModuleId = AddIntegerColumn(table, "ModuleId");
            SessionDate = AddDateTimeColumn(table, "SessionDate");
            ObserverUserId = AddStringColumn(table, "ObserverUserId", 256, true);
            EvaluatorName = AddStringColumn(table, "EvaluatorName", 200, true);
            BeneficiaryName = AddStringColumn(table, "BeneficiaryName", 200, true);
            HouseNumber = AddStringColumn(table, "HouseNumber", 100, true);
            IdOrBirthdate = AddStringColumn(table, "IdOrBirthdate", 100, true);
            WeatherConditions = AddStringColumn(table, "WeatherConditions", 500, true);
            Notes = AddStringColumn(table, "Notes", 2000, true);

            // Mapping Information
            HasWaterInPlot = AddBooleanColumn(table, "HasWaterInPlot", true);
            HasWaterCatchmentSystem = AddBooleanColumn(table, "HasWaterCatchmentSystem", true);

            // Tree Counting
            NumberOfExistingTrees = AddIntegerColumn(table, "NumberOfExistingTrees", true);
            NumberOfIndigenousTrees = AddIntegerColumn(table, "NumberOfIndigenousTrees", true);
            NumberOfFruitNutTrees = AddIntegerColumn(table, "NumberOfFruitNutTrees", true);

            // Space and Infrastructure
            HasSpaceForMoreTrees = AddBooleanColumn(table, "HasSpaceForMoreTrees", true);
            IsPropertyFenced = AddBooleanColumn(table, "IsPropertyFenced", true);
            HasCompostOrMulchResources = AddBooleanColumn(table, "HasCompostOrMulchResources", true);

            // Tree Health Monitoring
            TreesPlanted = AddIntegerColumn(table, "TreesPlanted", true);
            TreesAlive = AddIntegerColumn(table, "TreesAlive", true);
            TreesLookingHealthy = AddBooleanColumn(table, "TreesLookingHealthy", true);
            UsingChemicalFertilizers = AddBooleanColumn(table, "UsingChemicalFertilizers", true);
            UsingPesticides = AddBooleanColumn(table, "UsingPesticides", true);
            TreesBeingMulched = AddBooleanColumn(table, "TreesBeingMulched", true);
            MakingCompost = AddBooleanColumn(table, "MakingCompost", true);
            CollectingWater = AddBooleanColumn(table, "CollectingWater", true);

            // Problem Reporting
            ProblemsIdentified = AddStringColumn(table, "ProblemsIdentified", 2000, true);
            InterventionNeeded = AddStringColumn(table, "InterventionNeeded", 2000, true);
            MortalityRate = AddDecimalColumn(table, "MortalityRate", 5, 2, true);
            MortalityReason = AddStringColumn(table, "MortalityReason", 500, true);

            AddAuditableColumns(table);
            return this;
        }

        public OperationBuilder<AddColumnOperation> MonitoringSessionId { get; set; }
        public OperationBuilder<AddColumnOperation> ApplicationId { get; set; }
        public OperationBuilder<AddColumnOperation> GardenSiteId { get; set; }
        public OperationBuilder<AddColumnOperation> ModuleId { get; set; }
        public OperationBuilder<AddColumnOperation> SessionDate { get; set; }
        public OperationBuilder<AddColumnOperation> ObserverUserId { get; set; }
        public OperationBuilder<AddColumnOperation> EvaluatorName { get; set; }
        public OperationBuilder<AddColumnOperation> BeneficiaryName { get; set; }
        public OperationBuilder<AddColumnOperation> HouseNumber { get; set; }
        public OperationBuilder<AddColumnOperation> IdOrBirthdate { get; set; }
        public OperationBuilder<AddColumnOperation> WeatherConditions { get; set; }
        public OperationBuilder<AddColumnOperation> Notes { get; set; }
        public OperationBuilder<AddColumnOperation> HasWaterInPlot { get; set; }
        public OperationBuilder<AddColumnOperation> HasWaterCatchmentSystem { get; set; }
        public OperationBuilder<AddColumnOperation> NumberOfExistingTrees { get; set; }
        public OperationBuilder<AddColumnOperation> NumberOfIndigenousTrees { get; set; }
        public OperationBuilder<AddColumnOperation> NumberOfFruitNutTrees { get; set; }
        public OperationBuilder<AddColumnOperation> HasSpaceForMoreTrees { get; set; }
        public OperationBuilder<AddColumnOperation> IsPropertyFenced { get; set; }
        public OperationBuilder<AddColumnOperation> HasCompostOrMulchResources { get; set; }
        public OperationBuilder<AddColumnOperation> TreesPlanted { get; set; }
        public OperationBuilder<AddColumnOperation> TreesAlive { get; set; }
        public OperationBuilder<AddColumnOperation> TreesLookingHealthy { get; set; }
        public OperationBuilder<AddColumnOperation> UsingChemicalFertilizers { get; set; }
        public OperationBuilder<AddColumnOperation> UsingPesticides { get; set; }
        public OperationBuilder<AddColumnOperation> TreesBeingMulched { get; set; }
        public OperationBuilder<AddColumnOperation> MakingCompost { get; set; }
        public OperationBuilder<AddColumnOperation> CollectingWater { get; set; }
        public OperationBuilder<AddColumnOperation> ProblemsIdentified { get; set; }
        public OperationBuilder<AddColumnOperation> InterventionNeeded { get; set; }
        public OperationBuilder<AddColumnOperation> MortalityRate { get; set; }
        public OperationBuilder<AddColumnOperation> MortalityReason { get; set; }
    }

    public class MonitoringMetricEntityBuilder : AuditableBaseEntityBuilder<MonitoringMetricEntityBuilder>
    {
        private const string _entityTableName = "MonitoringMetric";
        private readonly PrimaryKey<MonitoringMetricEntityBuilder> _primaryKey = new("PK_MonitoringMetric", x => x.MonitoringMetricId);
        private readonly ForeignKey<MonitoringMetricEntityBuilder> _sessionForeignKey = new("FK_MonitoringMetric_Session", x => x.MonitoringSessionId, "MonitoringSession", "MonitoringSessionId", ReferentialAction.Cascade);

        public MonitoringMetricEntityBuilder(MigrationBuilder migrationBuilder, IDatabase database) : base(migrationBuilder, database)
        {
            EntityTableName = _entityTableName;
            PrimaryKey = _primaryKey;
            ForeignKeys.Add(_sessionForeignKey);
        }

        protected override MonitoringMetricEntityBuilder BuildTable(ColumnsBuilder table)
        {
            MonitoringMetricId = AddAutoIncrementColumn(table, "MonitoringMetricId");
            MonitoringSessionId = AddIntegerColumn(table, "MonitoringSessionId");
            MetricType = AddIntegerColumn(table, "MetricType");
            Value = AddDecimalColumn(table, "Value", 10, 3, true);
            Unit = AddStringColumn(table, "Unit", 50, true);
            Notes = AddStringColumn(table, "Notes", 1000, true);
            AddAuditableColumns(table);
            return this;
        }

        public OperationBuilder<AddColumnOperation> MonitoringMetricId { get; set; }
        public OperationBuilder<AddColumnOperation> MonitoringSessionId { get; set; }
        public OperationBuilder<AddColumnOperation> MetricType { get; set; }
        public OperationBuilder<AddColumnOperation> Value { get; set; }
        public OperationBuilder<AddColumnOperation> Unit { get; set; }
        public OperationBuilder<AddColumnOperation> Notes { get; set; }
    }

    public class MonitoringPhotoEntityBuilder : AuditableBaseEntityBuilder<MonitoringPhotoEntityBuilder>
    {
        private const string _entityTableName = "MonitoringPhoto";
        private readonly PrimaryKey<MonitoringPhotoEntityBuilder> _primaryKey = new("PK_MonitoringPhoto", x => x.MonitoringPhotoId);
        private readonly ForeignKey<MonitoringPhotoEntityBuilder> _sessionForeignKey = new("FK_MonitoringPhoto_Session", x => x.MonitoringSessionId, "MonitoringSession", "MonitoringSessionId", ReferentialAction.Cascade);

        public MonitoringPhotoEntityBuilder(MigrationBuilder migrationBuilder, IDatabase database) : base(migrationBuilder, database)
        {
            EntityTableName = _entityTableName;
            PrimaryKey = _primaryKey;
            ForeignKeys.Add(_sessionForeignKey);
        }

        protected override MonitoringPhotoEntityBuilder BuildTable(ColumnsBuilder table)
        {
            MonitoringPhotoId = AddAutoIncrementColumn(table, "MonitoringPhotoId");
            MonitoringSessionId = AddIntegerColumn(table, "MonitoringSessionId");
            Url = AddStringColumn(table, "Url", 500, true);
            Caption = AddStringColumn(table, "Caption", 500, true);
            FileName = AddStringColumn(table, "FileName", 255, true);
            ContentType = AddStringColumn(table, "ContentType", 100, true);
            FileSize = AddIntegerColumn(table, "FileSize", true);
            AddAuditableColumns(table);
            return this;
        }

        public OperationBuilder<AddColumnOperation> MonitoringPhotoId { get; set; }
        public OperationBuilder<AddColumnOperation> MonitoringSessionId { get; set; }
        public OperationBuilder<AddColumnOperation> Url { get; set; }
        public OperationBuilder<AddColumnOperation> Caption { get; set; }
        public OperationBuilder<AddColumnOperation> FileName { get; set; }
        public OperationBuilder<AddColumnOperation> ContentType { get; set; }
        public OperationBuilder<AddColumnOperation> FileSize { get; set; }
    }
}