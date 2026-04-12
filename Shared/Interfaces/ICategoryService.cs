using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIBS.Module.FAQ.Services
{
    public interface ICategoryService
    {
        Task<List<Models.Category>> GetCategoriesAsync(int ModuleId);

        Task<Models.Category> GetCategoryAsync(int CategoryId, int ModuleId);

        Task<Models.Category> AddCategoryAsync(Models.Category Category);

        Task<Models.Category> UpdateCategoryAsync(Models.Category Category);

        Task DeleteCategoryAsync(int CategoryId, int ModuleId);
    }
}
