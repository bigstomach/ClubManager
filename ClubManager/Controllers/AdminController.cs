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
    }
}