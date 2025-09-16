using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using Oqtane.Migrations.EntityBuilders;

namespace OE.TenTrees.Migrations.EntityBuilders
{
    public class GardenSiteEntityBuilder : AuditableBaseEntityBuilder<GardenSiteEntityBuilder>
    {
        private const string _entityTableName = "GardenSite";
        private readonly PrimaryKey<GardenSiteEntityBuilder> _primaryKey = new("PK_GardenSite", x => x.GardenSiteId);
        private readonly ForeignKey<GardenSiteEntityBuilder> _applicationForeignKey = new("FK_GardenSite_Application", x => x.ApplicationId, "TreePlantingApplication", "ApplicationId", ReferentialAction.Restrict);
        private readonly ForeignKey<GardenSiteEntityBuilder> _moduleForeignKey = new("FK_GardenSite_Module", x => x.ModuleId, "Module", "ModuleId", ReferentialAction.Cascade);

        public GardenSiteEntityBuilder(MigrationBuilder migrationBuilder, IDatabase database) : base(migrationBuilder, database)
        {
            EntityTableName = _entityTableName;
            PrimaryKey = _primaryKey;
            ForeignKeys.Add(_applicationForeignKey);
            ForeignKeys.Add(_moduleForeignKey);
        }

        protected override GardenSiteEntityBuilder BuildTable(ColumnsBuilder table)
        {
            GardenSiteId = AddAutoIncrementColumn(table, "GardenSiteId");
            ApplicationId = AddIntegerColumn(table, "ApplicationId");
            ModuleId = AddIntegerColumn(table, "ModuleId");
            
            EvaluatorName = AddStringColumn(table, "EvaluatorName", 200, true);
            BeneficiaryName = AddStringColumn(table, "BeneficiaryName", 200, true);
            HouseNumber = AddStringColumn(table, "HouseNumber", 100, true);
            IdOrBirthdate = AddStringColumn(table, "IdOrBirthdate", 100, true);
            
            HasWaterInPlot = AddBooleanColumn(table, "HasWaterInPlot", true);
            HasWaterCatchmentSystem = AddBooleanColumn(table, "HasWaterCatchmentSystem", true);
            NumberOfExistingTrees = AddIntegerColumn(table, "NumberOfExistingTrees", true);
            NumberOfIndigenousTrees = AddIntegerColumn(table, "NumberOfIndigenousTrees", true);
            NumberOfFruitNutTrees = AddIntegerColumn(table, "NumberOfFruitNutTrees", true);
            HasSpaceForMoreTrees = AddBooleanColumn(table, "HasSpaceForMoreTrees", true);
            IsPropertyFenced = AddBooleanColumn(table, "IsPropertyFenced", true);
            HasCompostOrMulchResources = AddBooleanColumn(table, "HasCompostOrMulchResources", true);
            
            Status = AddIntegerColumn(table, "Status");
            PlantingDate = AddDateTimeColumn(table, "PlantingDate", true);
            LastMonitoringDate = AddDateTimeColumn(table, "LastMonitoringDate", true);
            
            Village = AddStringColumn(table, "Village", 200, true);
            Address = AddStringColumn(table, "Address", 500, true);
            ContactInformation = AddStringColumn(table, "ContactInformation", 500, true);
            Latitude = AddDecimalColumn(table, "Latitude", 18, 6, true);
            Longitude = AddDecimalColumn(table, "Longitude", 18, 6, true);
            
            Notes = AddStringColumn(table, "Notes", 2000, true);
            SpecialInstructions = AddStringColumn(table, "SpecialInstructions", 1000, true);

            AddAuditableColumns(table);
            return this;
        }

        public OperationBuilder<AddColumnOperation> GardenSiteId { get; set; }
        public OperationBuilder<AddColumnOperation> ApplicationId { get; set; }
        public OperationBuilder<AddColumnOperation> ModuleId { get; set; }
        public OperationBuilder<AddColumnOperation> EvaluatorName { get; set; }
        public OperationBuilder<AddColumnOperation> BeneficiaryName { get; set; }
        public OperationBuilder<AddColumnOperation> HouseNumber { get; set; }
        public OperationBuilder<AddColumnOperation> IdOrBirthdate { get; set; }
        public OperationBuilder<AddColumnOperation> HasWaterInPlot { get; set; }
        public OperationBuilder<AddColumnOperation> HasWaterCatchmentSystem { get; set; }
        public OperationBuilder<AddColumnOperation> NumberOfExistingTrees { get; set; }
        public OperationBuilder<AddColumnOperation> NumberOfIndigenousTrees { get; set; }
        public OperationBuilder<AddColumnOperation> NumberOfFruitNutTrees { get; set; }
        public OperationBuilder<AddColumnOperation> HasSpaceForMoreTrees { get; set; }
        public OperationBuilder<AddColumnOperation> IsPropertyFenced { get; set; }
        public OperationBuilder<AddColumnOperation> HasCompostOrMulchResources { get; set; }
        public OperationBuilder<AddColumnOperation> Status { get; set; }
        public OperationBuilder<AddColumnOperation> PlantingDate { get; set; }
        public OperationBuilder<AddColumnOperation> LastMonitoringDate { get; set; }
        public OperationBuilder<AddColumnOperation> Village { get; set; }
        public OperationBuilder<AddColumnOperation> Address { get; set; }
        public OperationBuilder<AddColumnOperation> ContactInformation { get; set; }
        public OperationBuilder<AddColumnOperation> Latitude { get; set; }
        public OperationBuilder<AddColumnOperation> Longitude { get; set; }
        public OperationBuilder<AddColumnOperation> Notes { get; set; }
        public OperationBuilder<AddColumnOperation> SpecialInstructions { get; set; }
    }

