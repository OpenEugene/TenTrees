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
    public class GardenManager : MigratableModuleBase, IInstallable, IPortable, ISearchable
    {
        private readonly IMonitoringRepository _monitoringRepository;
        private readonly IDBContextDependencies _DBContextDependencies;

        public GardenManager(IMonitoringRepository monitoringRepository, IDBContextDependencies DBContextDependencies)
        {
            _monitoringRepository = monitoringRepository;
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
            List<Models.MonitoringSession> monitoringSessions = _monitoringRepository.GetMonitoringSessions().ToList();
            if (monitoringSessions != null)
            {
                content = JsonSerializer.Serialize(monitoringSessions);
            }
            return content;
        }

        public void ImportModule(Module module, string content, string version)
        {
            List<Models.MonitoringSession> monitoringSessions = null;
            if (!string.IsNullOrEmpty(content))
            {
                monitoringSessions = JsonSerializer.Deserialize<List<Models.MonitoringSession>>(content);
            }
            if (monitoringSessions != null)
            {
                foreach (var session in monitoringSessions)
                {
                    _monitoringRepository.AddMonitoringSession(session);
                }
            }
        }

        public Task<List<SearchContent>> GetSearchContentsAsync(PageModule pageModule, DateTime lastIndexedOn)
        {
            var searchContentList = new List<SearchContent>();

            foreach (var session in _monitoringRepository.GetMonitoringSessions())
            {
                if (session.ModifiedOn >= lastIndexedOn)
                {
                    var searchContent = new SearchContent
                    {
                        EntityName = "MonitoringSession",
                        EntityId = session.MonitoringSessionId.ToString(),
                        Title = $"Garden Monitoring - {session.BeneficiaryName}",
                        Body = $"Evaluator: {session.EvaluatorName}, Date: {session.SessionDate:yyyy-MM-dd}, Trees: {session.TreesAlive}/{session.TreesPlanted}",
                        ContentModifiedBy = session.ModifiedBy,
                        ContentModifiedOn = session.ModifiedOn,
                        AdditionalContent = $"{session.Notes} {session.ProblemsIdentified} {session.InterventionNeeded}".Trim()
                    };
                    searchContentList.Add(searchContent);
                }
            }

            return Task.FromResult(searchContentList);
        }
    }
}