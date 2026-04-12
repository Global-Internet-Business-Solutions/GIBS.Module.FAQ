using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Oqtane.Modules;
using Oqtane.Repository;
using Oqtane.Infrastructure;
using Oqtane.Repository.Databases.Interfaces;

namespace GIBS.Module.FAQ.Repository
{
    public class FAQContext : DBContextBase, ITransientService, IMultiDatabase
    {
        public virtual DbSet<Models.FAQ> FAQ { get; set; }

        public FAQContext(IDBContextDependencies DBContextDependencies) : base(DBContextDependencies)
        {
            // ContextBase handles multi-tenant database connections
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Models.FAQ>().ToTable(ActiveDatabase.RewriteName("GIBSFAQ"));
        }
    }
}
