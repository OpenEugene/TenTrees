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
using OpenEug.TenTrees.Module.Enrollment.Repository;
using OpenEug.TenTrees.Models;
using System.Threading.Tasks;

namespace OpenEug.TenTrees.Module.Enrollment.Manager
{
    public class EnrollmentManager : MigratableModuleBase, IInstallable, IPortable, ISearchable
    {
        private readonly IEnrollmentRepository _EnrollmentRepository;
        private readonly IDBContextDependencies _DBContextDependencies;

        public EnrollmentManager(IEnrollmentRepository EnrollmentRepository, IDBContextDependencies DBContextDependencies)
        {
            _EnrollmentRepository = EnrollmentRepository;
            _DBContextDependencies = DBContextDependencies;
        }

        public bool Install(Tenant tenant, string version)
        {
            return Migrate(new OpenEug.TenTrees.Repository.TenTreesContext(_DBContextDependencies), tenant, MigrationType.Up);
        }

        public bool Uninstall(Tenant tenant)
        {
            return Migrate(new OpenEug.TenTrees.Repository.TenTreesContext(_DBContextDependencies), tenant, MigrationType.Down);
        }

        public string ExportModule(Oqtane.Models.Module module)
        {
            string content = "";
            List<Models.Enrollment> Enrollments = _EnrollmentRepository.GetEnrollments(module.ModuleId).ToList();
            if (Enrollments != null)
            {
                content = JsonSerializer.Serialize(Enrollments);
            }
            return content;
        }

        public void ImportModule(Oqtane.Models.Module module, string content, string version)
        {
            List<Models.Enrollment> Enrollments = null;
            if (!string.IsNullOrEmpty(content))
            {
                Enrollments = JsonSerializer.Deserialize<List<Models.Enrollment>>(content);
            }
            if (Enrollments != null)
            {
                foreach(var Enrollment in Enrollments)
                {
                    _EnrollmentRepository.AddEnrollment(new Models.Enrollment { 
                        ModuleId = module.ModuleId, 
                        GrowerName = Enrollment.GrowerName,
                        VillageId = Enrollment.VillageId,
                        HouseholdSize = Enrollment.HouseholdSize,
                        MentorId = Enrollment.MentorId,
                        EnrollmentDate = DateTime.UtcNow
                    });
                }
            }
        }

        public Task<List<SearchContent>> GetSearchContentsAsync(PageModule pageModule, DateTime lastIndexedOn)
        {
           var searchContentList = new List<SearchContent>();

           foreach (var Enrollment in _EnrollmentRepository.GetEnrollments(pageModule.ModuleId))
           {
               if (Enrollment.ModifiedOn >= lastIndexedOn)
               {
                   searchContentList.Add(new SearchContent
                   {
                       EntityName = "OpenEug.TenTreesEnrollment",
                       EntityId = Enrollment.EnrollmentId.ToString(),
                       Title = Enrollment.GrowerName,
                       Body = $"{Enrollment.GrowerName} - Village: {Enrollment.VillageId}",
                       ContentModifiedBy = Enrollment.ModifiedBy,
                       ContentModifiedOn = Enrollment.ModifiedOn
                   });
               }
           }

           return Task.FromResult(searchContentList);
        }
    }
}
