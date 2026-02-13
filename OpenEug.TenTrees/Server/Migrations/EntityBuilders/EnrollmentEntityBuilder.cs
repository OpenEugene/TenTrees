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
            EnrollmentId = AddAutoIncrementColumn(table, "EnrollmentId");
            ModuleId = AddIntegerColumn(table, "ModuleId");
            
            // Participant Information
            ParticipantId = AddIntegerColumn(table, "ParticipantId", true);
            GrowerName = AddMaxStringColumn(table, "GrowerName");
            VillageId = AddIntegerColumn(table, "VillageId");
            HouseNumber = AddStringColumn(table, "HouseNumber", 50, true);
            IdNumber = AddStringColumn(table, "IdNumber", 50, true);
            BirthDate = AddDateTimeColumn(table, "BirthDate", true);
            HouseholdSize = AddIntegerColumn(table, "HouseholdSize");
            OwnsHome = AddBooleanColumn(table, "OwnsHome");
            
            // Enrollment metadata
            MentorId = AddIntegerColumn(table, "MentorId");
            TreeMentorName = AddStringColumn(table, "TreeMentorName", 200, true);
            EnrollmentDate = AddDateTimeColumn(table, "EnrollmentDate");
            
            // Preferred Criteria
            EnrolledInPE = AddBooleanColumn(table, "EnrolledInPE");
            PEGraduate = AddBooleanColumn(table, "PEGraduate");
            GardenPlantedAndTended = AddBooleanColumn(table, "GardenPlantedAndTended");
            ChildHeadedHousehold = AddBooleanColumn(table, "ChildHeadedHousehold");
            WomanHeadedHousehold = AddBooleanColumn(table, "WomanHeadedHousehold");
            EmptyYard = AddBooleanColumn(table, "EmptyYard");
            
            // Commitments
            CommitNoChemicals = AddBooleanColumn(table, "CommitNoChemicals");
            CommitAttendClasses = AddBooleanColumn(table, "CommitAttendClasses");
            CommitNoCuttingTrees = AddBooleanColumn(table, "CommitNoCuttingTrees");
            CommitStandForWomenChildren = AddBooleanColumn(table, "CommitStandForWomenChildren");
            CommitCareWhileAway = AddBooleanColumn(table, "CommitCareWhileAway");
            CommitAllowYardAccess = AddBooleanColumn(table, "CommitAllowYardAccess");
            
            // E-Signature
            SignatureCollected = AddBooleanColumn(table, "SignatureCollected");
            SignatureDate = AddDateTimeColumn(table, "SignatureDate", true);
            SignatureData = AddMaxStringColumn(table, "SignatureData", true);
            
            // Status
            Status = AddIntegerColumn(table, "Status");
            
            AddAuditableColumns(table);
            return this;
        }

        public OperationBuilder<AddColumnOperation> EnrollmentId { get; set; }
        public OperationBuilder<AddColumnOperation> ModuleId { get; set; }
        public OperationBuilder<AddColumnOperation> ParticipantId { get; set; }
        public OperationBuilder<AddColumnOperation> GrowerName { get; set; }
        public OperationBuilder<AddColumnOperation> VillageId { get; set; }
        public OperationBuilder<AddColumnOperation> HouseNumber { get; set; }
        public OperationBuilder<AddColumnOperation> IdNumber { get; set; }
        public OperationBuilder<AddColumnOperation> BirthDate { get; set; }
        public OperationBuilder<AddColumnOperation> HouseholdSize { get; set; }
        public OperationBuilder<AddColumnOperation> OwnsHome { get; set; }
        public OperationBuilder<AddColumnOperation> MentorId { get; set; }
        public OperationBuilder<AddColumnOperation> TreeMentorName { get; set; }
        public OperationBuilder<AddColumnOperation> EnrollmentDate { get; set; }
        public OperationBuilder<AddColumnOperation> EnrolledInPE { get; set; }
        public OperationBuilder<AddColumnOperation> PEGraduate { get; set; }
        public OperationBuilder<AddColumnOperation> GardenPlantedAndTended { get; set; }
        public OperationBuilder<AddColumnOperation> ChildHeadedHousehold { get; set; }
        public OperationBuilder<AddColumnOperation> WomanHeadedHousehold { get; set; }
        public OperationBuilder<AddColumnOperation> EmptyYard { get; set; }
        public OperationBuilder<AddColumnOperation> CommitNoChemicals { get; set; }
        public OperationBuilder<AddColumnOperation> CommitAttendClasses { get; set; }
        public OperationBuilder<AddColumnOperation> CommitNoCuttingTrees { get; set; }
        public OperationBuilder<AddColumnOperation> CommitStandForWomenChildren { get; set; }
        public OperationBuilder<AddColumnOperation> CommitCareWhileAway { get; set; }
        public OperationBuilder<AddColumnOperation> CommitAllowYardAccess { get; set; }
        public OperationBuilder<AddColumnOperation> SignatureCollected { get; set; }
        public OperationBuilder<AddColumnOperation> SignatureDate { get; set; }
        public OperationBuilder<AddColumnOperation> SignatureData { get; set; }
        public OperationBuilder<AddColumnOperation> Status { get; set; }
    }
}
