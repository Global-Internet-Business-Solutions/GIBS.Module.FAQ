using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIBS.Module.FAQ.Services
{
    public interface IFAQService 
    {
        Task<List<Models.FAQ>> GetFAQsAsync(int ModuleId);

        Task<Models.FAQ> GetFAQAsync(int FAQId, int ModuleId);

        Task<Models.FAQ> AddFAQAsync(Models.FAQ FAQ);

        Task<Models.FAQ> UpdateFAQAsync(Models.FAQ FAQ);

        Task DeleteFAQAsync(int FAQId, int ModuleId);
    }
}
