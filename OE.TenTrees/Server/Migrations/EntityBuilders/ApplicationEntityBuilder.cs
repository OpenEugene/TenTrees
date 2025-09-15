using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using Oqtane.Migrations.EntityBuilders;

namespace OE.TenTrees.Migrations.EntityBuilders
{
    public class ApplicationEntityBuilder : AuditableBaseEntityBuilder<ApplicationEntityBuilder>
    {
        private const string _entityTableName = "TreePlantingApplication";
        private readonly PrimaryKey<ApplicationEntityBuilder> _primaryKey = new("PK_TreePlantingApplication", x => x.ApplicationId);
        private readonly ForeignKey<ApplicationEntityBuilder> _moduleForeignKey = new("FK_TreePlantingApplication_Module", x => x.ModuleId, "Module", "ModuleId", ReferentialAction.Cascade);

        public ApplicationEntityBuilder(MigrationBuilder migrationBuilder, IDatabase database) : base(migrationBuilder, database)
        {
            EntityTableName = _entityTableName;
            PrimaryKey = _primaryKey;
            ForeignKeys.Add(_moduleForeignKey);
        }

        protected override ApplicationEntityBuilder BuildTable(ColumnsBuilder table)
        {
            ApplicationId = AddAutoIncrementColumn(table, "ApplicationId");
            ModuleId = AddIntegerColumn(table, "ModuleId");
            EvaluatorName = AddStringColumn(table, "EvaluatorName", 200, true);
            ApplicantUserId = AddStringColumn(table, "ApplicantUserId", 256, true);
            BeneficiaryName = AddStringColumn(table, "BeneficiaryName", 200, true);
            HouseholdIdentifier = AddStringColumn(table, "HouseholdIdentifier", 100, true);
            Address = AddStringColumn(table, "Address", 500, true);
            Village = AddStringColumn(table, "Village", 200, true);
            HouseholdSize = AddIntegerColumn(table, "HouseholdSize", true);
            SubmissionDate = AddDateTimeColumn(table, "SubmissionDate", true);
            
            HasExistingGardenInterest = AddBooleanColumn(table, "HasExistingGardenInterest");
            GardenCurrentlyTended = AddBooleanColumn(table, "GardenCurrentlyTended");
            ChildHeadedHousehold = AddBooleanColumn(table, "ChildHeadedHousehold");
            WomanHeadedHousehold = AddBooleanColumn(table, "WomanHeadedHousehold");
            EmptyOrNearlyEmptyYard = AddBooleanColumn(table, "EmptyOrNearlyEmptyYard");
            
            CommitNoChemicals = AddBooleanColumn(table, "CommitNoChemicals");
            CommitAttendTraining = AddBooleanColumn(table, "CommitAttendTraining");
            CommitNotCutTrees = AddBooleanColumn(table, "CommitNotCutTrees");
            CommitStandAgainstAbuse = AddBooleanColumn(table, "CommitStandAgainstAbuse");
            
            Status = AddIntegerColumn(table, "Status");
            RejectionReason = AddStringColumn(table, "RejectionReason", 1000, true);
            Notes = AddStringColumn(table, "Notes", 2000, true);
            
            AddAuditableColumns(table);
            return this;
        }

        public OperationBuilder<AddColumnOperation> ApplicationId { get; set; }
        public OperationBuilder<AddColumnOperation> ModuleId { get; set; }
        public OperationBuilder<AddColumnOperation> EvaluatorName { get; set; }
        public OperationBuilder<AddColumnOperation> ApplicantUserId { get; set; }
        public OperationBuilder<AddColumnOperation> BeneficiaryName { get; set; }
        public OperationBuilder<AddColumnOperation> HouseholdIdentifier { get; set; }
        public OperationBuilder<AddColumnOperation> Address { get; set; }
        public OperationBuilder<AddColumnOperation> Village { get; set; }
        public OperationBuilder<AddColumnOperation> HouseholdSize { get; set; }
        public OperationBuilder<AddColumnOperation> SubmissionDate { get; set; }
        public OperationBuilder<AddColumnOperation> HasExistingGardenInterest { get; set; }
        public OperationBuilder<AddColumnOperation> GardenCurrentlyTended { get; set; }
        public OperationBuilder<AddColumnOperation> ChildHeadedHousehold { get; set; }
        public OperationBuilder<AddColumnOperation> WomanHeadedHousehold { get; set; }
        public OperationBuilder<AddColumnOperation> EmptyOrNearlyEmptyYard { get; set; }
        public OperationBuilder<AddColumnOperation> CommitNoChemicals { get; set; }
        public OperationBuilder<AddColumnOperation> CommitAttendTraining { get; set; }
        public OperationBuilder<AddColumnOperation> CommitNotCutTrees { get; set; }
        public OperationBuilder<AddColumnOperation> CommitStandAgainstAbuse { get; set; }
        public OperationBuilder<AddColumnOperation> Status { get; set; }
        public OperationBuilder<AddColumnOperation> RejectionReason { get; set; }
        public OperationBuilder<AddColumnOperation> Notes { get; set; }
    }

