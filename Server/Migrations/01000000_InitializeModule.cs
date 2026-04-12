using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using GIBS.Module.FAQ.Migrations.EntityBuilders;
using GIBS.Module.FAQ.Repository;

namespace GIBS.Module.FAQ.Migrations
{
    [DbContext(typeof(FAQContext))]
    [Migration("GIBS.Module.FAQ.01.00.00.00")]
    public class InitializeModule : MultiDatabaseMigration
    {
        public InitializeModule(IDatabase database) : base(database)
        {
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var categoryEntityBuilder = new CategoryEntityBuilder(migrationBuilder, ActiveDatabase);
            categoryEntityBuilder.Create();

            var faqEntityBuilder = new FAQEntityBuilder(migrationBuilder, ActiveDatabase);
            faqEntityBuilder.Create();
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var faqEntityBuilder = new FAQEntityBuilder(migrationBuilder, ActiveDatabase);
            faqEntityBuilder.Drop();

            var categoryEntityBuilder = new CategoryEntityBuilder(migrationBuilder, ActiveDatabase);
            categoryEntityBuilder.Drop();
        }
    }
}