    public class TreePlantingEntityBuilder : AuditableBaseEntityBuilder<TreePlantingEntityBuilder>
    {
        private const string _entityTableName = "TreePlanting";
        private readonly PrimaryKey<TreePlantingEntityBuilder> _primaryKey = new("PK_TreePlanting", x => x.TreePlantingId);
        private readonly ForeignKey<TreePlantingEntityBuilder> _gardenSiteForeignKey = new("FK_TreePlanting_GardenSite", x => x.GardenSiteId, "GardenSite", "GardenSiteId", ReferentialAction.Cascade);

        public TreePlantingEntityBuilder(MigrationBuilder migrationBuilder, IDatabase database) : base(migrationBuilder, database)
        {
            EntityTableName = _entityTableName;
            PrimaryKey = _primaryKey;
            ForeignKeys.Add(_gardenSiteForeignKey);
        }

        protected override TreePlantingEntityBuilder BuildTable(ColumnsBuilder table)
        {
            TreePlantingId = AddAutoIncrementColumn(table, "TreePlantingId");
            GardenSiteId = AddIntegerColumn(table, "GardenSiteId");
            TreeSpecies = AddStringColumn(table, "TreeSpecies", 200, true);
            TreeVariety = AddStringColumn(table, "TreeVariety", 200, true);
            Quantity = AddIntegerColumn(table, "Quantity");
            PlantingDate = AddDateTimeColumn(table, "PlantingDate");
            PlantingLocation = AddStringColumn(table, "PlantingLocation", 500, true);
            Status = AddIntegerColumn(table, "Status");
            Notes = AddStringColumn(table, "Notes", 1000, true);

            AddAuditableColumns(table);
            return this;
        }

        public OperationBuilder<AddColumnOperation> TreePlantingId { get; set; }
        public OperationBuilder<AddColumnOperation> GardenSiteId { get; set; }
        public OperationBuilder<AddColumnOperation> TreeSpecies { get; set; }
        public OperationBuilder<AddColumnOperation> TreeVariety { get; set; }
        public OperationBuilder<AddColumnOperation> Quantity { get; set; }
        public OperationBuilder<AddColumnOperation> PlantingDate { get; set; }
        public OperationBuilder<AddColumnOperation> PlantingLocation { get; set; }
        public OperationBuilder<AddColumnOperation> Status { get; set; }
        public OperationBuilder<AddColumnOperation> Notes { get; set; }
    }

    public class GardenPhotoEntityBuilder : AuditableBaseEntityBuilder<GardenPhotoEntityBuilder>
    {
        private const string _entityTableName = "GardenPhoto";
        private readonly PrimaryKey<GardenPhotoEntityBuilder> _primaryKey = new("PK_GardenPhoto", x => x.GardenPhotoId);
        private readonly ForeignKey<GardenPhotoEntityBuilder> _gardenSiteForeignKey = new("FK_GardenPhoto_GardenSite", x => x.GardenSiteId, "GardenSite", "GardenSiteId", ReferentialAction.Cascade);

        public GardenPhotoEntityBuilder(MigrationBuilder migrationBuilder, IDatabase database) : base(migrationBuilder, database)
        {
            EntityTableName = _entityTableName;
            PrimaryKey = _primaryKey;
            ForeignKeys.Add(_gardenSiteForeignKey);
        }

        protected override GardenPhotoEntityBuilder BuildTable(ColumnsBuilder table)
        {
            GardenPhotoId = AddAutoIncrementColumn(table, "GardenPhotoId");
            GardenSiteId = AddIntegerColumn(table, "GardenSiteId");
            Url = AddStringColumn(table, "Url", 500, true);
            Caption = AddStringColumn(table, "Caption", 500, true);
            FileName = AddStringColumn(table, "FileName", 255, true);
            ContentType = AddStringColumn(table, "ContentType", 100, true);
            FileSize = AddIntegerColumn(table, "FileSize", true);
            PhotoType = AddIntegerColumn(table, "PhotoType");
            PhotoDate = AddDateTimeColumn(table, "PhotoDate");

            AddAuditableColumns(table);
            return this;
        }

        public OperationBuilder<AddColumnOperation> GardenPhotoId { get; set; }
        public OperationBuilder<AddColumnOperation> GardenSiteId { get; set; }
        public OperationBuilder<AddColumnOperation> Url { get; set; }
        public OperationBuilder<AddColumnOperation> Caption { get; set; }
        public OperationBuilder<AddColumnOperation> FileName { get; set; }
        public OperationBuilder<AddColumnOperation> ContentType { get; set; }
        public OperationBuilder<AddColumnOperation> FileSize { get; set; }
        public OperationBuilder<AddColumnOperation> PhotoType { get; set; }
        public OperationBuilder<AddColumnOperation> PhotoDate { get; set; }
    }
}