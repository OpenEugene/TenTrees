using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Oqtane.Shared;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using OE.TenTrees.Services;
using Oqtane.Controllers;
using System.Net;
using System.Threading.Tasks;
using OE.TenTrees.Models;

namespace OE.TenTrees.Controllers
{
    [Route(ControllerRoutes.ApiRoute)]
    public class GardenController : ModuleControllerBase
    {
        private readonly IGardenService _gardenService;

        public GardenController(IGardenService gardenService, ILogManager logger, IHttpContextAccessor accessor) : base(logger, accessor)
        {
            _gardenService = gardenService;
        }

        // GET: api/<controller>
        [HttpGet]
        [Authorize(Roles = RoleNames.Registered)]
        public async Task<IEnumerable<GardenSite>> Get()
        {
            return await _gardenService.GetGardensAsync();
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        [Authorize(Roles = RoleNames.Registered)]
        public async Task<GardenSite> Get(int id)
        {
            return await _gardenService.GetGardenAsync(id);
        }

        // GET api/<controller>/details/5
        [HttpGet("details/{id}")]
        [Authorize(Roles = RoleNames.Registered)]
        public async Task<GardenDetailVm> GetDetails(int id)
        {
            return await _gardenService.GetGardenDetailAsync(id);
        }

        // GET api/<controller>/by-application/5
        [HttpGet("by-application/{applicationId}")]
        [Authorize(Roles = RoleNames.Registered)]
        public async Task<GardenSite> GetByApplicationId(int applicationId)
        {
            return await _gardenService.GetGardenByApplicationIdAsync(applicationId);
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize(Roles = RoleNames.Registered)]
        public async Task<GardenSite> Post([FromBody] GardenSite gardenSite)
        {
            if (ModelState.IsValid)
            {
                return await _gardenService.AddGardenAsync(gardenSite);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Invalid Garden Post Attempt {Garden}", gardenSite);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return null;
            }
        }

        // POST api/<controller>/from-application
        [HttpPost("from-application")]
        [Authorize(Roles = RoleNames.Registered)]
        public async Task<GardenSite> CreateFromApplication([FromBody] CreateGardenFromApplicationRequest request)
        {
            if (ModelState.IsValid)
            {
                return await _gardenService.CreateGardenFromApplicationAsync(request);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Invalid Create Garden From Application Attempt {Request}", request);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return null;
            }
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize(Roles = RoleNames.Registered)]
        public async Task<GardenSite> Put(int id, [FromBody] GardenSite gardenSite)
        {
            if (ModelState.IsValid && gardenSite.GardenSiteId == id)
            {
                return await _gardenService.UpdateGardenAsync(gardenSite);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Invalid Garden Put Attempt {Garden}", gardenSite);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return null;
            }
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = RoleNames.Registered)]
        public async Task Delete(int id)
        {
            await _gardenService.DeleteGardenAsync(id);
        }

        // GET api/<controller>/listitems
        [HttpGet("listitems")]
        [Authorize(Roles = RoleNames.Registered)]
        public async Task<IEnumerable<GardenListItemVm>> GetListItems()
        {
            return await _gardenService.GetGardenListItemsAsync();
        }

        // GET api/<controller>/statistics/5
        [HttpGet("statistics/{id}")]
        [Authorize(Roles = RoleNames.Registered)]
        public async Task<GardenStatistics> GetStatistics(int id)
        {
            return await _gardenService.GetGardenStatisticsAsync(id);
        }

        // Tree Planting endpoints
        // GET api/<controller>/5/plantings
        [HttpGet("{gardenId}/plantings")]
        [Authorize(Roles = RoleNames.Registered)]
        public async Task<IEnumerable<TreePlanting>> GetTreePlantings(int gardenId)
        {
            return await _gardenService.GetTreePlantingsAsync(gardenId);
        }

        // POST api/<controller>/plantings
        [HttpPost("plantings")]
        [Authorize(Roles = RoleNames.Registered)]
        public async Task<TreePlanting> PostTreePlanting([FromBody] TreePlanting treePlanting)
        {
            if (ModelState.IsValid)
            {
                return await _gardenService.AddTreePlantingAsync(treePlanting);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Invalid Tree Planting Post Attempt {TreePlanting}", treePlanting);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return null;
            }
        }

        // PUT api/<controller>/plantings/5
        [HttpPut("plantings/{id}")]
        [Authorize(Roles = RoleNames.Registered)]
        public async Task<TreePlanting> PutTreePlanting(int id, [FromBody] TreePlanting treePlanting)
        {
            if (ModelState.IsValid && treePlanting.TreePlantingId == id)
            {
                return await _gardenService.UpdateTreePlantingAsync(treePlanting);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Invalid Tree Planting Put Attempt {TreePlanting}", treePlanting);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return null;
            }
        }

        // DELETE api/<controller>/plantings/5
        [HttpDelete("plantings/{id}")]
        [Authorize(Roles = RoleNames.Registered)]
        public async Task DeleteTreePlanting(int id)
        {
            await _gardenService.DeleteTreePlantingAsync(id);
        }

        // Photo endpoints
        // GET api/<controller>/5/photos
        [HttpGet("{gardenId}/photos")]
        [Authorize(Roles = RoleNames.Registered)]
        public async Task<IEnumerable<GardenPhoto>> GetPhotos(int gardenId)
        {
            return await _gardenService.GetGardenPhotosAsync(gardenId);
        }

        // POST api/<controller>/photos
        [HttpPost("photos")]
        [Authorize(Roles = RoleNames.Registered)]
        public async Task<GardenPhoto> PostPhoto([FromBody] GardenPhoto photo)
        {
            if (ModelState.IsValid)
            {
                return await _gardenService.AddGardenPhotoAsync(photo);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Invalid Garden Photo Post Attempt {Photo}", photo);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return null;
            }
        }

        // DELETE api/<controller>/photos/5
        [HttpDelete("photos/{id}")]
        [Authorize(Roles = RoleNames.Registered)]
        public async Task DeletePhoto(int id)
        {
            await _gardenService.DeleteGardenPhotoAsync(id);
        }
    }
}