using GIBS.Module.FAQ.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oqtane.Controllers;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Oqtane.Shared;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace GIBS.Module.FAQ.Controllers
{
    [Route(ControllerRoutes.ApiRoute)]
    public class CategoryController : ModuleControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService, ILogManager logger, IHttpContextAccessor accessor) : base(logger, accessor)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<IEnumerable<Models.Category>> Get(string moduleid)
        {
            if (int.TryParse(moduleid, out int ModuleId) && IsAuthorizedEntityId(EntityNames.Module, ModuleId))
            {
                return await _categoryService.GetCategoriesAsync(ModuleId);
            }
            _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Category Get Attempt {ModuleId}", moduleid);
            HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            return null;
        }

        [HttpGet("{id}/{moduleid}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<Models.Category> Get(int id, int moduleid)
        {
            Models.Category category = await _categoryService.GetCategoryAsync(id, moduleid);
            if (category != null && IsAuthorizedEntityId(EntityNames.Module, category.ModuleId))
            {
                return category;
            }
            _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Category Get Attempt {CategoryId} {ModuleId}", id, moduleid);
            HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            return null;
        }

        [HttpPost]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<Models.Category> Post([FromBody] Models.Category category)
        {
            if (ModelState.IsValid && IsAuthorizedEntityId(EntityNames.Module, category.ModuleId))
            {
                return await _categoryService.AddCategoryAsync(category);
            }
            _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Category Post Attempt {Category}", category);
            HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            return null;
        }

        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<Models.Category> Put(int id, [FromBody] Models.Category category)
        {
            if (ModelState.IsValid && category.CategoryId == id && IsAuthorizedEntityId(EntityNames.Module, category.ModuleId))
            {
                return await _categoryService.UpdateCategoryAsync(category);
            }
            _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Category Put Attempt {Category}", category);
            HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            return null;
        }

        [HttpDelete("{id}/{moduleid}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task Delete(int id, int moduleid)
        {
            Models.Category category = await _categoryService.GetCategoryAsync(id, moduleid);
            if (category != null && IsAuthorizedEntityId(EntityNames.Module, category.ModuleId))
            {
                await _categoryService.DeleteCategoryAsync(id, category.ModuleId);
                return;
            }
            _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Category Delete Attempt {CategoryId} {ModuleId}", id, moduleid);
            HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
        }
    }
}
