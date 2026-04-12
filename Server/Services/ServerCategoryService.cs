using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GIBS.Module.FAQ.Repository;
using Microsoft.AspNetCore.Http;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Oqtane.Interfaces;
using Oqtane.Models;
using Oqtane.Security;
using Oqtane.Shared;

namespace GIBS.Module.FAQ.Services
{
    public class ServerCategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserPermissions _userPermissions;
        private readonly ILogManager _logger;
        private readonly IHttpContextAccessor _accessor;
        private readonly Alias _alias;

        public ServerCategoryService(ICategoryRepository categoryRepository, IUserPermissions userPermissions, ITenantManager tenantManager, ILogManager logger, IHttpContextAccessor accessor)
        {
            _categoryRepository = categoryRepository;
            _userPermissions = userPermissions;
            _logger = logger;
            _accessor = accessor;
            _alias = tenantManager.GetAlias();
        }

        public Task<List<Models.Category>> GetCategoriesAsync(int ModuleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.View))
            {
                return Task.FromResult(_categoryRepository.GetCategories(ModuleId).ToList());
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Category Get Attempt {ModuleId}", ModuleId);
                return null;
            }
        }

        public Task<Models.Category> GetCategoryAsync(int CategoryId, int ModuleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.View))
            {
                return Task.FromResult(_categoryRepository.GetCategory(CategoryId));
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Category Get Attempt {CategoryId} {ModuleId}", CategoryId, ModuleId);
                return null;
            }
        }

        public Task<Models.Category> AddCategoryAsync(Models.Category Category)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, Category.ModuleId, PermissionNames.Edit))
            {
                Category = _categoryRepository.AddCategory(Category);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "Category Added {Category}", Category);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Category Add Attempt {Category}", Category);
                Category = null;
            }
            return Task.FromResult(Category);
        }

        public Task<Models.Category> UpdateCategoryAsync(Models.Category Category)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, Category.ModuleId, PermissionNames.Edit))
            {
                Category = _categoryRepository.UpdateCategory(Category);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "Category Updated {Category}", Category);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Category Update Attempt {Category}", Category);
                Category = null;
            }
            return Task.FromResult(Category);
        }

        public Task DeleteCategoryAsync(int CategoryId, int ModuleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.Edit))
            {
                _categoryRepository.DeleteCategory(CategoryId);
                _logger.Log(LogLevel.Information, this, LogFunction.Delete, "Category Deleted {CategoryId}", CategoryId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Category Delete Attempt {CategoryId} {ModuleId}", CategoryId, ModuleId);
            }
            return Task.CompletedTask;
        }
    }
}
