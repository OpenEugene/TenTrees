using System.Collections.Generic;
using System.Threading.Tasks;
using OpenEug.TenTrees.Models;

namespace OpenEug.TenTrees.Module.Grower.Services
{
    public interface IGrowerService
    {
        Task<Models.Grower> GetGrowerAsync(int growerId, int moduleId);
        Task<List<Models.Grower>> GetAllGrowersAsync(int moduleId, int? villageId = null);
        Task<Models.Grower> ToggleActiveStatusAsync(int growerId, int moduleId);
        Task<Models.Grower> RecordProgramExitAsync(int growerId, ProgramExitRequest request, int moduleId);
        Task<List<Models.Grower>> GetActiveGrowersAsync(int moduleId, int? villageId = null);
        Task<List<Models.Grower>> GetGrowersByStatusAsync(GrowerStatus status, int moduleId, int? villageId = null);
        Task<GrowerStatusSummary> GetStatusSummaryAsync(int moduleId, int? villageId = null);
        Task<Models.Grower> UpdateGrowerAsync(Models.Grower grower, int moduleId);
    }
}
