using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using Oqtane.Migrations.EntityBuilders;

namespace GIBS.Module.FAQ.Migrations.EntityBuilders
{
    public class CategoryEntityBuilder : AuditableBaseEntityBuilder<CategoryEntityBuilder>
    {
        private const string _entityTableName = "GIBSFAQ_Category";
        private readonly PrimaryKey<CategoryEntityBuilder> _primaryKey = new("PK_GIBSFAQ_Category", x => x.CategoryId);
        private readonly ForeignKey<CategoryEntityBuilder> _moduleForeignKey = new("FK_GIBSFAQ_Category_Module", x => x.ModuleId, "Module", "ModuleId", ReferentialAction.Cascade);

        public CategoryEntityBuilder(MigrationBuilder migrationBuilder, IDatabase database) : base(migrationBuilder, database)
        {
            EntityTableName = _entityTableName;
            PrimaryKey = _primaryKey;
            ForeignKeys.Add(_moduleForeignKey);
        }

        protected override CategoryEntityBuilder BuildTable(ColumnsBuilder table)
        {
            CategoryId = AddAutoIncrementColumn(table, "CategoryId");
            ModuleId = AddIntegerColumn(table, "ModuleId");
            Name = AddMaxStringColumn(table, "Name");
            Slug = AddMaxStringColumn(table, "Slug");
            ParentId = AddIntegerColumn(table, "ParentId");
            SortOrder = AddIntegerColumn(table, "SortOrder");
            IsActive = AddBooleanColumn(table, "IsActive");
            AddAuditableColumns(table);
            return this;
        }

        public OperationBuilder<AddColumnOperation> CategoryId { get; set; }
        public OperationBuilder<AddColumnOperation> ModuleId { get; set; }
        public OperationBuilder<AddColumnOperation> Name { get; set; }
        public OperationBuilder<AddColumnOperation> Slug { get; set; }
        public OperationBuilder<AddColumnOperation> ParentId { get; set; }
        public OperationBuilder<AddColumnOperation> SortOrder { get; set; }
        public OperationBuilder<AddColumnOperation> IsActive { get; set; }
    }
}
