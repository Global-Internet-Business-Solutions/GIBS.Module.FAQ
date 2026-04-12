using Microsoft.EntityFrameworkCore;
using Oqtane.Modules;
using System.Collections.Generic;
using System.Linq;

namespace GIBS.Module.FAQ.Repository
{
    public interface ICategoryRepository
    {
        IEnumerable<Models.Category> GetCategories(int ModuleId);
        Models.Category GetCategory(int CategoryId);
        Models.Category GetCategory(int CategoryId, bool tracking);
        Models.Category AddCategory(Models.Category Category);
        Models.Category UpdateCategory(Models.Category Category);
        void DeleteCategory(int CategoryId);
    }

    public class CategoryRepository : ICategoryRepository, ITransientService
    {
        private readonly IDbContextFactory<FAQContext> _factory;

        public CategoryRepository(IDbContextFactory<FAQContext> factory)
        {
            _factory = factory;
        }

        public IEnumerable<Models.Category> GetCategories(int ModuleId)
        {
            using var db = _factory.CreateDbContext();
            return db.Category.Where(item => item.ModuleId == ModuleId).ToList();
        }

        public Models.Category GetCategory(int CategoryId)
        {
            return GetCategory(CategoryId, true);
        }

        public Models.Category GetCategory(int CategoryId, bool tracking)
        {
            using var db = _factory.CreateDbContext();
            if (tracking)
            {
                return db.Category.Find(CategoryId);
            }
            else
            {
                return db.Category.AsNoTracking().FirstOrDefault(item => item.CategoryId == CategoryId);
            }
        }

        public Models.Category AddCategory(Models.Category Category)
        {
            using var db = _factory.CreateDbContext();
            db.Category.Add(Category);
            db.SaveChanges();
            return Category;
        }

        public Models.Category UpdateCategory(Models.Category Category)
        {
            using var db = _factory.CreateDbContext();
            db.Entry(Category).State = EntityState.Modified;
            db.SaveChanges();
            return Category;
        }

        public void DeleteCategory(int CategoryId)
        {
            using var db = _factory.CreateDbContext();
            Models.Category Category = db.Category.Find(CategoryId);
            db.Category.Remove(Category);
            db.SaveChanges();
        }
    }
}
