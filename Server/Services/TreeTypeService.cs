using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Models = OpenEug.TenTrees.Models;
using OpenEug.TenTrees.Module.TreeType.Repository;
using Oqtane.Shared;

namespace OpenEug.TenTrees.Module.TreeType.Services
{
    public class ServerTreeTypeService : ITreeTypeService
    {
        private readonly ITreeTypeRepository _treeTypeRepository;
        private readonly ILogManager _logger;

        public ServerTreeTypeService(ITreeTypeRepository treeTypeRepository, ILogManager logger)
        {
            _treeTypeRepository = treeTypeRepository;
            _logger = logger;
        }

        public Task<List<Models.TreeType>> GetTreeTypesAsync()
        {
            return Task.FromResult(_treeTypeRepository.GetTreeTypes().ToList());
        }

        public Task<Models.TreeType> GetTreeTypeAsync(int treeTypeId)
        {
            return Task.FromResult(_treeTypeRepository.GetTreeType(treeTypeId));
        }

        public Task<Models.TreeType> AddTreeTypeAsync(Models.TreeType treeType)
        {
            treeType = _treeTypeRepository.AddTreeType(treeType);
            _logger.Log(LogLevel.Information, this, LogFunction.Create, "TreeType Added {TreeType}", treeType);
            return Task.FromResult(treeType);
        }

        public Task<Models.TreeType> UpdateTreeTypeAsync(Models.TreeType treeType)
        {
            treeType = _treeTypeRepository.UpdateTreeType(treeType);
            _logger.Log(LogLevel.Information, this, LogFunction.Update, "TreeType Updated {TreeType}", treeType);
            return Task.FromResult(treeType);
        }

        public Task DeleteTreeTypeAsync(int treeTypeId)
        {
            _treeTypeRepository.DeleteTreeType(treeTypeId);
            _logger.Log(LogLevel.Information, this, LogFunction.Delete, "TreeType Deleted {TreeTypeId}", treeTypeId);
            return Task.CompletedTask;
        }

        public Task<List<Models.TreeType>> GetActiveTreeTypesAsync()
        {
            return Task.FromResult(_treeTypeRepository.GetTreeTypes()
                .Where(t => t.IsActive)
                .OrderBy(t => t.Category)
                .ThenBy(t => t.SortOrder)
                .ToList());
        }
    }
}