    public class ApplicationDocumentEntityBuilder : AuditableBaseEntityBuilder<ApplicationDocumentEntityBuilder>
    {
        private const string _entityTableName = "ApplicationDocument";
        private readonly PrimaryKey<ApplicationDocumentEntityBuilder> _primaryKey = new("PK_ApplicationDocument", x => x.DocumentId);
        private readonly ForeignKey<ApplicationDocumentEntityBuilder> _applicationForeignKey = new("FK_ApplicationDocument_Application", x => x.ApplicationId, "TreePlantingApplication", "ApplicationId", ReferentialAction.Cascade);

        public ApplicationDocumentEntityBuilder(MigrationBuilder migrationBuilder, IDatabase database) : base(migrationBuilder, database)
        {
            EntityTableName = _entityTableName;
            PrimaryKey = _primaryKey;
            ForeignKeys.Add(_applicationForeignKey);
        }

        protected override ApplicationDocumentEntityBuilder BuildTable(ColumnsBuilder table)
        {
            DocumentId = AddAutoIncrementColumn(table, "DocumentId");
            ApplicationId = AddIntegerColumn(table, "ApplicationId");
            DocumentType = AddIntegerColumn(table, "DocumentType");
            FileName = AddStringColumn(table, "FileName", 255);
            ContentType = AddStringColumn(table, "ContentType", 100, true);
            FileSize = AddIntegerColumn(table, "FileSize"); // Use integer instead of long
            Url = AddStringColumn(table, "Url", 500, true);
            Caption = AddStringColumn(table, "Caption", 500, true);
            AddAuditableColumns(table);
            return this;
        }

        public OperationBuilder<AddColumnOperation> DocumentId { get; set; }
        public OperationBuilder<AddColumnOperation> ApplicationId { get; set; }
        public OperationBuilder<AddColumnOperation> DocumentType { get; set; }
        public OperationBuilder<AddColumnOperation> FileName { get; set; }
        public OperationBuilder<AddColumnOperation> ContentType { get; set; }
        public OperationBuilder<AddColumnOperation> FileSize { get; set; }
        public OperationBuilder<AddColumnOperation> Url { get; set; }
        public OperationBuilder<AddColumnOperation> Caption { get; set; }
    }

    public class ApplicationStatusHistoryEntityBuilder : AuditableBaseEntityBuilder<ApplicationStatusHistoryEntityBuilder>
    {
        private const string _entityTableName = "ApplicationStatusHistory";
        private readonly PrimaryKey<ApplicationStatusHistoryEntityBuilder> _primaryKey = new("PK_ApplicationStatusHistory", x => x.HistoryId);
        private readonly ForeignKey<ApplicationStatusHistoryEntityBuilder> _applicationForeignKey = new("FK_ApplicationStatusHistory_Application", x => x.ApplicationId, "TreePlantingApplication", "ApplicationId", ReferentialAction.Cascade);

        public ApplicationStatusHistoryEntityBuilder(MigrationBuilder migrationBuilder, IDatabase database) : base(migrationBuilder, database)
        {
            EntityTableName = _entityTableName;
            PrimaryKey = _primaryKey;
            ForeignKeys.Add(_applicationForeignKey);
        }

        protected override ApplicationStatusHistoryEntityBuilder BuildTable(ColumnsBuilder table)
        {
            HistoryId = AddAutoIncrementColumn(table, "HistoryId");
            ApplicationId = AddIntegerColumn(table, "ApplicationId");
            Status = AddIntegerColumn(table, "Status");
            ChangedByUserId = AddStringColumn(table, "ChangedByUserId", 256, true);
            Comment = AddStringColumn(table, "Comment", 1000, true);
            AddAuditableColumns(table);
            return this;
        }

