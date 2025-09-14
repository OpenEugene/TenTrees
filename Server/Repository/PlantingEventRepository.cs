using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Oqtane.Modules;
using OE.TenTrees.Models;

namespace OE.TenTrees.Repository
{
    public class PlantingEventRepository : IPlantingEventRepository, ITransientService
    {
        private readonly IDbContextFactory<TenTreesContext> _factory;

        public PlantingEventRepository(IDbContextFactory<TenTreesContext> factory)
        {
            _factory = factory;
        }

        public async Task<IEnumerable<PlantingEvent>> GetPlantingEventsAsync(int ModuleId)
        {
            using var db = _factory.CreateDbContext();
            return await db.PlantingEvent.Where(item => item.ModuleId == ModuleId).ToListAsync();
        }

        public async Task<PlantingEvent?> GetPlantingEventAsync(int PlantingEventId, int ModuleId)
        {
            using var db = _factory.CreateDbContext();
            return await db.PlantingEvent.Where(item => item.PlantingEventId == PlantingEventId && item.ModuleId == ModuleId).FirstOrDefaultAsync();
        }

        public async Task<PlantingEvent> AddPlantingEventAsync(PlantingEvent plantingEvent)
        {
            using var db = _factory.CreateDbContext();
            db.PlantingEvent.Add(plantingEvent);
            await db.SaveChangesAsync();
            return plantingEvent;
        }

        public async Task<PlantingEvent> UpdatePlantingEventAsync(PlantingEvent plantingEvent)
        {
            using var db = _factory.CreateDbContext();
            db.Entry(plantingEvent).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return plantingEvent;
        }

        public async Task DeletePlantingEventAsync(int PlantingEventId, int ModuleId)
        {
            using var db = _factory.CreateDbContext();
            var plantingEvent = await db.PlantingEvent.Where(item => item.PlantingEventId == PlantingEventId && item.ModuleId == ModuleId).FirstOrDefaultAsync();
            if (plantingEvent != null)
            {
                db.PlantingEvent.Remove(plantingEvent);
                await db.SaveChangesAsync();
            }
        }
    }
}