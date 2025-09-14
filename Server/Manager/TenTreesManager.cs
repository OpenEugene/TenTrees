using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Oqtane.Modules;
using Oqtane.Models;
using Oqtane.Infrastructure;
using Oqtane.Interfaces;
using OE.TenTrees.Repository;

namespace OE.TenTrees.Manager
{
    public class TenTreesManager : MigratableModuleBase, IInstallable, IPortable, ISearchable
    {
        private readonly ITenTreesRepository _TenTreesRepository;

        public TenTreesManager(ITenTreesRepository TenTreesRepository)
        {
            _TenTreesRepository = TenTreesRepository;
        }

        public bool Install(Tenant tenant, string version)
        {
            return Migrate(tenant, MigrationType.Up, version);
        }

        public bool Uninstall(Tenant tenant)
        {
            return Migrate(tenant, MigrationType.Down, "1.0.0");
        }

        public string ExportModule(Oqtane.Models.Module module)
        {
            string content = "";
            var trees = _TenTreesRepository.GetTrees(module.ModuleId);
            if (trees != null)
            {
                content = JsonSerializer.Serialize(trees);
            }
            return content;
        }

        public void ImportModule(Oqtane.Models.Module module, string content, string version)
        {
            var trees = JsonSerializer.Deserialize<List<Models.Tree>>(content);
            if (trees != null)
            {
                foreach(var tree in trees)
                {
                    tree.ModuleId = module.ModuleId;
                    _TenTreesRepository.AddTree(tree);
                }
            }
        }

        public List<SearchContent> GetSearchContents(Oqtane.Models.Module module, DateTime lastIndexedOn)
        {
            var searchContentList = new List<SearchContent>();

            var trees = _TenTreesRepository.GetTrees(module.ModuleId);
            foreach (var tree in trees)
            {
                if (tree.ModifiedOn >= lastIndexedOn)
                {
                    var searchContent = new SearchContent
                    {
                        EntityName = "OETenTreesTree",
                        EntityId = tree.TreeId.ToString(),
                        Title = $"{tree.Species} - {tree.CommonName}",
                        Body = $"{tree.Location} {tree.Notes}",
                        ContentModifiedBy = tree.ModifiedBy,
                        ContentModifiedOn = tree.ModifiedOn
                    };
                    searchContentList.Add(searchContent);
                }
            }

            return searchContentList;
        }
    }
}