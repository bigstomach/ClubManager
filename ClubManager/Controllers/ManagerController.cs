using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ManagerController : ControllerBase
    {
        private readonly IManagerService _managerService;

        public ManagerController(IManagerService managerService)
        {
            _managerService = managerService;
        }
        
        // ---------------------------------------------------------------------------------------
        // ------------------------------------社团信息获取-----------------------------------------
        // ---------------------------------------------------------------------------------------    
        
        //根据用户id获取社团名称
        [HttpPost("getClubName")]
        [ProducesResponseType(typeof(NameVO), 200)]
        public IActionResult GetClubName()
        {
            var userId = Utils.GetCurrentUserId(this.User);
            return Ok(new NameVO {Name = _managerService.GetClubName(userId)});
        }

        // ---------------------------------------------------------------------------------------
        // ------------------------------------活动管理--------------------------------------------
        // ---------------------------------------------------------------------------------------    
        
        
        
        //-------------------------------------活动查询--------------------------------------------
        
        //获取活动列表并分页
        [HttpPost("getActivities")]
        [ProducesResponseType(typeof(PaginatedList<ActivityVO>), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetActivities([FromBody] PageQO pq)
        {
            var userId = Utils.GetCurrentUserId(this.User);
            var acts = _managerService.GetActs(userId, pq.Query);
            if (acts == null) return NotFound();
            return Ok(PaginatedList<ActivityVO>.Create(acts, pq.PageNumber ?? 1, pq.PageSize));
        }
        
        //通过id获取一条活动记录
        [HttpPost("getOneActivity/{id}")]
        [ProducesResponseType(typeof(ActivityVO), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetOneActivity(long id)
        {
            var userId = Utils.GetCurrentUserId(this.User);
            var act = _managerService.GetOneAct(userId, id);
            if (act == null)
            {
                return NotFound();
            }

            return Ok(act);
        }

        //-------------------------------------活动增加--------------------------------------------
        //增加一条活动记录
        [HttpPost("addOneActivity")]
        [ProducesResponseType(200)]
        public IActionResult AddOneActivity([FromBody] ActivityQO aq)
        {
            var userId = Utils.GetCurrentUserId(this.User);
            _managerService.AddAct(aq, userId);
            return Ok();
        }

        //-------------------------------------活动更新--------------------------------------------
        //根据id更新一条活动记录
        [HttpPost("updateOneActivity/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult UpdateOneActivity([FromBody] ActivityQO aq)
        {
            var userId = Utils.GetCurrentUserId(this.User);
            bool success = _managerService.UpdateAct(aq, userId);
            if (success) return Ok();
            return NotFound();
        }

        //--------------------------------------活动删除--------------------------------------------
        //根据id删除一条活动记录
        [HttpPost("deleteOneActivity/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult DeleteOneActivity(long id)
        {
            var userId = Utils.GetCurrentUserId(this.User);
            var exist = _managerService.DeleteAct(id, userId);
            if (exist) return Ok();
            return NotFound();
        }
        

        // ---------------------------------------------------------------------------------------
        // ------------------------------------公告管理--------------------------------------------
        // ---------------------------------------------------------------------------------------    
        
        
        
        
        //-------------------------------------公告查询--------------------------------------------
        
        //获取公告列表并分页
        [HttpPost("getAnnouncements")]
        [ProducesResponseType(typeof(PaginatedList<AnnouncementVO>), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetAnnouncements([FromBody] PageQO pq)
        {
            var userId = Utils.GetCurrentUserId(this.User);
            var announces = _managerService.GetAnnounces(userId, pq.Query);
            if ( announces== null) return NotFound();
            return Ok(PaginatedList<AnnouncementVO>.Create(announces, pq.PageNumber ?? 1, pq.PageSize));
        }
        
        //根据id获取一条公告
        [HttpPost("getOneAnnouncement/{id}")]
        [ProducesResponseType(typeof(AnnouncementVO),200)]
        public IActionResult GetOneAnnouncement(long id)
        {
            var userId = Utils.GetCurrentUserId(this.User);
            var announce = _managerService.GetOneAnnounce(userId, id);
            if (announce == null)
            {
                return NotFound();
            }
            return Ok(announce);
        }
        
        //-------------------------------------公告增加--------------------------------------------
        //增加一条公告记录
        [HttpPost("addOneAnnouncement")]
        [ProducesResponseType(200)]
        public IActionResult AddOneAnnouncement([FromBody] AnnouncementQO aq)
        {
            var userId = Utils.GetCurrentUserId(this.User);
            _managerService.AddAnnounce(aq, userId);
            return Ok();
        }
        
        //-------------------------------------公告更新--------------------------------------------
        //更新一条公告记录
        //根据id更新一条活动记录
        [HttpPost("updateOneAnnouncement/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult UpdateOneAnnouncement([FromBody] AnnouncementQO aq)
        {
            var userId = Utils.GetCurrentUserId(this.User);
            var success = _managerService.UpdateAnnounce(aq, userId);
            if (success) return Ok();
            return NotFound();
        }
        
        //--------------------------------------公告删除--------------------------------------------
        //根据id删除一条公告记录
        [HttpPost("deleteOneAnnouncement/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult deleteOneAnnouncement(long id)
        {
            var userId = Utils.GetCurrentUserId(this.User);
            var exist = _managerService.DeleteAnnounce(id, userId);
            if (exist) return Ok();
            return NotFound();
        }

        
        // ---------------------------------------------------------------------------------------
        // ------------------------------------成员管理--------------------------------------------
        // ---------------------------------------------------------------------------------------   
        
        //-------------------------------------成员查询--------------------------------------------
        
        //获取成员列表并分页
        [HttpPost("getClubMembers")]
        [ProducesResponseType(typeof(PaginatedList<StudentVO>), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetClubMembers([FromBody] PageQO pq)
        {
            var userId = Utils.GetCurrentUserId(this.User);
            var mems = _managerService.GetClubMem(userId, pq.Query);
            if (mems == null) return NotFound();
            return Ok(PaginatedList<StudentVO>.Create(mems, pq.PageNumber ?? 1, pq.PageSize));
        }
        
        //--------------------------------------成员删除--------------------------------------------
        
        //清理社团成员
        [HttpPost("deleteClubMember/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult DeleteClubMember(long id)
        {
            var userId = Utils.GetCurrentUserId(this.User);
            var exist = _managerService.DeleteClubMem(id, userId);
            if (exist) return Ok();
            return NotFound();
        }
        
        //--------------------------------------社团换届--------------------------------------------
        
        //社长换届
        [HttpPost("changeManager/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult ChangeManager(long id)
        {
            var userId = Utils.GetCurrentUserId(this.User);
            var success = _managerService.ChangeManager(id, userId);
            if (success) return Ok();
            return NotFound();
        }


        //--------------------------------------申请赞助--------------------------------------------
        //申请赞助
        [HttpPost("applySponsorship")]
        [ProducesResponseType(200)]
        public IActionResult ApplySponsorship([FromBody] SponsorshipQO aq)
        {
            var userId = Utils.GetCurrentUserId(this.User);
            _managerService.ApplySponsorship(aq, userId);
            return Ok();
        }


    }
}