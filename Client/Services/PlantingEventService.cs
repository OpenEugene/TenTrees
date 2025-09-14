using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Oqtane.Services;
using OE.TenTrees.Models;

namespace OE.TenTrees.Services
{
    public class PlantingEventService : ServiceBase, IPlantingEventService
    {
        public PlantingEventService(HttpClient http, SiteState siteState) : base(http, siteState) { }

        private string Apiurl => CreateApiUrl("PlantingEvent");

        public async Task<List<PlantingEvent>> GetPlantingEventsAsync(int ModuleId)
        {
            return await GetJsonAsync<List<PlantingEvent>>($"{Apiurl}?moduleid={ModuleId}");
        }

        public async Task<PlantingEvent?> GetPlantingEventAsync(int PlantingEventId, int ModuleId)
        {
            return await GetJsonAsync<PlantingEvent>($"{Apiurl}/{PlantingEventId}?moduleid={ModuleId}");
        }

        public async Task<PlantingEvent> AddPlantingEventAsync(PlantingEvent plantingEvent)
        {
            return await PostJsonAsync<PlantingEvent>(Apiurl, plantingEvent);
        }

        public async Task<PlantingEvent> UpdatePlantingEventAsync(PlantingEvent plantingEvent)
        {
            return await PutJsonAsync<PlantingEvent>($"{Apiurl}/{plantingEvent.PlantingEventId}", plantingEvent);
        }

        public async Task DeletePlantingEventAsync(int PlantingEventId, int ModuleId)
        {
            await DeleteAsync($"{Apiurl}/{PlantingEventId}?moduleid={ModuleId}");
        }
    }
}