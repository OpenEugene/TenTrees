using System;
using System.Collections.Generic;
using System.Linq;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Oqtane.Models;
using Oqtane.Repository;
using Oqtane.Shared;
using OpenEug.TenTrees.Models;
using OpenEug.TenTrees.Shared;

namespace OpenEug.TenTrees.Module.Assessment.Services
{
    /// <summary>
    /// Provisions the private Oqtane folders that hold assessment photos and keeps
    /// their permissions aligned with the grower's assigned mentor.
    ///
    /// Layout (all folders are private, site-level, IsSystem):
    ///   AssessmentPhotos/              staff only (Browse/View/Edit for admins, Browse/View for reviewers)
    ///   AssessmentPhotos/{GrowerId}/   staff as above, plus the assigned mentor user (Browse/View/Edit)
    /// </summary>
    public interface IAssessmentPhotoFolderService
    {
        /// <summary>
        /// Returns the grower's photo folder for the active site, creating the root and grower
        /// folders if needed and re-syncing the grower folder's permissions to the current mentor.
        /// Returns null when the site cannot be resolved.
        /// </summary>
        Folder GetOrCreateGrowerFolder(Models.Grower grower);

        /// <summary>
        /// Re-syncs an existing grower folder's permissions to the grower's current mentor.
        /// No-op when the grower has no folder yet.
        /// </summary>
        void SyncGrowerFolderPermissions(Models.Grower grower);
    }

    public class AssessmentPhotoFolderService : IAssessmentPhotoFolderService
    {
        private readonly IFolderRepository _folderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ITenantManager _tenantManager;
        private readonly ILogManager _logger;

        public AssessmentPhotoFolderService(
            IFolderRepository folderRepository,
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            ITenantManager tenantManager,
            ILogManager logger)
        {
            _folderRepository = folderRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _tenantManager = tenantManager;
            _logger = logger;
        }

        public Folder GetOrCreateGrowerFolder(Models.Grower grower)
        {
            if (grower == null)
            {
                return null;
            }

            var siteId = _tenantManager.GetAlias()?.SiteId ?? 0;
            if (siteId <= 0)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Assessment photo folder requested without an active site {GrowerId}", grower.GrowerId);
                return null;
            }

            var root = GetOrCreateRootFolder(siteId);
            if (root == null)
            {
                return null;
            }

            var growerPath = AssessmentPhotoRules.GrowerFolderPath(grower.GrowerId);
            var growerFolder = _folderRepository.GetFolder(siteId, growerPath);
            var expected = GrowerFolderPermissions(siteId, grower);

            if (growerFolder == null)
            {
                growerFolder = _folderRepository.AddFolder(new Folder
                {
                    SiteId = siteId,
                    ParentId = root.FolderId,
                    Name = grower.GrowerId.ToString(),
                    Type = FolderTypes.Private,
                    Path = growerPath,
                    Order = 1,
                    ImageSizes = string.Empty,
                    Capacity = 0,
                    CacheControl = "no-store",
                    IsSystem = true,
                    PermissionList = expected
                });
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "Assessment photo folder created {Path} {FolderId}", growerPath, growerFolder.FolderId);
                return growerFolder;
            }

