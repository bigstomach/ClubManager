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
        // ------------------------------------学生管理--------------------------------------------
        // ---------------------------------------------------------------------------------------  

        //获取学生信息并分页
        [HttpPost("getStudentInfo")]
        [ProducesResponseType(typeof(PaginatedList<StudentMetaVO>), 200)]
        public ActionResult<PaginatedList<StudentMetaVO>> GetStudentInfo([FromBody] PageQO pq)
        {
            var stu = _adminService.GetStudentInfo(pq.Query);
            return Ok(PaginatedList<StudentMetaVO>.Create(stu, pq.PageNumber ?? 1, pq.PageSize));
        }
        
        //增加一条新生的记录
        [HttpPost("addOneStudentInfo")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult addOneStudentInfo(NewStudentQO student)
        {
            var newStu = _adminService.AddNewStudent(student);
            if (!newStu) return NotFound(new {msg = "学号已存在"});
            return Ok();
        }
        

        // ---------------------------------------------------------------------------------------
        // ------------------------------------赞助审核--------------------------------------------
        // ---------------------------------------------------------------------------------------  

        
        //获取所有赞助申请
        [HttpPost("getSponsorships")]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(PaginatedList<SponsorshipVO>),200)]
        public ActionResult<PaginatedList<SponsorshipVO>> GetSponsorships([FromBody] PageQO SponsorshipPage)
        {
            if (SponsorshipPage.Query != "unaudited" && SponsorshipPage.Query != "failed" && SponsorshipPage.Query != "pass" && SponsorshipPage.Query != "all")
                return NotFound();
            var Sponsorships = _adminService.GetSponsorship(SponsorshipPage.Query); 
            return Ok(PaginatedList<SponsorshipVO>.Create(Sponsorships, SponsorshipPage.PageNumber ?? 1, SponsorshipPage.PageSize));
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
        [HttpPost("updateSuggestion")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public IActionResult UpdateSuggestion(SponsorshipSuggestionQO newsuggestion)
        {
            var userId = Utils.GetCurrentUserId(this.User);
            var exist = _adminService.UpdateSuggestion(newsuggestion,userId);
            if (exist) return Ok();
            else return NotFound();
        }

        [HttpPost("updateStatus")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult UpdateStatus(SponsorshipStatusQO newStatus)
        {
            if (newStatus.Status > 2 || newStatus.Status <= 0) return BadRequest();//只能修改为审核通过或者未通过，不能修改为待审核
            var userId = Utils.GetCurrentUserId(this.User);
            var exist = _adminService.UpdateStatus(newStatus, userId);
            if (exist) return Ok();
            else return NotFound();
        }
    }
}