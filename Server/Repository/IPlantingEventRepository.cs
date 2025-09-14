using System.Collections.Generic;
using System.Threading.Tasks;
using OE.TenTrees.Models;

namespace OE.TenTrees.Repository
{
    public interface IPlantingEventRepository
    {
        Task<IEnumerable<PlantingEvent>> GetPlantingEventsAsync(int ModuleId);
        Task<PlantingEvent?> GetPlantingEventAsync(int PlantingEventId, int ModuleId);
        Task<PlantingEvent> AddPlantingEventAsync(PlantingEvent plantingEvent);
        Task<PlantingEvent> UpdatePlantingEventAsync(PlantingEvent plantingEvent);
        Task DeletePlantingEventAsync(int PlantingEventId, int ModuleId);
    }
}