            SyncPermissions(growerFolder, expected);
            return growerFolder;
        }

        public void SyncGrowerFolderPermissions(Models.Grower grower)
        {
            if (grower == null)
            {
                return;
            }

            var siteId = _tenantManager.GetAlias()?.SiteId ?? 0;
            if (siteId <= 0)
            {
                return;
            }

            var growerFolder = _folderRepository.GetFolder(siteId, AssessmentPhotoRules.GrowerFolderPath(grower.GrowerId));
            if (growerFolder == null)
            {
                return;
            }

            SyncPermissions(growerFolder, GrowerFolderPermissions(siteId, grower));
        }

        private Folder GetOrCreateRootFolder(int siteId)
        {
            var root = _folderRepository.GetFolder(siteId, AssessmentPhotoRules.FolderPath);
            if (root != null)
            {
                return root;
            }

            var siteRoot = _folderRepository.GetFolder(siteId, string.Empty);
            if (siteRoot == null)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Create, "Site root folder not found for site {SiteId}", siteId);
                return null;
            }

            root = _folderRepository.AddFolder(new Folder
            {
                SiteId = siteId,
                ParentId = siteRoot.FolderId,
                Name = "AssessmentPhotos",
                Type = FolderTypes.Private,
                Path = AssessmentPhotoRules.FolderPath,
                Order = 1,
                ImageSizes = string.Empty,
                Capacity = 0,
                CacheControl = "no-store",
                IsSystem = true,
                PermissionList = StaffPermissions(siteId)
            });
            _logger.Log(LogLevel.Information, this, LogFunction.Create, "Assessment photo root folder created {FolderId}", root.FolderId);
            return root;
        }

        private void SyncPermissions(Folder folder, List<Permission> expected)
        {
            var current = folder.PermissionList ?? new List<Permission>();
            if (current.Count == expected.Count && current.All(c => expected.Any(e => SamePermission(c, e))))
            {
                return;
            }

            folder.PermissionList = expected;
            _folderRepository.UpdateFolder(folder);
            _logger.Log(LogLevel.Information, this, LogFunction.Update, "Assessment photo folder permissions synced {Path}", folder.Path);
        }

        private static bool SamePermission(Permission a, Permission b)
        {
            return a.PermissionName == b.PermissionName
                && a.IsAuthorized == b.IsAuthorized
                && a.UserId == b.UserId
                && string.Equals(a.RoleName ?? string.Empty, b.RoleName ?? string.Empty, StringComparison.Ordinal);
        }

        private List<Permission> GrowerFolderPermissions(int siteId, Models.Grower grower)
        {
            var permissions = StaffPermissions(siteId);

            var mentor = string.IsNullOrWhiteSpace(grower.MentorUsername) ? null : _userRepository.GetUser(grower.MentorUsername);
            if (mentor != null)
            {
                permissions.Add(new Permission(PermissionNames.Browse, mentor.UserId, true));
                permissions.Add(new Permission(PermissionNames.View, mentor.UserId, true));
                permissions.Add(new Permission(PermissionNames.Edit, mentor.UserId, true));
            }
            else if (!string.IsNullOrWhiteSpace(grower.MentorUsername))
            {
                _logger.Log(LogLevel.Warning, this, LogFunction.Security, "Mentor {MentorUsername} for grower {GrowerId} is not an Oqtane user; folder grants staff access only", grower.MentorUsername, grower.GrowerId);
            }

            return permissions;
        }

        /// <summary>
        /// Role permissions shared by every assessment photo folder. Roles that do not exist on the
        /// site are skipped so Oqtane never stores a permission row with neither a role nor a user.
        /// </summary>
        private List<Permission> StaffPermissions(int siteId)
        {
            var siteRoles = _roleRepository.GetRoles(siteId, true).Select(r => r.Name).ToHashSet(StringComparer.Ordinal);
            var permissions = new List<Permission>();

            void Grant(string roleName, params string[] permissionNames)
            {
                if (!siteRoles.Contains(roleName))
                {
                    return;
                }

                foreach (var permissionName in permissionNames)
                {
                    permissions.Add(new Permission(permissionName, roleName, true));
                }
            }

            Grant(AppRoleNames.Admin, PermissionNames.Browse, PermissionNames.View, PermissionNames.Edit);
            Grant(AppRoleNames.TenTreesAdmin, PermissionNames.Browse, PermissionNames.View, PermissionNames.Edit);
            Grant(AppRoleNames.Educator, PermissionNames.Browse, PermissionNames.View);
            Grant(AppRoleNames.ProjectManager, PermissionNames.Browse, PermissionNames.View);

            return permissions;
        }
    }
}
