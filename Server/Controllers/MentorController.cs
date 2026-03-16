using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oqtane.Controllers;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Oqtane.Shared;
using OpenEug.TenTrees.Models;
using OpenEug.TenTrees.Module.Mentor.Services;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace OpenEug.TenTrees.Module.Mentor.Controllers
{
    [Route(ControllerRoutes.ApiRoute)]
    public class MentorController : ModuleControllerBase
    {
        private readonly IMentorService _mentorService;

        public MentorController(IMentorService mentorService, ILogManager logger, IHttpContextAccessor accessor)
            : base(logger, accessor)
        {
            _mentorService = mentorService;
        }

        // GET api/mentor?moduleId=x
        [HttpGet]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<List<MentorViewModel>> Get(int moduleId)
        {
            var result = await _mentorService.GetMentorsAsync(moduleId);
            if (result == null)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
            return result;
        }

        // GET api/mentor/{username}?moduleId=x
        [HttpGet("{username}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<MentorViewModel> Get(string username, int moduleId)
        {
            var result = await _mentorService.GetMentorAsync(username, moduleId);
            if (result == null)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return null;
            }
            return result;
        }

        // POST api/mentor?moduleId=x
        [HttpPost]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<MentorViewModel> Post([FromBody] MentorViewModel model, int moduleId)
        {
            if (!ModelState.IsValid)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return null;
            }

            model.Password = model.Password ?? string.Empty;
            var result = await _mentorService.CreateMentorAsync(model, moduleId);
            if (result == null)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                return null;
            }
            return result;
        }

        // PUT api/mentor/{username}/profile?moduleId=x
        [HttpPut("{username}/profile")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<MentorViewModel> PutProfile(string username, [FromBody] MentorViewModel model, int moduleId)
        {
            if (!ModelState.IsValid || model.Username != username)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return null;
            }

            return await _mentorService.UpdateMentorProfileAsync(model, moduleId);
        }

        // PUT api/mentor/{username}/activate?moduleId=x
        [HttpPut("{username}/activate")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task Activate(string username, int moduleId)
        {
            await _mentorService.SetMentorActiveAsync(username, isActive: true, moduleId);
        }

        // PUT api/mentor/{username}/deactivate?moduleId=x
        [HttpPut("{username}/deactivate")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task Deactivate(string username, int moduleId)
        {
            await _mentorService.SetMentorActiveAsync(username, isActive: false, moduleId);
        }

        // PUT api/mentor/grower/{growerId}?newMentorUsername=xxx&moduleId=x
        [HttpPut("grower/{growerId}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task ReassignGrower(int growerId, string newMentorUsername, int moduleId)
        {
            await _mentorService.ReassignGrowerAsync(growerId, newMentorUsername, moduleId);
        }
    }
}
