using System.Collections.Generic;
using System.Threading.Tasks;
using OE.TenTrees.Models;

namespace OE.TenTrees.Repository
{
    public interface ITreeRepository
    {
        Task<IEnumerable<Tree>> GetTreesAsync(int ModuleId);
        Task<Tree?> GetTreeAsync(int TreeId, int ModuleId);
        Task<Tree> AddTreeAsync(Tree tree);
        Task<Tree> UpdateTreeAsync(Tree tree);
        Task DeleteTreeAsync(int TreeId, int ModuleId);
    }
}