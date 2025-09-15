using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Oqtane.Services;
using Oqtane.Shared;

namespace OE.TenTrees.Services
{
    public interface IMyModuleService
    {
        Task<List<Models.MyModule>> GetMyModulesAsync(int ModuleId);

        Task<Models.MyModule> GetMyModuleAsync(int MyModuleId, int ModuleId);

        Task<Models.MyModule> AddMyModuleAsync(Models.MyModule MyModule);

        Task<Models.MyModule> UpdateMyModuleAsync(Models.MyModule MyModule);

        Task DeleteMyModuleAsync(int MyModuleId, int ModuleId);
    }

    public class MyModuleService : ServiceBase, IMyModuleService
    {
        public MyModuleService(HttpClient http, SiteState siteState) : base(http, siteState) { }

        private string Apiurl => CreateApiUrl("MyModule");

        public async Task<List<Models.MyModule>> GetMyModulesAsync(int ModuleId)
        {
            List<Models.MyModule> Tasks = await GetJsonAsync<List<Models.MyModule>>($"{Apiurl}?moduleid={ModuleId}", Enumerable.Empty<Models.MyModule>().ToList());
            return Tasks.OrderBy(item => item.Name).ToList();
        }

        public async Task<Models.MyModule> GetMyModuleAsync(int MyModuleId, int ModuleId)
        {
            return await GetJsonAsync<Models.MyModule>($"{Apiurl}/{MyModuleId}/{ModuleId}");
        }

        public async Task<Models.MyModule> AddMyModuleAsync(Models.MyModule MyModule)
        {
            return await PostJsonAsync<Models.MyModule>($"{Apiurl}", MyModule);
        }

        public async Task<Models.MyModule> UpdateMyModuleAsync(Models.MyModule MyModule)
        {
            return await PutJsonAsync<Models.MyModule>($"{Apiurl}/{MyModule.MyModuleId}", MyModule);
        }

        public async Task DeleteMyModuleAsync(int MyModuleId, int ModuleId)
        {
            await DeleteAsync($"{Apiurl}/{MyModuleId}/{ModuleId}");
        }
    }
}
