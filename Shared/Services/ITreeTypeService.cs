using System.Collections.Generic;
using System.Threading.Tasks;
using OpenEug.TenTrees.Models;

namespace OpenEug.TenTrees.Module.TreeType.Services
{
    public interface ITreeTypeService
    {
        Task<List<Models.TreeType>> GetTreeTypesAsync();
        Task<Models.TreeType> GetTreeTypeAsync(int treeTypeId);
        Task<Models.TreeType> AddTreeTypeAsync(Models.TreeType treeType);
        Task<Models.TreeType> UpdateTreeTypeAsync(Models.TreeType treeType);
        Task DeleteTreeTypeAsync(int treeTypeId);
        Task<List<Models.TreeType>> GetActiveTreeTypesAsync();
    }
}