        public OperationBuilder<AddColumnOperation> HistoryId { get; set; }
        public OperationBuilder<AddColumnOperation> ApplicationId { get; set; }
        public OperationBuilder<AddColumnOperation> Status { get; set; }
        public OperationBuilder<AddColumnOperation> ChangedByUserId { get; set; }
        public OperationBuilder<AddColumnOperation> Comment { get; set; }
    }

    public class ApplicationReviewEntityBuilder : AuditableBaseEntityBuilder<ApplicationReviewEntityBuilder>
    {
        private const string _entityTableName = "ApplicationReview";
        private readonly PrimaryKey<ApplicationReviewEntityBuilder> _primaryKey = new("PK_ApplicationReview", x => x.ReviewId);
        private readonly ForeignKey<ApplicationReviewEntityBuilder> _applicationForeignKey = new("FK_ApplicationReview_Application", x => x.ApplicationId, "TreePlantingApplication", "ApplicationId", ReferentialAction.Cascade);

        public ApplicationReviewEntityBuilder(MigrationBuilder migrationBuilder, IDatabase database) : base(migrationBuilder, database)
        {
            EntityTableName = _entityTableName;
            PrimaryKey = _primaryKey;
            ForeignKeys.Add(_applicationForeignKey);
        }

        protected override ApplicationReviewEntityBuilder BuildTable(ColumnsBuilder table)
        {
            ReviewId = AddAutoIncrementColumn(table, "ReviewId");
            ApplicationId = AddIntegerColumn(table, "ApplicationId");
            ReviewerUserId = AddStringColumn(table, "ReviewerUserId", 256, true);
            ReviewDate = AddDateTimeColumn(table, "ReviewDate");
            Decision = AddIntegerColumn(table, "Decision");
            Summary = AddStringColumn(table, "Summary", 1000, true);
            Comments = AddStringColumn(table, "Comments", 2000, true);
            RequiresFollowUp = AddBooleanColumn(table, "RequiresFollowUp");
            AddAuditableColumns(table);
            return this;
        }

        public OperationBuilder<AddColumnOperation> ReviewId { get; set; }
        public OperationBuilder<AddColumnOperation> ApplicationId { get; set; }
        public OperationBuilder<AddColumnOperation> ReviewerUserId { get; set; }
        public OperationBuilder<AddColumnOperation> ReviewDate { get; set; }
        public OperationBuilder<AddColumnOperation> Decision { get; set; }
        public OperationBuilder<AddColumnOperation> Summary { get; set; }
        public OperationBuilder<AddColumnOperation> Comments { get; set; }
        public OperationBuilder<AddColumnOperation> RequiresFollowUp { get; set; }
    }

    public class ReviewChecklistItemEntityBuilder : AuditableBaseEntityBuilder<ReviewChecklistItemEntityBuilder>
    {
        private const string _entityTableName = "ReviewChecklistItem";
        private readonly PrimaryKey<ReviewChecklistItemEntityBuilder> _primaryKey = new("PK_ReviewChecklistItem", x => x.ChecklistItemId);
        private readonly ForeignKey<ReviewChecklistItemEntityBuilder> _reviewForeignKey = new("FK_ReviewChecklistItem_Review", x => x.ReviewId, "ApplicationReview", "ReviewId", ReferentialAction.Cascade);

        public ReviewChecklistItemEntityBuilder(MigrationBuilder migrationBuilder, IDatabase database) : base(migrationBuilder, database)
        {
            EntityTableName = _entityTableName;
            PrimaryKey = _primaryKey;
            ForeignKeys.Add(_reviewForeignKey);
        }

        protected override ReviewChecklistItemEntityBuilder BuildTable(ColumnsBuilder table)
        {
            ChecklistItemId = AddAutoIncrementColumn(table, "ChecklistItemId");
            ReviewId = AddIntegerColumn(table, "ReviewId");
            Item = AddStringColumn(table, "Item", 500);
            IsComplete = AddBooleanColumn(table, "IsComplete");
            Notes = AddStringColumn(table, "Notes", 1000, true);
            AddAuditableColumns(table);
            return this;
        }

        public OperationBuilder<AddColumnOperation> ChecklistItemId { get; set; }
        public OperationBuilder<AddColumnOperation> ReviewId { get; set; }
        public OperationBuilder<AddColumnOperation> Item { get; set; }
        public OperationBuilder<AddColumnOperation> IsComplete { get; set; }
        public OperationBuilder<AddColumnOperation> Notes { get; set; }
    }
}