using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Oqtane.Modules;

namespace GIBS.Module.FAQ.Repository
{
    public interface IFAQRepository
    {
        IEnumerable<Models.FAQ> GetFAQs(int ModuleId);
        Models.FAQ GetFAQ(int FAQId);
        Models.FAQ GetFAQ(int FAQId, bool tracking);
        Models.FAQ AddFAQ(Models.FAQ FAQ);
        Models.FAQ UpdateFAQ(Models.FAQ FAQ);
        void DeleteFAQ(int FAQId);
    }

    public class FAQRepository : IFAQRepository, ITransientService
    {
        private readonly IDbContextFactory<FAQContext> _factory;

        public FAQRepository(IDbContextFactory<FAQContext> factory)
        {
            _factory = factory;
        }

        public IEnumerable<Models.FAQ> GetFAQs(int ModuleId)
        {
            using var db = _factory.CreateDbContext();
            return db.FAQ.Where(item => item.ModuleId == ModuleId).ToList();
        }

        public Models.FAQ GetFAQ(int FAQId)
        {
            return GetFAQ(FAQId, true);
        }

        public Models.FAQ GetFAQ(int FAQId, bool tracking)
        {
            using var db = _factory.CreateDbContext();
            if (tracking)
            {
                return db.FAQ.Find(FAQId);
            }
            else
            {
                return db.FAQ.AsNoTracking().FirstOrDefault(item => item.FAQId == FAQId);
            }
        }

        public Models.FAQ AddFAQ(Models.FAQ FAQ)
        {
            using var db = _factory.CreateDbContext();
            db.FAQ.Add(FAQ);
            db.SaveChanges();
            return FAQ;
        }

        public Models.FAQ UpdateFAQ(Models.FAQ FAQ)
        {
            using var db = _factory.CreateDbContext();
            db.Entry(FAQ).State = EntityState.Modified;
            db.SaveChanges();
            return FAQ;
        }

        public void DeleteFAQ(int FAQId)
        {
            using var db = _factory.CreateDbContext();
            Models.FAQ FAQ = db.FAQ.Find(FAQId);
            db.FAQ.Remove(FAQ);
            db.SaveChanges();
        }
    }
}
