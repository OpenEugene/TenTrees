using System.Collections.Generic;
using System.Threading.Tasks;
using OE.TenTrees.Models;

namespace OE.TenTrees.Services
{
    public interface ITreeService
    {
        Task<List<Tree>> GetTreesAsync(int ModuleId);
        Task<Tree?> GetTreeAsync(int TreeId, int ModuleId);
        Task<Tree> AddTreeAsync(Tree tree);
        Task<Tree> UpdateTreeAsync(Tree tree);
        Task DeleteTreeAsync(int TreeId, int ModuleId);
    }
}