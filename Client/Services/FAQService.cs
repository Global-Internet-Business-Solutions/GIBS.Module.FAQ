using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Oqtane.Services;
using Oqtane.Shared;

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

    public class FAQService : ServiceBase, IFAQService
    {
        public FAQService(HttpClient http, SiteState siteState) : base(http, siteState) { }

        private string Apiurl => CreateApiUrl("FAQ");

        public async Task<List<Models.FAQ>> GetFAQsAsync(int ModuleId)
        {
            List<Models.FAQ> FAQs = await GetJsonAsync<List<Models.FAQ>>(CreateAuthorizationPolicyUrl($"{Apiurl}?moduleid={ModuleId}", EntityNames.Module, ModuleId), Enumerable.Empty<Models.FAQ>().ToList());
            return FAQs.OrderBy(item => item.Name).ToList();
        }

        public async Task<Models.FAQ> GetFAQAsync(int FAQId, int ModuleId)
        {
            return await GetJsonAsync<Models.FAQ>(CreateAuthorizationPolicyUrl($"{Apiurl}/{FAQId}/{ModuleId}", EntityNames.Module, ModuleId));
        }

        public async Task<Models.FAQ> AddFAQAsync(Models.FAQ FAQ)
        {
            return await PostJsonAsync<Models.FAQ>(CreateAuthorizationPolicyUrl($"{Apiurl}", EntityNames.Module, FAQ.ModuleId), FAQ);
        }

        public async Task<Models.FAQ> UpdateFAQAsync(Models.FAQ FAQ)
        {
            return await PutJsonAsync<Models.FAQ>(CreateAuthorizationPolicyUrl($"{Apiurl}/{FAQ.FAQId}", EntityNames.Module, FAQ.ModuleId), FAQ);
        }

        public async Task DeleteFAQAsync(int FAQId, int ModuleId)
        {
            await DeleteAsync(CreateAuthorizationPolicyUrl($"{Apiurl}/{FAQId}/{ModuleId}", EntityNames.Module, ModuleId));
        }
    }
}
