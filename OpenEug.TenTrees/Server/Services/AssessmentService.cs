using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Oqtane.Models;
using Oqtane.Security;
using Oqtane.Shared;
using OpenEug.TenTrees.Module.Assessment.Repository;
using OpenEug.TenTrees.Models;
using OpenEug.TenTrees.Module.Grower.Repository;

namespace OpenEug.TenTrees.Module.Assessment.Services
{
    public class ServerAssessmentService : IAssessmentService
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IGrowerRepository _growerRepository;
        private readonly IUserPermissions _userPermissions;
        private readonly ILogManager _logger;
        private readonly IHttpContextAccessor _accessor;
        private readonly Alias _alias;

        public ServerAssessmentService(
            IAssessmentRepository assessmentRepository,
            IGrowerRepository growerRepository,
            IUserPermissions userPermissions,
            ITenantManager tenantManager,
            ILogManager logger,
            IHttpContextAccessor accessor)
        {
            _assessmentRepository = assessmentRepository;
            _growerRepository = growerRepository;
            _userPermissions = userPermissions;
            _logger = logger;
            _accessor = accessor;
            _alias = tenantManager.GetAlias();
        }

        public Task<Models.Assessment> GetAssessmentAsync(int assessmentId, int moduleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.View))
            {
                return Task.FromResult(_assessmentRepository.GetAssessment(assessmentId));
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Assessment Get Attempt {AssessmentId} {ModuleId}", assessmentId, moduleId);
                return null;
            }
        }

        public Task<List<Models.Assessment>> GetAssessmentsAsync(int moduleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.View))
            {
                return Task.FromResult(_assessmentRepository.GetAssessments(moduleId).ToList());
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Assessments Get Attempt {ModuleId}", moduleId);
                return null;
            }
        }

        public Task<List<Models.Assessment>> GetAssessmentsByGrowerAsync(int growerId, int moduleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.View))
            {
                return Task.FromResult(_assessmentRepository.GetAssessmentsByGrower(growerId, moduleId).ToList());
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Assessments By Grower Get Attempt {GrowerId} {ModuleId}", growerId, moduleId);
                return null;
            }
        }

        public Task<Models.Assessment> AddAssessmentAsync(Models.Assessment assessment)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, assessment.ModuleId, PermissionNames.Edit))
            {
                var grower = _growerRepository.GetGrower(assessment.GrowerId);
                if (grower == null)
                {
                    _logger.Log(LogLevel.Error, this, LogFunction.Create, "Grower not found {GrowerId}", assessment.GrowerId);
                    return Task.FromResult<Models.Assessment>(null);
                }

                bool isAdmin = _accessor.HttpContext.User.IsInRole(RoleNames.Admin);
                if (!isAdmin && grower.MentorId != _accessor.HttpContext.User.Identity.Name)
                {
                    _logger.Log(LogLevel.Error, this, LogFunction.Security, "User {User} is not assigned to grower {GrowerId}", _accessor.HttpContext.User.Identity.Name, assessment.GrowerId);
                    return Task.FromResult<Models.Assessment>(null);
                }

                // Calculate Permaculture Principles Count
                assessment.PermaculturePrinciplesCount = 0;
                if (assessment.TreesLookHealthy) assessment.PermaculturePrinciplesCount++;
                if (!assessment.HasChemicalFertilizers) assessment.PermaculturePrinciplesCount++;
                if (!assessment.HasPesticides) assessment.PermaculturePrinciplesCount++;
                if (assessment.IsMulched) assessment.PermaculturePrinciplesCount++;
                if (assessment.IsMakingCompost) assessment.PermaculturePrinciplesCount++;
                if (assessment.IsCollectingWater) assessment.PermaculturePrinciplesCount++;
                if (!assessment.HasLeakyTaps) assessment.PermaculturePrinciplesCount++;
                if (assessment.IsGardenDesignedToCaptureWater) assessment.PermaculturePrinciplesCount++;
                if (assessment.IsUsingGreywater) assessment.PermaculturePrinciplesCount++;

                assessment = _assessmentRepository.AddAssessment(assessment);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "Assessment Added {Assessment}", assessment);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Assessment Add Attempt {Assessment}", assessment);
                assessment = null;
            }
            return Task.FromResult(assessment);
        }

        public Task<Models.Assessment> UpdateAssessmentAsync(Models.Assessment assessment)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, assessment.ModuleId, PermissionNames.Edit))
            {
                // Calculate Permaculture Principles Count
                assessment.PermaculturePrinciplesCount = 0;
                if (assessment.TreesLookHealthy) assessment.PermaculturePrinciplesCount++;
                if (!assessment.HasChemicalFertilizers) assessment.PermaculturePrinciplesCount++;
                if (!assessment.HasPesticides) assessment.PermaculturePrinciplesCount++;
                if (assessment.IsMulched) assessment.PermaculturePrinciplesCount++;
                if (assessment.IsMakingCompost) assessment.PermaculturePrinciplesCount++;
                if (assessment.IsCollectingWater) assessment.PermaculturePrinciplesCount++;
                if (!assessment.HasLeakyTaps) assessment.PermaculturePrinciplesCount++;
                if (assessment.IsGardenDesignedToCaptureWater) assessment.PermaculturePrinciplesCount++;
                if (assessment.IsUsingGreywater) assessment.PermaculturePrinciplesCount++;

                assessment = _assessmentRepository.UpdateAssessment(assessment);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "Assessment Updated {Assessment}", assessment);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Assessment Update Attempt {Assessment}", assessment);
                assessment = null;
            }
            return Task.FromResult(assessment);
        }

        public Task DeleteAssessmentAsync(int assessmentId, int moduleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.Edit))
            {
                _assessmentRepository.DeleteAssessment(assessmentId);
                _logger.Log(LogLevel.Information, this, LogFunction.Delete, "Assessment Deleted {AssessmentId}", assessmentId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Assessment Delete Attempt {AssessmentId} {ModuleId}", assessmentId, moduleId);
            }
            return Task.CompletedTask;
        }

        public Task<bool> CanSubmitAssessmentAsync(int growerId, int moduleId)
        {
            if (!_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, moduleId, PermissionNames.Edit))
            {
                return Task.FromResult(false);
            }

            var grower = _growerRepository.GetGrower(growerId);
            if (grower == null || grower.Status != GrowerStatus.Active)
            {
                return Task.FromResult(false);
            }

            bool isAdmin = _accessor.HttpContext.User.IsInRole(RoleNames.Admin);
            if (!isAdmin && grower.MentorId != _accessor.HttpContext.User.Identity.Name)
            {
                return Task.FromResult(false);
            }

            var assessments = _assessmentRepository.GetAssessmentsByGrower(growerId, moduleId).OrderByDescending(a => a.AssessmentDate).ToList();
            if (!assessments.Any())
            {
                return Task.FromResult(true);
            }

            var lastAssessment = assessments.First();
            var daysSinceLast = (DateTime.UtcNow - lastAssessment.AssessmentDate).TotalDays;

            // Determine program year based on grower creation date
            var programYear = (DateTime.UtcNow - grower.CreatedOn).TotalDays <= 365 ? 1 : 2;

            if (programYear == 1)
            {
                return Task.FromResult(daysSinceLast >= 14);
            }
            else
            {
                return Task.FromResult(daysSinceLast >= 30);
            }
        }
    }
}
