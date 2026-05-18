using System.Collections.Generic;
using System.Threading.Tasks;
using OpenEug.TenTrees.Models;

namespace OpenEug.TenTrees.Module.Grower.Services
{
    public interface IOrchardService
    {
        Task<List<OrchardWithTreesDto>> GetOrchardsForGrowerAsync(int growerId);
        Task<TreeDetailDto> AddTreeToGrowerAsync(AddTreeRequest request, int moduleId);
        Task DeleteTreeAsync(int treeId, int moduleId);
    }
}
