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

        public FAQEntityBuilder(MigrationBuilder migrationBuilder, IDatabase database) : base(migrationBuilder, database)
        {
            EntityTableName = _entityTableName;
            PrimaryKey = _primaryKey;
            ForeignKeys.Add(_moduleForeignKey);
        }

        protected override FAQEntityBuilder BuildTable(ColumnsBuilder table)
        {
            FAQId = AddAutoIncrementColumn(table,"FAQId");
            ModuleId = AddIntegerColumn(table,"ModuleId");
            Name = AddMaxStringColumn(table,"Name");
            AddAuditableColumns(table);
            return this;
        }

        public OperationBuilder<AddColumnOperation> FAQId { get; set; }
        public OperationBuilder<AddColumnOperation> ModuleId { get; set; }
        public OperationBuilder<AddColumnOperation> Name { get; set; }
    }
}
