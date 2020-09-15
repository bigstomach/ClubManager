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
        [HttpPost("getSponsorshipDetails")]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(SponsorshipVO),200)]
        public IActionResult GetSponsorshipDetails([FromBody]SponsorshipIdQO sponsorshipId)
        {
            var Sponsorship = _adminService.GetSponsorshipDetails(sponsorshipId.SponsorshipId);
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
        [HttpPost("getActivityDetails")]
        [ProducesResponseType(typeof(ActivityVO),200)]
        [ProducesResponseType(404)]
        public IActionResult GetActivityDetails([FromBody] ActivityIdQO activityId)
        {
            var Activity = _adminService.GetActivityDetails(activityId.ActivityId);
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

        //获取社团列表
        [HttpPost("getClubs")]
        [ProducesResponseType(typeof(PaginatedList<ClubVO>),200)]
        [ProducesResponseType(404)]
        public ActionResult<PaginatedList<ClubVO>> GetClubs([FromBody] ClubListQO ClubPage)
        {
            if (ClubPage.Status != "unaudited" && ClubPage.Status != "dissolved" && ClubPage.Status != "pass" && ClubPage.Status != "all") return NotFound();
            var Club = _adminService.GetClubs(ClubPage.Status, ClubPage.PageQO.Query);
            if (Club == null) return NotFound();
            else return Ok(PaginatedList<ClubVO>.Create(Club, ClubPage.PageQO.PageNumber ?? 1, ClubPage.PageQO.PageSize));
        }

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

        //社团审核状态更新
        [HttpPost("updateClubStatus")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult UpdateClubStatus([FromBody] ClubStatusQO newClubStatus)
        {
            var exist = _adminService.UpdateClubStatus(newClubStatus);
            if (exist) return Ok();
            else return NotFound();
        }


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
        [HttpPost("updateGraduate")]
        [ProducesResponseType(typeof(SuccessVO), 200)]
        public IActionResult UpdateGraduate([FromBody] StudentNumberQO studentNumber)
        {
            SuccessVO success = new SuccessVO
            {
                IsSuccess = _adminService.UpdateGraduate(studentNumber.Number)
            };
            return Ok(success);
        }

        //删除学生信息
        [HttpPost("deleteStudentMeta")]
        [ProducesResponseType(typeof(SuccessVO), 200)]
        public IActionResult DeleteStudentMeta([FromBody] StudentNumberQO studentNumber)
        {
            SuccessVO success = new SuccessVO
            {
                IsSuccess = _adminService.DeleteStudentMeta(studentNumber.Number)
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
        // ---------------------------------------------------------------------------------------
        // ------------------------------------公告管理--------------------------------------------
        // ---------------------------------------------------------------------------------------    




        //-------------------------------------公告查询--------------------------------------------

        //获取公告列表并分页
        [HttpPost("getAnnouncements")]
        [ProducesResponseType(typeof(PaginatedList<AnnouncementVO>), 200)]
        public IActionResult GetAnnouncements([FromBody] PageQO pq)
        {
            var adminId = Utils.GetCurrentUserId(this.User);
            var announces = _adminService.GetAnnounces(adminId, pq.Query);
            return Ok(PaginatedList<AnnouncementVO>.Create(announces, pq.PageNumber ?? 1, pq.PageSize));
        }

        //根据公告id获取一条公告
        [HttpPost("getOneAnnouncement/{announcementId}")]
        [ProducesResponseType(typeof(AnnouncementVO), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetOneAnnouncement(long announcementId)
        {
            var adminId = Utils.GetCurrentUserId(this.User);
            var announce = _adminService.GetOneAnnounce(adminId, announcementId);
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
            var adminId = Utils.GetCurrentUserId(this.User);
            _adminService.AddAnnounce(adminId, aq);
            return Ok();
        }

        //-------------------------------------公告更新--------------------------------------------
        //更新一条公告记录
        //根据id更新一条活动记录
        [HttpPost("updateOneAnnouncement")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult UpdateOneAnnouncement([FromBody] AnnouncementQO aq)
        {
            var adminId = Utils.GetCurrentUserId(this.User);
            var success = _adminService.UpdateAnnounce(adminId, aq);
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
            var adminId = Utils.GetCurrentUserId(this.User);
            var exist = _adminService.DeleteAnnounce(adminId, id);
            if (exist) return Ok();
            return NotFound();
        }
    }
}