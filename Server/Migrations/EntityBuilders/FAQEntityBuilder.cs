using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using Oqtane.Migrations.EntityBuilders;

namespace GIBS.Module.FAQ.Migrations.EntityBuilders
{
    public class FAQEntityBuilder : AuditableBaseEntityBuilder<FAQEntityBuilder>
    {
        private const string _entityTableName = "GIBSFAQ";
        private readonly PrimaryKey<FAQEntityBuilder> _primaryKey = new("PK_GIBSFAQ", x => x.FAQId);
        private readonly ForeignKey<FAQEntityBuilder> _moduleForeignKey = new("FK_GIBSFAQ_Module", x => x.ModuleId, "Module", "ModuleId", ReferentialAction.Cascade);
        private readonly ForeignKey<FAQEntityBuilder> _categoryForeignKey = new("FK_GIBSFAQ_Category", x => x.CategoryId, "GIBSFAQ_Category", "CategoryId", ReferentialAction.NoAction);

        public FAQEntityBuilder(MigrationBuilder migrationBuilder, IDatabase database) : base(migrationBuilder, database)
        {
            EntityTableName = _entityTableName;
            PrimaryKey = _primaryKey;
            ForeignKeys.Add(_moduleForeignKey);
            ForeignKeys.Add(_categoryForeignKey);
        }

        protected override FAQEntityBuilder BuildTable(ColumnsBuilder table)
        {
            FAQId = AddAutoIncrementColumn(table, "FAQId");
            ModuleId = AddIntegerColumn(table, "ModuleId");
            Question = AddMaxStringColumn(table, "Question");
            Answer = AddMaxStringColumn(table, "Answer", true);
            CategoryId = AddIntegerColumn(table, "CategoryId");
            SortOrder = AddIntegerColumn(table, "SortOrder");
            Status = AddStringColumn(table, "Status", 20);
            ViewCount = AddIntegerColumn(table, "ViewCount", true, 0);
            AddAuditableColumns(table);
            return this;
        }

        public OperationBuilder<AddColumnOperation> FAQId { get; set; }
        public OperationBuilder<AddColumnOperation> ModuleId { get; set; }
        public OperationBuilder<AddColumnOperation> Question { get; set; }
        public OperationBuilder<AddColumnOperation> Answer { get; set; }
        public OperationBuilder<AddColumnOperation> CategoryId { get; set; }
        public OperationBuilder<AddColumnOperation> SortOrder { get; set; }
        public OperationBuilder<AddColumnOperation> Status { get; set; }
        public OperationBuilder<AddColumnOperation> ViewCount { get; set; }
    }
}
