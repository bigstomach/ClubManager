using System;
using System.Collections.Generic;
using ClubManager.helpers;
using ClubManager.Helpers;
using ClubManager.QueryObjects;
using ClubManager.Services;
using ClubManager.ViewObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClubManager.Controllers
{
    [Authorize(Roles = "Manager")]
    [Route("Principal")]
    [ApiController]
    public class ManagerController:ControllerBase
    {
        private readonly IManagerService _managerService;
        
        public ManagerController(IManagerService managerService)
        {
            _managerService = managerService;
        }

        [HttpPost("getActivities")]
        [ProducesResponseType(typeof(PaginatedList<ActivitiesVO>),200)]
        public IActionResult GetActivities([FromBody] PageQO pq)
        {
            var userId = Utils.GetCurrentUserId(this.User);
            var acts = _managerService.GetActs(userId,pq.Query);
            return Ok(PaginatedList<ActivitiesVO>.Create(acts,pq.PageNumber ?? 1,pq.PageSize));
        }

        [HttpPost("addOneActivity")]
        [ProducesResponseType(typeof(ActivitiesVO),200)]
        public IActionResult AddActivities([FromBody] ActQO aq)
        {
            var userId = Utils.GetCurrentUserId(this.User);
            var newAct=_managerService.AddAct(aq,userId);
            return CreatedAtAction(nameof(GetActById), new {id = newAct.ActivityId}, newAct);
        }

        [HttpPost("getOneActivity/{id}")]
        [ProducesResponseType(typeof(ActivitiesVO),200)]
        [ProducesResponseType(404)]
        public IActionResult GetActById(long id)
        {
            var userId = Utils.GetCurrentUserId(this.User);
            var act = _managerService.GetOneAct(userId,id);
            if (act == null)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpPost("updateOneActivity/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(string),404)]
        public IActionResult UpdateAct([FromBody]UpdateActQO aq)
        {
            var userId = Utils.GetCurrentUserId(this.User);
            bool success=_managerService.UpdateAct(aq, userId);
            if (success) return NoContent();
            return NotFound();
        }
        
        [HttpPost("deleteOneActivity/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteSpec(long id)
        {
            var userId = Utils.GetCurrentUserId(this.User);
            var exist = _managerService.DeleteAct(id,userId);
            if (exist) return NoContent();
            return NotFound();
        }

        [HttpPost("getClubName")]
        public IActionResult GetClubName()
        {
            var userId = Utils.GetCurrentUserId(this.User);
            return Ok(_managerService.GetClubName(userId));
        }
        
    }
}