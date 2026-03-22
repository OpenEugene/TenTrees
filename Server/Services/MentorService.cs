using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Oqtane.Managers;
using Oqtane.Models;
using Oqtane.Repository;
using Oqtane.Shared;
using OpenEug.TenTrees.Models;
using OpenEug.TenTrees.Module.Mentor.Repository;
using OpenEug.TenTrees.Module.Village.Repository;
using AppRoleNames = OpenEug.TenTrees.Shared.RoleNames;

namespace OpenEug.TenTrees.Module.Mentor.Services
{
    public class ServerMentorService : IMentorService
    {
        private readonly IMentorRepository _mentorRepository;
        private readonly IVillageRepository _villageRepository;
        private readonly IUserManager _userManager;
        private readonly IRoleRepository _roles;
        private readonly IUserRoleRepository _userRoles;
        private readonly ILogManager _logger;
        private readonly IHttpContextAccessor _accessor;
        private readonly Alias _alias;

        public ServerMentorService(
            IMentorRepository mentorRepository,
            IVillageRepository villageRepository,
            IUserManager userManager,
            IRoleRepository roles,
            IUserRoleRepository userRoles,
            ITenantManager tenantManager,
            ILogManager logger,
            IHttpContextAccessor accessor)
        {
            _mentorRepository = mentorRepository;
            _villageRepository = villageRepository;
            _userManager = userManager;
            _roles = roles;
            _userRoles = userRoles;
            _logger = logger;
            _accessor = accessor;
            _alias = tenantManager.GetAlias();
        }

        public Task<List<MentorViewModel>> GetMentorsAsync(int moduleId)
        {
            var mentorRoleUsers = _userRoles.GetUserRoles(AppRoleNames.Mentor, _alias.SiteId)
                .Where(ur => ur.User != null)
                .ToList();

            var villageIds = _mentorRepository.GetAllVillageIds();
            var growerCounts = _mentorRepository.GetGrowerCountsByMentor();
            var villages = _villageRepository.GetVillages().ToDictionary(v => v.VillageId, v => v.VillageName);

            // Primary source: users in Mentor role.
            // Fallback: users with a VillageId assignment (covers legacy/misconfigured role assignment cases).
            var mentorUserIds = new HashSet<int>(mentorRoleUsers.Select(ur => ur.UserId));
            foreach (var userId in villageIds.Keys)
            {
                mentorUserIds.Add(userId);
            }

            var users = _userRoles.GetUserRoles(_alias.SiteId)
                .Where(ur => ur.User != null && mentorUserIds.Contains(ur.UserId))
                .GroupBy(ur => ur.UserId)
                .Select(g => g.First().User)
                .ToList();

            var result = users
                .Select(u => BuildViewModel(u, villageIds, growerCounts, villages))
                .ToList();

            return Task.FromResult(result);
        }

        public Task<MentorViewModel> GetMentorAsync(string username, int moduleId)
        {
            var user = _userManager.GetUser(username, _alias.SiteId);
            if (user == null) return Task.FromResult<MentorViewModel>(null);

            var villageIds = _mentorRepository.GetAllVillageIds();
            var growerCounts = _mentorRepository.GetGrowerCountsByMentor();
            var villages = _villageRepository.GetVillages().ToDictionary(v => v.VillageId, v => v.VillageName);

            return Task.FromResult(BuildViewModel(user, villageIds, growerCounts, villages));
        }

        public async Task<MentorViewModel> CreateMentorAsync(MentorViewModel model, int moduleId)
        {
            // Normalize and validate required fields before creating the Oqtane user
            var username = model.Username?.Trim();
            if (string.IsNullOrWhiteSpace(username))
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Create, "Failed to create Oqtane user for mentor: Username is required");
                return null;
            }

            var displayName = !string.IsNullOrWhiteSpace(model.DisplayName)
                ? model.DisplayName.Trim()
                : username;

            var email = string.IsNullOrWhiteSpace(model.Email)
                ? null
                : model.Email.Trim();

            // Create the Oqtane user
            var user = new User
            {
                SiteId = _alias.SiteId,
                Username = username,
                DisplayName = displayName,
                Email = email,
                Password = model.Password,
                EmailConfirmed = model.EmailConfirmed,
                IsAuthenticated = true  // admin path — skips password requirement
            };

            user = await _userManager.AddUser(user);
            if (user == null)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Create, "Failed to create Oqtane user for mentor {Username}", model.Username);
                return null;
            }

            // Assign the Mentor role
            var role = _roles.GetRoles(_alias.SiteId).FirstOrDefault(r => r.Name == AppRoleNames.Mentor);
            if (role != null)
            {
                _userRoles.AddUserRole(new UserRole { UserId = user.UserId, RoleId = role.RoleId });
            }
            else
            {
                _logger.Log(LogLevel.Warning, this, LogFunction.Create, "Mentor role not found in site {SiteId} — user created without role assignment", _alias.SiteId);
            }

            // Save village assignment
            if (model.VillageId > 0)
            {
                _mentorRepository.SetVillageId(user.UserId, model.VillageId);
            }

            _logger.Log(LogLevel.Information, this, LogFunction.Create, "Mentor Created {Username}", user.Username);
            return await GetMentorAsync(user.Username, moduleId);
        }

        public Task<MentorViewModel> UpdateMentorProfileAsync(MentorViewModel model, int moduleId)
        {
            _mentorRepository.SetVillageId(model.UserId, model.VillageId);

            _logger.Log(LogLevel.Information, this, LogFunction.Update, "Mentor Profile Updated {Username}", model.Username);
            return GetMentorAsync(model.Username, moduleId);
        }

        public async Task SetMentorActiveAsync(string username, bool isActive, int moduleId)
        {
            var user = _userManager.GetUser(username, _alias.SiteId);
            if (user == null)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Update, "Mentor not found for activate/deactivate {Username}", username);
                return;
            }

            user.IsDeleted = !isActive;
            await _userManager.UpdateUser(user);

            _logger.Log(LogLevel.Information, this, LogFunction.Update, "Mentor {Action} {Username}", isActive ? "Activated" : "Deactivated", username);
        }

        public Task ReassignGrowerAsync(int growerId, string newMentorUsername, int moduleId)
        {
            _mentorRepository.ReassignGrower(growerId, newMentorUsername);
            _logger.Log(LogLevel.Information, this, LogFunction.Update, "Grower {GrowerId} reassigned to mentor {MentorUsername}", growerId, newMentorUsername);
            return Task.CompletedTask;
        }

        // --- helpers ---

        private MentorViewModel BuildViewModel(
            User user,
            Dictionary<int, int> villageIds,
            Dictionary<string, int> growerCounts,
            Dictionary<int, string> villages)
        {
            villageIds.TryGetValue(user.UserId, out var villageId);
            villages.TryGetValue(villageId, out var villageName);
            growerCounts.TryGetValue(user.Username, out var count);

            return new MentorViewModel
            {
                UserId = user.UserId,
                Username = user.Username,
                DisplayName = user.DisplayName,
                Email = user.Email,
                IsDeleted = user.IsDeleted,
                VillageId = villageId,
                VillageName = villageName,
                GrowerCount = count
            };
        }
    }
}
