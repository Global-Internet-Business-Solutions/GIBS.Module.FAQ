using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Oqtane.Shared;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using GIBS.Module.FAQ.Services;
using Oqtane.Controllers;
using System.Net;
using System.Threading.Tasks;

namespace GIBS.Module.FAQ.Controllers
{
    [Route(ControllerRoutes.ApiRoute)]
    public class FAQController : ModuleControllerBase
    {
        private readonly IFAQService _FAQService;

        public FAQController(IFAQService FAQService, ILogManager logger, IHttpContextAccessor accessor) : base(logger, accessor)
        {
            _FAQService = FAQService;
        }

        // GET: api/<controller>?moduleid=x
        [HttpGet]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<IEnumerable<Models.FAQ>> Get(string moduleid)
        {
            int ModuleId;
            if (int.TryParse(moduleid, out ModuleId) && IsAuthorizedEntityId(EntityNames.Module, ModuleId))
            {
                return await _FAQService.GetFAQsAsync(ModuleId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized FAQ Get Attempt {ModuleId}", moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        // GET api/<controller>/5
        [HttpGet("{id}/{moduleid}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<Models.FAQ> Get(int id, int moduleid)
        {
            Models.FAQ FAQ = await _FAQService.GetFAQAsync(id, moduleid);
            if (FAQ != null && IsAuthorizedEntityId(EntityNames.Module, FAQ.ModuleId))
            {
                return FAQ;
            }
            else
            { 
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized FAQ Get Attempt {FAQId} {ModuleId}", id, moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<Models.FAQ> Post([FromBody] Models.FAQ FAQ)
        {
            if (ModelState.IsValid && IsAuthorizedEntityId(EntityNames.Module, FAQ.ModuleId))
            {
                FAQ = await _FAQService.AddFAQAsync(FAQ);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized FAQ Post Attempt {FAQ}", FAQ);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                FAQ = null;
            }
            return FAQ;
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<Models.FAQ> Put(int id, [FromBody] Models.FAQ FAQ)
        {
            if (ModelState.IsValid && FAQ.FAQId == id && IsAuthorizedEntityId(EntityNames.Module, FAQ.ModuleId))
            {
                FAQ = await _FAQService.UpdateFAQAsync(FAQ);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized FAQ Put Attempt {FAQ}", FAQ);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                FAQ = null;
            }
            return FAQ;
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}/{moduleid}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task Delete(int id, int moduleid)
        {
            Models.FAQ FAQ = await _FAQService.GetFAQAsync(id, moduleid);
            if (FAQ != null && IsAuthorizedEntityId(EntityNames.Module, FAQ.ModuleId))
            {
                await _FAQService.DeleteFAQAsync(id, FAQ.ModuleId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized FAQ Delete Attempt {FAQId} {ModuleId}", id, moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            }
        }
    }
}
