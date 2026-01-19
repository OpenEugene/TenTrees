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
using OpenEug.TenTrees.Module.Village.Repository;
using Models = OpenEug.TenTrees.Models;
using System.Threading.Tasks;

namespace OpenEug.TenTrees.Module.Village.Manager
{
    public class VillageManager : MigratableModuleBase, IInstallable, IPortable, ISearchable
    {
        private readonly IVillageRepository _villageRepository;
        private readonly IDBContextDependencies _dbContextDependencies;

        public VillageManager(IVillageRepository villageRepository, IDBContextDependencies dbContextDependencies)
        {
            _villageRepository = villageRepository;
            _dbContextDependencies = dbContextDependencies;
        }

        public bool Install(Tenant tenant, string version)
        {
            return Migrate(new OpenEug.TenTrees.Repository.TenTreesContext(_dbContextDependencies), tenant, MigrationType.Up);
        }

        public bool Uninstall(Tenant tenant)
        {
            return Migrate(new OpenEug.TenTrees.Repository.TenTreesContext(_dbContextDependencies), tenant, MigrationType.Down);
        }

        public string ExportModule(Oqtane.Models.Module module)
        {
            string content = "";
            List<Models.Village> villages = _villageRepository.GetVillages().ToList();
            if (villages != null)
            {
                content = JsonSerializer.Serialize(villages);
            }
            return content;
        }

        public void ImportModule(Oqtane.Models.Module module, string content, string version)
        {
            List<Models.Village> villages = null;
            if (!string.IsNullOrEmpty(content))
            {
                villages = JsonSerializer.Deserialize<List<Models.Village>>(content);
            }
            if (villages != null)
            {
                foreach(var village in villages)
                {
                    _villageRepository.AddVillage(new Models.Village { 
                        VillageName = village.VillageName,
                        ContactName = village.ContactName,
                        ContactPhone = village.ContactPhone,
                        ContactEmail = village.ContactEmail,
                        Notes = village.Notes,
                        IsActive = village.IsActive
                    });
                }
            }
        }

        public Task<List<SearchContent>> GetSearchContentsAsync(PageModule pageModule, DateTime lastIndexedOn)
        {
           var searchContentList = new List<SearchContent>();

           foreach (var village in _villageRepository.GetVillages())
           {
               if (village.ModifiedOn >= lastIndexedOn)
               {
                   searchContentList.Add(new SearchContent
                   {
                       EntityName = "OpenEug.TenTreesVillage",
                       EntityId = village.VillageId.ToString(),
                       Title = village.VillageName,
                       Body = $"{village.VillageName} - Contact: {village.ContactName}",
                       ContentModifiedBy = village.ModifiedBy,
                       ContentModifiedOn = village.ModifiedOn
                   });
               }
           }

           return Task.FromResult(searchContentList);
        }
    }
}
