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


        //获取所有社团制度并分页
        [HttpPost("getSpecifications")]
        [ProducesResponseType(typeof(PaginatedList<SpecVO>), 200)]
        [ProducesResponseType(404)]
        public ActionResult<PaginatedList<SpecVO>> GetSpec([FromBody] PageQO pq)
        {
            var spec = _adminService.GetSpec(pq.Query);
            if (spec == null) return NotFound();
            return Ok(PaginatedList<SpecVO>.Create(spec, pq.PageNumber ?? 1, pq.PageSize));
        }

        //提交一条新的社团制度
        [HttpPost("addOneSpecification")]
        [ProducesResponseType(200)]
        public IActionResult addOneSpec([FromBody] SpecQO ps)
        {
            var userId = Utils.GetCurrentUserId(this.User);
            var spec = _adminService.AddSpec(ps, userId);
            return Ok();
        }

        //通过id获取一条社团制度
        [HttpPost("getOneSpecification/{id}")]
        [ProducesResponseType(typeof(SpecVO), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetSpecById(long id)
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
        public IActionResult PutSpec([FromBody] SpecQO ps)
        {
            var userId = Utils.GetCurrentUserId(this.User);
            _adminService.PutSpec(ps, ps.SpecificationId, userId);
            return Ok();
        }

        //通过id删除一条社团制度
        [HttpPost("deleteOneSpecification/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult DeleteSpec(long id)
        {
            var exist = _adminService.DeleteSpec(id);
            if (exist) return Ok();
            return NotFound();
        }

        //增加一条新生的记录
        [HttpPost("addOneStudentInfo")]
        [ProducesResponseType(200)]
        public IActionResult AddNewStudent(NewStuQO stu)
        {
            var newStu = _adminService.AddNewStudent(stu);
            if (newStu == null) return BadRequest(new {msg = "Number already exist"});
            return Ok();
        }

        //修改一条社团赞助审核（我认为添加赞助审核应该是社团社长方面进行的，管理员方面应该完成的是添加审核意见和结果这些内容）
        [HttpPost("putOneSponsorshipAudit/{id}")]
        [ProducesResponseType(200)]
        public IActionResult PutSponsorAudit([FromBody] PostSponsorAuditQO psa)
        {
            var UserId = Utils.GetCurrentUserId(this.User);//貌似是获取当前的用户id？
            _adminService.PutSponsorAudit(psa,psa.SponsorshipAuditId,UserId);
            return Ok();
        }
    }
}