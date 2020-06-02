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
        
        //获取活动列表并分页
        [HttpPost("getActivities")]
        [ProducesResponseType(typeof(PaginatedList<ActivitiesVO>),200)]
        [ProducesResponseType(404)]
        public IActionResult GetActivities([FromBody] PageQO pq)
        {
            var userId = Utils.GetCurrentUserId(this.User);
            var acts = _managerService.GetActs(userId,pq.Query);
            if (acts == null) return NotFound();
            return Ok(PaginatedList<ActivitiesVO>.Create(acts,pq.PageNumber ?? 1,pq.PageSize));
        }

        //增加一条活动记录
        [HttpPost("addOneActivity")]
        [ProducesResponseType(200)]
        public IActionResult AddActivities([FromBody] ActQO aq)
        {
            var userId = Utils.GetCurrentUserId(this.User);
            var newAct=_managerService.AddAct(aq,userId);
            return Ok();
        }

        //通过id获取一条活动记录
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
            return Ok(act);
        }
        
        //根据id更新一条活动记录
        [HttpPost("updateOneActivity/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult UpdateAct([FromBody]UpdateActQO aq)
        {
            var userId = Utils.GetCurrentUserId(this.User);
            bool success=_managerService.UpdateAct(aq, userId);
            if (success) return Ok();
            return NotFound();
        }
        
        //根据id删除一条活动记录
        [HttpPost("deleteOneActivity/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult DeleteSpec(long id)
        {
            var userId = Utils.GetCurrentUserId(this.User);
            var exist = _managerService.DeleteAct(id,userId);
            if (exist) return Ok();
            return NotFound();
        }

        //根据用户id获取社团名称
        [HttpPost("getClubName")]
        [ProducesResponseType(typeof(NameVO),200)]
        public IActionResult GetClubName()
        {
            var userId = Utils.GetCurrentUserId(this.User);
            return Ok(new NameVO{Name = _managerService.GetClubName(userId)});
        }
        
    }
}