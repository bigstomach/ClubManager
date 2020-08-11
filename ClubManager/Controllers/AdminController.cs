using System;
using ClubManager.helpers;
using ClubManager.Helpers;
using ClubManager.QueryObjects;
using ClubManager.Services;
using ClubManager.ViewObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClubManager.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }
        
        // ---------------------------------------------------------------------------------------
        // ------------------------------------赞助审核--------------------------------------------
        // ---------------------------------------------------------------------------------------  

        
        //获取所有赞助申请
        [HttpPost("getSponsorships")]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(PaginatedList<SponsorshipVO>),200)]
        public ActionResult<PaginatedList<SponsorshipVO>> GetSponsorships([FromBody] SponsorshipListQO SponsorshipPage)
        {
            if (SponsorshipPage.Status != "unaudited" && SponsorshipPage.Status != "failed" && SponsorshipPage.Status != "pass" && SponsorshipPage.Status != "all") return NotFound();
            var Sponsorships = _adminService.GetSponsorship(SponsorshipPage.Status,SponsorshipPage.PageQO.Query);
            if (Sponsorships == null) return NotFound();
            else return Ok(PaginatedList<SponsorshipVO>.Create(Sponsorships, SponsorshipPage.PageQO.PageNumber ?? 1, SponsorshipPage.PageQO.PageSize));
        }

        //根据赞助id获取赞助详情
        [HttpPost("getSponsorshipDetails/{id}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(SponsorshipVO),200)]
        public IActionResult GetSponsorshipDetails(long id)
        {
            var Sponsorship = _adminService.GetSponsorshipDetails(id);
            if (Sponsorship == null) return NotFound();
            else return Ok(Sponsorship);
        }

        //管理员根据赞助id批复赞助
        [HttpPost("updateSponSuggestion")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public IActionResult UpdateSuggestion(SponsorshipSuggestionQO newSuggestion)
        {
            var userId = Utils.GetCurrentUserId(this.User);
            var exist = _adminService.UpdateSponSuggestion(newSuggestion,userId);
            if (exist) return Ok();
            else return NotFound();
        }

        //管理员根据赞助id审核赞助是否通过
        [HttpPost("updateSponStatus")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public IActionResult UpdateStatus(SponsorshipStatusQO newStatus)
        {
            if (newStatus.Status > 2 || newStatus.Status <= 0) return NotFound();//只能修改为审核通过或者未通过，不能修改为待审核
            var userId = Utils.GetCurrentUserId(this.User);
            var exist = _adminService.UpdateSponStatus(newStatus, userId);
            if (exist) return Ok();
            else return NotFound();
        }

        // ---------------------------------------------------------------------------------------
        // ------------------------------------活动审核-------------------------------------------
        // --------------------------------------------------------------------------------------- 

        //获取所有活动
        [HttpPost("getActivities")]
        [ProducesResponseType(typeof(PaginatedList<ActivityVO>),200)]
        [ProducesResponseType(404)]
        public ActionResult<PaginatedList<ActivityVO>> GetActivities([FromBody] ActivityListQO ActivityPage)
        {
            if (ActivityPage.Status != "unaudited" && ActivityPage.Status != "failed" && ActivityPage.Status != "pass" && ActivityPage.Status != "all") return NotFound();
            var Activity = _adminService.GetActivities(ActivityPage.Status, ActivityPage.PageQO.Query);
            if (Activity == null) return NotFound();
            else return Ok(PaginatedList<ActivityVO>.Create(Activity, ActivityPage.PageQO.PageNumber ?? 1, ActivityPage.PageQO.PageSize));
        }

        //根据活动id获取活动详细内容
        [HttpPost("getActivityDetails/{id}")]
        [ProducesResponseType(typeof(ActivityVO),200)]
        [ProducesResponseType(404)]
        public IActionResult GetActivityDetails(long id)
        {
            var Activity = _adminService.GetActivityDetails(id);
            if (Activity == null) return NotFound();
            else return Ok(Activity);
        }

        //管理员根据活动id批复
        [HttpPost("updateActSuggestion")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult UpdateActSuggestion([FromBody] ActivitySuggestionQO newActSuggestion)
        {
            var UserId = Utils.GetCurrentUserId(this.User);
            var exist = _adminService.UpdateActSuggestion(newActSuggestion, UserId);
            if (exist) return Ok();
            else return NotFound();
        }

        //管理员根据活动id审核活动是否通过
        [HttpPost("updateActStatus")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult UpdateActStatus([FromBody] ActivityStatusQO newActStatus)
        {
            var UserId = Utils.GetCurrentUserId(this.User);
            var exist = _adminService.UpdateActStatus(newActStatus, UserId);
            if (exist) return Ok();
            else return NotFound();
        }

        // ---------------------------------------------------------------------------------------
        // ------------------------------------新社团审核-----------------------------------------
        // --------------------------------------------------------------------------------------- 

        //获取社团列表(待完善）

        //获取社团详细信息
        [HttpPost("getClubDetails")]
        [ProducesResponseType(typeof(ClubVO),200)]
        [ProducesResponseType(404)]
        public IActionResult GetClubDetails([FromBody]ClubDetailsQO clubDetailsQO)
        {
            var Club = _adminService.GetClubDetails(clubDetailsQO.ClubId,clubDetailsQO.ManagerId);
            if (Club == null) return NotFound();
            else return Ok(Club);
        }

        //社团消息发送
        [HttpPost("sendMessage")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult SendMessage([FromBody]MessageQO message)
        {
            var exist = _adminService.SendMessage(message);
            if (exist) return Ok();
            else return NotFound();
        }

        //社团审核状态更新(待完善）

        // ---------------------------------------------------------------------------------------
        // ------------------------------------学生元信息管理-------------------------------------
        // ---------------------------------------------------------------------------------------

        //获取学生列表
        [HttpPost("getStudentMetas")]
        [ProducesResponseType(typeof(PaginatedList<StudentMetaVO>),200)]
        [ProducesResponseType(404)]
        public ActionResult<PaginatedList<StudentMetaVO>> GetStudentMetas([FromBody]StudentMetaListQO studentMetaList)
        {
            if (studentMetaList.Status != "graduated" && studentMetaList.Status != "atSchool" && studentMetaList.Status != "all") return NotFound();
            var studentMeta = _adminService.GetStudentMetas(studentMetaList.Status, studentMetaList.PageQO.Query);
            if (studentMeta == null) return NotFound();
            else return Ok(PaginatedList<StudentMetaVO>.Create(studentMeta, studentMetaList.PageQO.PageNumber ?? 1, studentMetaList.PageQO.PageSize));
        }

        //更新学生信息
        [HttpPost("updateStudentMeta")]
        [ProducesResponseType(typeof(SuccessVO),200)]
        public IActionResult UpdateStudentMeta([FromBody]StudentMetaQO newStudentMeta)
        {
            SuccessVO success = new SuccessVO
            {
                IsSuccess = _adminService.UpdateStudentMeta(newStudentMeta)
            };
            return Ok(success);
        }

        //更新学生毕业状态
        [HttpPost("updateGraduate/{number}")]
        [ProducesResponseType(typeof(SuccessVO), 200)]
        public IActionResult UpdateGraduate(int number)
        {
            SuccessVO success = new SuccessVO
            {
                IsSuccess = _adminService.UpdateGraduate(number)
            };
            return Ok(success);
        }

        //删除学生信息
        [HttpPost("deleteStudentMeta/{number}")]
        [ProducesResponseType(typeof(SuccessVO), 200)]
        public IActionResult DeleteStudentMeta(int number)
        {
            SuccessVO success = new SuccessVO
            {
                IsSuccess = _adminService.DeleteStudentMeta(number)
            };
            return Ok(success);
        }

        //新增学生信息
        [HttpPost("insertStudentMeta")]
        [ProducesResponseType(typeof(SuccessVO), 200)]
        public IActionResult InsertStudentMeta(StudentMetaQO newStudentMetaQO)
        {
            SuccessVO success = new SuccessVO
            {
                IsSuccess = _adminService.InsertStudentMeta(newStudentMetaQO)
            };
            return Ok(success);
        }
    }
}