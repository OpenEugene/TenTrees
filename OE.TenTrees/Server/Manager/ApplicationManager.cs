using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Oqtane.Modules;
using Oqtane.Models;
using Oqtane.Infrastructure;
using Oqtane.Interfaces;
using Oqtane.Enums;
using Oqtane.Repository;
using OE.TenTrees.Repository;

namespace OE.TenTrees.Manager
{
    public class ApplicationManager : MigratableModuleBase, IInstallable, IPortable, ISearchable
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IDBContextDependencies _DBContextDependencies;

        public ApplicationManager(IApplicationRepository applicationRepository, IDBContextDependencies DBContextDependencies)
        {
            _applicationRepository = applicationRepository;
            _DBContextDependencies = DBContextDependencies;
        }

        public bool Install(Tenant tenant, string version)
        {
            return Migrate(new Context(_DBContextDependencies), tenant, MigrationType.Up);
        }

        public bool Uninstall(Tenant tenant)
        {
            return Migrate(new Context(_DBContextDependencies), tenant, MigrationType.Down);
        }

        public string ExportModule(Module module)
        {
            string content = "";
            List<Models.TreePlantingApplication> applications = _applicationRepository.GetApplications(module.ModuleId).ToList();
            if (applications != null)
            {
                content = JsonSerializer.Serialize(applications);
            }
            return content;
        }

        public void ImportModule(Module module, string content, string version)
        {
            List<Models.TreePlantingApplication> applications = null;
            if (!string.IsNullOrEmpty(content))
            {
                applications = JsonSerializer.Deserialize<List<Models.TreePlantingApplication>>(content);
            }
            if (applications != null)
            {
                foreach (var application in applications)
                {
                    application.ModuleId = module.ModuleId;
                    _applicationRepository.AddApplication(application);
                }
            }
        }

        public Task<List<SearchContent>> GetSearchContentsAsync(PageModule pageModule, DateTime lastIndexedOn)
        {
            var searchContentList = new List<SearchContent>();

            foreach (var application in _applicationRepository.GetApplications(pageModule.ModuleId))
            {
                if (application.ModifiedOn >= lastIndexedOn)
                {
                    var searchContent = new SearchContent
                    {
                        EntityName = "TreePlantingApplication",
                        EntityId = application.ApplicationId.ToString(),
                        Title = $"Application for {application.BeneficiaryName}",
                        Body = $"Evaluator: {application.EvaluatorName}, Village: {application.Village}, Status: {application.Status}",
                        ContentModifiedBy = application.ModifiedBy,
                        ContentModifiedOn = application.ModifiedOn,
                        AdditionalContent = application.Notes
                    };
                    searchContentList.Add(searchContent);
                }
            }

            return Task.FromResult(searchContentList);
        }
    }
}