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
    public class MonitoringManager : MigratableModuleBase, IInstallable, IPortable, ISearchable
    {
        private readonly IMonitoringRepository _monitoringRepository;
        private readonly IDBContextDependencies _DBContextDependencies;

        public MonitoringManager(IMonitoringRepository monitoringRepository, IDBContextDependencies DBContextDependencies)
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
            List<Models.MonitoringSession> sessions = _monitoringRepository.GetMonitoringSessions().ToList();
            if (sessions != null)
            {
                content = JsonSerializer.Serialize(sessions);
            }
            return content;
        }

        public void ImportModule(Module module, string content, string version)
        {
            List<Models.MonitoringSession> sessions = null;
            if (!string.IsNullOrEmpty(content))
            {
                sessions = JsonSerializer.Deserialize<List<Models.MonitoringSession>>(content);
            }
            if (sessions != null)
            {
                foreach (var session in sessions)
                {
                    session.ModuleId = module.ModuleId;
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
                        Title = $"Monitoring Session - {session.BeneficiaryName}",
                        Body = $"Evaluator: {session.EvaluatorName}, Date: {session.SessionDate:MM/dd/yyyy}",
                        ContentModifiedBy = session.ModifiedBy,
                        ContentModifiedOn = session.ModifiedOn,
                        AdditionalContent = session.Notes
                    };
                    searchContentList.Add(searchContent);
                }
            }

            return Task.FromResult(searchContentList);
        }
    }
}