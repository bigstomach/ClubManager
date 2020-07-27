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
        [ProducesResponseType(typeof(PaginatedList<StudentVO>), 200)]
        [ProducesResponseType(404)]
        public ActionResult<PaginatedList<StudentVO>> GetStudentInfo([FromBody] PageQO pq)
        {
            var stu = _adminService.GetStudentInfo(pq.Query);
            if (stu == null) return NotFound();
            return Ok(PaginatedList<StudentVO>.Create(stu, pq.PageNumber ?? 1, pq.PageSize));
        }
        
        //增加一条新生的记录
        [HttpPost("addOneStudentInfo")]
        [ProducesResponseType(200)]
        public IActionResult addOneStudentInfo(NewStudentQO student)
        {
            var newStu = _adminService.AddNewStudent(student);
            if (newStu == null) return BadRequest(new {msg = "Number already exist"});
            return Ok();
        }

        // ---------------------------------------------------------------------------------------
        // ------------------------------------系统公告--------------------------------------------
        // ---------------------------------------------------------------------------------------  

        //获取所有社团制度并分页
        [HttpPost("getSpecifications")]
        [ProducesResponseType(typeof(PaginatedList<SpecificationVO>), 200)]
        [ProducesResponseType(404)]
        public ActionResult<PaginatedList<SpecificationVO>> GetSpecifications([FromBody] PageQO pq)
        {
            var spec = _adminService.GetSpec(pq.Query);
            if (spec == null) return NotFound();
            return Ok(PaginatedList<SpecificationVO>.Create(spec, pq.PageNumber ?? 1, pq.PageSize));
        }

        //提交一条新的社团制度
        [HttpPost("addOneSpecification")]
        [ProducesResponseType(200)]
        public IActionResult AddOneSpecification([FromBody] SpecificationQO ps)
        {
            var userId = Utils.GetCurrentUserId(this.User);
            var spec = _adminService.AddSpec(ps, userId);
            return Ok();
        }

        //通过id获取一条社团制度
        [HttpPost("getOneSpecification/{id}")]
        [ProducesResponseType(typeof(SpecificationVO), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetOneSpecification(long id)
        {
            var spec = _adminService.GetSpec(id);
            if (spec == null)
            {
                return NotFound();
            }

            return Ok(spec);
        }

        //通过id修改一条社团制度
        [HttpPost("updateOneSpecification/{id}")]
        [ProducesResponseType(200)]
        public IActionResult UpdateOneSpecification([FromBody] SpecificationQO ps)
        {
            var userId = Utils.GetCurrentUserId(this.User);
            _adminService.PutSpec(ps, ps.SpecificationId, userId);
            return Ok();
        }

        //通过id删除一条社团制度
        [HttpPost("deleteOneSpecification/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult DeleteOneSpecification(long id)
        {
            var exist = _adminService.DeleteSpec(id);
            if (exist) return Ok();
            return NotFound();
        }

        // ---------------------------------------------------------------------------------------
        // ------------------------------------赞助审核--------------------------------------------
        // ---------------------------------------------------------------------------------------  

        
        //获取所有赞助申请
        [HttpPost("getSponsorships")]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(PaginatedList<SponsorshipVO>),200)]
        public ActionResult<PaginatedList<SponsorshipVO>> GetSponsorships([FromBody] PageQO SponsorshipPage)
        {
            if (SponsorshipPage.Query != "unaudited" && SponsorshipPage.Query != "failed" && SponsorshipPage.Query != "pass" && SponsorshipPage.Query != "all")
                return BadRequest();//申请的参数内容有误
            var Sponsorships = _adminService.GetSponsorship(SponsorshipPage.Query);
            if (Sponsorships == null) return NotFound();
            else return Ok(PaginatedList<SponsorshipVO>.Create(Sponsorships, SponsorshipPage.PageNumber ?? 1, SponsorshipPage.PageSize));
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
            if (newStatus.status > 2 || newStatus.status <= 0) return BadRequest();//只能修改为审核通过或者未通过，不能修改为待审核
            var userId = Utils.GetCurrentUserId(this.User);
            var exist = _adminService.UpdateStatus(newStatus, userId);
            if (exist) return Ok();
            else return NotFound();
        }
    }
}