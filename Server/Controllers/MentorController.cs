using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oqtane.Controllers;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Oqtane.Shared;
using OpenEug.TenTrees.Models;
using OpenEug.TenTrees.Module.Mentor.Services;
using System;
using System.Collections.Generic;
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
        [Authorize]
        public async Task<ActionResult<List<MentorViewModel>>> Get(int moduleId)
        {
            try
            {
                var result = await _mentorService.GetMentorsAsync(moduleId);
                if (result == null)
                {
                    return StatusCode(StatusCodes.Status403Forbidden);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Mentor Get Failed {ModuleId} {Error}", moduleId, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET api/mentor/{username}?moduleId=x
        [HttpGet("{username}")]
        [Authorize]
        public async Task<ActionResult<MentorViewModel>> Get(string username, int moduleId)
        {
            try
            {
                var result = await _mentorService.GetMentorAsync(username, moduleId);
                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, "Mentor Get Failed {Username} {ModuleId} {Error}", username, moduleId, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST api/mentor?moduleId=x
        [HttpPost]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<ActionResult<MentorViewModel>> Post([FromBody] MentorViewModel model, int moduleId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                model.Password = model.Password ?? string.Empty;
                var result = await _mentorService.CreateMentorAsync(model, moduleId);
                if (result == null)
                {
                    return StatusCode(StatusCodes.Status422UnprocessableEntity);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Create, "Mentor Create Failed {ModuleId} {Username} {Error}", moduleId, model?.Username, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT api/mentor/{username}/profile?moduleId=x
        [HttpPut("{username}/profile")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<ActionResult<MentorViewModel>> PutProfile(string username, [FromBody] MentorViewModel model, int moduleId)
        {
            try
            {
                if (!ModelState.IsValid || model?.Username != username)
                {
                    return BadRequest(ModelState);
                }

                var updated = await _mentorService.UpdateMentorProfileAsync(model, moduleId);
                if (updated == null)
                {
                    return NotFound();
                }

                return Ok(updated);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Update, "Mentor Profile Update Failed {Username} {ModuleId} {Error}", username, moduleId, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT api/mentor/{username}/activate?moduleId=x
        [HttpPut("{username}/activate")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<ActionResult> Activate(string username, int moduleId)
        {
            try
            {
                await _mentorService.SetMentorActiveAsync(username, isActive: true, moduleId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Update, "Mentor Activate Failed {Username} {ModuleId} {Error}", username, moduleId, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT api/mentor/{username}/deactivate?moduleId=x
        [HttpPut("{username}/deactivate")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<ActionResult> Deactivate(string username, int moduleId)
        {
            try
            {
                await _mentorService.SetMentorActiveAsync(username, isActive: false, moduleId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Update, "Mentor Deactivate Failed {Username} {ModuleId} {Error}", username, moduleId, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT api/mentor/grower/{growerId}?newMentorUsername=xxx&moduleId=x
        [HttpPut("grower/{growerId}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<ActionResult> ReassignGrower(int growerId, string newMentorUsername, int moduleId)
        {
            try
            {
                await _mentorService.ReassignGrowerAsync(growerId, newMentorUsername, moduleId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Update, "Mentor ReassignGrower Failed {GrowerId} {NewMentor} {ModuleId} {Error}", growerId, newMentorUsername, moduleId, ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
