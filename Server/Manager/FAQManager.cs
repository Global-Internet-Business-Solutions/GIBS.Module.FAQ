using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Oqtane.Modules;
using Oqtane.Models;
using Oqtane.Infrastructure;
using Oqtane.Interfaces;
using Oqtane.Enums;
using Oqtane.Repository;
using GIBS.Module.FAQ.Repository;
using System.Threading.Tasks;

namespace GIBS.Module.FAQ.Manager
{
    public class FAQManager : MigratableModuleBase, IInstallable, IPortable, ISearchable
    {
        private readonly IFAQRepository _FAQRepository;
        private readonly IDBContextDependencies _DBContextDependencies;

        public FAQManager(IFAQRepository FAQRepository, IDBContextDependencies DBContextDependencies)
        {
            _FAQRepository = FAQRepository;
            _DBContextDependencies = DBContextDependencies;
        }

        public bool Install(Tenant tenant, string version)
        {
            return Migrate(new FAQContext(_DBContextDependencies), tenant, MigrationType.Up);
        }

        public bool Uninstall(Tenant tenant)
        {
            return Migrate(new FAQContext(_DBContextDependencies), tenant, MigrationType.Down);
        }

        public string ExportModule(Oqtane.Models.Module module)
        {
            string content = "";
            List<Models.FAQ> FAQs = _FAQRepository.GetFAQs(module.ModuleId).ToList();
            if (FAQs != null)
            {
                content = JsonSerializer.Serialize(FAQs);
            }
            return content;
        }

        public void ImportModule(Oqtane.Models.Module module, string content, string version)
        {
            List<Models.FAQ> FAQs = null;
            if (!string.IsNullOrEmpty(content))
            {
                FAQs = JsonSerializer.Deserialize<List<Models.FAQ>>(content);
            }
            if (FAQs != null)
            {
                foreach(var FAQ in FAQs)
                {
                    _FAQRepository.AddFAQ(new Models.FAQ { ModuleId = module.ModuleId, Name = FAQ.Name });
                }
            }
        }

        public Task<List<SearchContent>> GetSearchContentsAsync(PageModule pageModule, DateTime lastIndexedOn)
        {
           var searchContentList = new List<SearchContent>();

           foreach (var FAQ in _FAQRepository.GetFAQs(pageModule.ModuleId))
           {
               if (FAQ.ModifiedOn >= lastIndexedOn)
               {
                   searchContentList.Add(new SearchContent
                   {
                       EntityName = "GIBSFAQ",
                       EntityId = FAQ.FAQId.ToString(),
                       Title = FAQ.Name,
                       Body = FAQ.Name,
                       ContentModifiedBy = FAQ.ModifiedBy,
                       ContentModifiedOn = FAQ.ModifiedOn
                   });
               }
           }

           return Task.FromResult(searchContentList);
        }
    }
}
