using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Oqtane.Models;
using Oqtane.Repository;
using Oqtane.Security;
using Oqtane.Shared;
using GIBS.Module.FAQ.Repository;

namespace GIBS.Module.FAQ.Services
{
    public class ServerFAQService : IFAQService
    {
        private readonly IFAQRepository _FAQRepository;
        private readonly IUserPermissions _userPermissions;
        private readonly ILogManager _logger;
        private readonly IHttpContextAccessor _accessor;
        private readonly INotificationRepository _notifications;
        private readonly IUserRoleRepository _userRoles;
        private readonly Alias _alias;

        public ServerFAQService(IFAQRepository FAQRepository, IUserPermissions userPermissions, ITenantManager tenantManager, ILogManager logger, IHttpContextAccessor accessor, INotificationRepository notifications, IUserRoleRepository userRoles)
        {
            _FAQRepository = FAQRepository;
            _userPermissions = userPermissions;
            _logger = logger;
            _accessor = accessor;
            _notifications = notifications;
            _userRoles = userRoles;
            _alias = tenantManager.GetAlias();
        }

        public Task<List<Models.FAQ>> GetFAQsAsync(int ModuleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.View))
            {
                return Task.FromResult(_FAQRepository.GetFAQs(ModuleId).ToList());
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized FAQ Get Attempt {ModuleId}", ModuleId);
                return null;
            }
        }

        public Task<Models.FAQ> GetFAQAsync(int FAQId, int ModuleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.View))
            {
                return Task.FromResult(_FAQRepository.GetFAQ(FAQId));
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized FAQ Get Attempt {FAQId} {ModuleId}", FAQId, ModuleId);
                return null;
            }
        }

        public Task<Models.FAQ> AddFAQAsync(Models.FAQ FAQ)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, FAQ.ModuleId, PermissionNames.Edit))
            {
                FAQ = _FAQRepository.AddFAQ(FAQ);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "FAQ Added {FAQ}", FAQ);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized FAQ Add Attempt {FAQ}", FAQ);
                FAQ = null;
            }
            return Task.FromResult(FAQ);
        }

        public Task<Models.FAQ> SubmitQuestionAsync(Models.FAQ FAQ)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, FAQ.ModuleId, PermissionNames.View))
            {
                FAQ.FAQId = 0;
                FAQ.Answer ??= string.Empty;
                FAQ.Status = string.IsNullOrWhiteSpace(FAQ.Status) ? "draft" : FAQ.Status;
                FAQ.SortOrder = 0;
                FAQ.ViewCount = 0;

                var submitterName = FAQ.SubmitterName;
                var submitterEmail = FAQ.SubmitterEmail;
                var question = FAQ.Question;
                var moduleId = FAQ.ModuleId;

                FAQ = _FAQRepository.AddFAQ(FAQ);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "FAQ Question Submitted {FAQ}", FAQ);

                NotifyAdministrators(_alias.SiteId, submitterName, submitterEmail, question, moduleId, FAQ.FAQId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized FAQ Submit Attempt {FAQ}", FAQ);
                FAQ = null;
            }
            return Task.FromResult(FAQ);
        }

        public Task<Models.FAQ> UpdateFAQAsync(Models.FAQ FAQ)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, FAQ.ModuleId, PermissionNames.Edit))
            {
                FAQ = _FAQRepository.UpdateFAQ(FAQ);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "FAQ Updated {FAQ}", FAQ);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized FAQ Update Attempt {FAQ}", FAQ);
                FAQ = null;
            }
            return Task.FromResult(FAQ);
        }

        public Task DeleteFAQAsync(int FAQId, int ModuleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.Edit))
            {
                _FAQRepository.DeleteFAQ(FAQId);
                _logger.Log(LogLevel.Information, this, LogFunction.Delete, "FAQ Deleted {FAQId}", FAQId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized FAQ Delete Attempt {FAQId} {ModuleId}", FAQId, ModuleId);
            }
            return Task.CompletedTask;
        }

        public Task IncrementViewCountAsync(int FAQId, int ModuleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.View))
            {
                _FAQRepository.IncrementViewCount(FAQId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized FAQ ViewCount Increment Attempt {FAQId} {ModuleId}", FAQId, ModuleId);
            }
            return Task.CompletedTask;
        }

        private void NotifyAdministrators(int siteId, string submitterName, string submitterEmail, string question, int moduleId, int faqId)
        {
            var siteUrl = $"{_alias.Protocol}{_alias.Name}";
            var editLink = $"{siteUrl}/faq/*/{moduleId}/Edit?id={faqId}";

            var subject = "New FAQ Question Submitted";
            var body = $"<p>A new question has been submitted for review.</p>" +
                       $"<p><strong>From:</strong> {submitterName} &lt;{submitterEmail}&gt;</p>" +
                       $"<p><strong>Question:</strong></p>" +
                       $"<p>{question}</p>" +
                       $"<p><a href=\"{editLink}\">Click here to review and manage this question</a></p>";

            var adminRoleUsers = _userRoles.GetUserRoles(RoleNames.Admin, siteId);
            foreach (var userRole in adminRoleUsers)
            {
                var notification = new Notification(siteId, submitterName, submitterEmail, userRole.User.DisplayName, userRole.User.Email, subject, body);
                _notifications.AddNotification(notification);
            }
        }
    }
}
