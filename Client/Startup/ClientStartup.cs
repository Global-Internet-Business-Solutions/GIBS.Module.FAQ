using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Oqtane.Services;
using GIBS.Module.FAQ.Services;

namespace GIBS.Module.FAQ.Startup
{
    public class ClientStartup : IClientStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            if (!services.Any(s => s.ServiceType == typeof(IFAQService)))
            {
                services.AddScoped<IFAQService, ClientFAQService>();
            }

            if (!services.Any(s => s.ServiceType == typeof(ICategoryService)))
            {
                services.AddScoped<ICategoryService, ClientCategoryService>();
            }
        }
    }
}
