using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Oqtane.Shared;
using Models = OpenEug.TenTrees.Models;
using OpenEug.TenTrees.Module.Grower.Repository;
using OpenEug.TenTrees.Module.TreeType.Repository;

namespace OpenEug.TenTrees.Module.Grower.Services
{
    public class ServerOrchardService : IOrchardService
    {
        private readonly IOrchardRepository _orchardRepository;
        private readonly IGrowerRepository _growerRepository;
        private readonly ITreeTypeRepository _treeTypeRepository;
        private readonly ILogManager _logger;

        public ServerOrchardService(IOrchardRepository orchardRepository, IGrowerRepository growerRepository, ITreeTypeRepository treeTypeRepository, ILogManager logger)
        {
            _orchardRepository = orchardRepository;
            _growerRepository = growerRepository;
            _treeTypeRepository = treeTypeRepository;
            _logger = logger;
        }

        public Task<List<Models.OrchardWithTreesDto>> GetOrchardsForGrowerAsync(int growerId)
        {
            var orchards = _orchardRepository.GetOrchardsByGrower(growerId).ToList();
            if (orchards.Count == 0)
            {
                return Task.FromResult(new List<Models.OrchardWithTreesDto>());
            }

            var treeTypes = _treeTypeRepository.GetTreeTypes().ToDictionary(t => t.TreeTypeId);
            var treesByOrchard = _orchardRepository.GetTreesByOrchardIds(orchards.Select(o => o.OrchardId))
                .GroupBy(t => t.OrchardId)
                .ToDictionary(g => g.Key, g => g.ToList());

            var result = orchards.Select(orchard =>
            {
                var trees = (treesByOrchard.GetValueOrDefault(orchard.OrchardId) ?? new List<Models.Tree>())
                    .Select(t =>
                    {
                        treeTypes.TryGetValue(t.TreeTypeId, out var treeType);
                        return new Models.TreeDetailDto
                        {
                            TreeId = t.TreeId,
                            OrchardId = t.OrchardId,
                            TreeTypeId = t.TreeTypeId,
                            TreeTypeName = treeType?.Name ?? string.Empty,
                            TreeTypeCategory = treeType?.Category ?? string.Empty,
                            Quantity = t.Quantity,
                            Notes = t.Notes
                        };
                    }).ToList();

                return new Models.OrchardWithTreesDto { Orchard = orchard, Trees = trees };
            }).ToList();

            return Task.FromResult(result);
        }

        public Task<Models.TreeDetailDto> AddTreeToGrowerAsync(Models.AddTreeRequest request, int moduleId)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Tree request is required.");
            }

            var grower = _growerRepository.GetGrower(request.GrowerId, false);
            if (grower == null)
            {
                _logger.Log(LogLevel.Warning, this, LogFunction.Create, "AddTree rejected. Grower not found {GrowerId}", request.GrowerId);
                throw new ArgumentException($"Grower does not exist: {request.GrowerId}", nameof(request));
            }

            var orchard = _orchardRepository.GetOrDefaultOrchard(request.GrowerId);
            if (orchard == null)
            {
                orchard = _orchardRepository.AddOrchard(new Models.Orchard
                {
                    GrowerId = request.GrowerId,
                    Name = "Default Orchard"
                });
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "Orchard created for Grower {GrowerId}", request.GrowerId);
            }

            var tree = _orchardRepository.AddTree(new Models.Tree
            {
                OrchardId = orchard.OrchardId,
                TreeTypeId = request.TreeTypeId,
                Quantity = request.Quantity,
                Notes = request.Notes
            });
            _logger.Log(LogLevel.Information, this, LogFunction.Create, "Tree Added {Tree}", tree);

            var treeType = _treeTypeRepository.GetTreeType(tree.TreeTypeId, false);
            return Task.FromResult(new Models.TreeDetailDto
            {
                TreeId = tree.TreeId,
                OrchardId = tree.OrchardId,
                TreeTypeId = tree.TreeTypeId,
                TreeTypeName = treeType?.Name ?? string.Empty,
                TreeTypeCategory = treeType?.Category ?? string.Empty,
                Quantity = tree.Quantity,
                Notes = tree.Notes
            });
        }

        public Task DeleteTreeAsync(int treeId, int moduleId)
        {
            _orchardRepository.DeleteTree(treeId);
            _logger.Log(LogLevel.Information, this, LogFunction.Delete, "Tree Deleted {TreeId}", treeId);
            return Task.CompletedTask;
        }
    }
}
