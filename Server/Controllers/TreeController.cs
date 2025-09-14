using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;
using Oqtane.Shared;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using OE.TenTrees.Models;
using OE.TenTrees.Repository;

namespace OE.TenTrees.Controllers
{
    [Route(ControllerRoutes.ApiRoute)]
    public class TreeController : Controller
    {
        private readonly ITreeRepository _TreeRepository;
        private readonly ILogManager _logger;

        public TreeController(ITreeRepository TreeRepository, ILogManager logger)
        {
            _TreeRepository = TreeRepository;
            _logger = logger;
        }

        // GET: api/<controller>?moduleid=x
        [HttpGet]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<IEnumerable<Tree>> Get(string moduleid)
        {
            int ModuleId;
            if (int.TryParse(moduleid, out ModuleId) && IsAuthorized(EntityNames.Module, ModuleId, PermissionNames.View))
            {
                return await _TreeRepository.GetTreesAsync(ModuleId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Tree Get Attempt {ModuleId}", moduleid);
                HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                return new List<Tree>();
            }
        }

        // GET api/<controller>/5?moduleid=x
        [HttpGet("{id}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<Tree?> Get(int id, string moduleid)
        {
            int ModuleId;
            if (int.TryParse(moduleid, out ModuleId) && IsAuthorized(EntityNames.Module, ModuleId, PermissionNames.View))
            {
                return await _TreeRepository.GetTreeAsync(id, ModuleId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Tree Get Attempt {TreeId} {ModuleId}", id, moduleid);
                HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                return null;
            }
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<Tree> Post([FromBody] Tree Tree)
        {
            if (ModelState.IsValid && IsAuthorized(EntityNames.Module, Tree.ModuleId, PermissionNames.Edit))
            {
                Tree = await _TreeRepository.AddTreeAsync(Tree);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "Tree Added {Tree}", Tree);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Tree Post Attempt {Tree}", Tree);
                HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                Tree = null;
            }
            return Tree;
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<Tree> Put(int id, [FromBody] Tree Tree)
        {
            if (ModelState.IsValid && Tree.TreeId == id && IsAuthorized(EntityNames.Module, Tree.ModuleId, PermissionNames.Edit))
            {
                Tree = await _TreeRepository.UpdateTreeAsync(Tree);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "Tree Updated {Tree}", Tree);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Tree Put Attempt {Tree}", Tree);
                HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                Tree = null;
            }
            return Tree;
        }

        // DELETE api/<controller>/5?moduleid=x
        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task Delete(int id, string moduleid)
        {
            int ModuleId;
            if (int.TryParse(moduleid, out ModuleId) && IsAuthorized(EntityNames.Module, ModuleId, PermissionNames.Edit))
            {
                await _TreeRepository.DeleteTreeAsync(id, ModuleId);
                _logger.Log(LogLevel.Information, this, LogFunction.Delete, "Tree Deleted {TreeId}", id);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Tree Delete Attempt {TreeId} {ModuleId}", id, moduleid);
                HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
            }
        }

        private bool IsAuthorized(string entityName, int entityId, string permissionName)
        {
            return User.IsInRole(RoleNames.Admin) || 
                   UserPermissions.IsAuthorized(User, entityName, entityId, permissionName);
        }
    }
}