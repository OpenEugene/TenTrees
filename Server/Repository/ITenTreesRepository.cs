using System.Collections.Generic;
using OE.TenTrees.Models;

namespace OE.TenTrees.Repository
{
    public interface ITenTreesRepository
    {
        IEnumerable<Tree> GetTrees(int ModuleId);
        Tree AddTree(Tree tree);
    }
}