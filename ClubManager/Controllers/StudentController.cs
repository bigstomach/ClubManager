using ClubManager.helpers;
using ClubManager.Services;
using ClubManager.Helpers;
using ClubManager.QueryObjects;
using ClubManager.ViewObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace ClubManager.Controllers
{
    [Authorize(Roles = "Student")]
    [Route("[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;


        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        //获取学生已加入社团的列表
        [HttpPost("inClub")]
        [ProducesResponseType(typeof(PaginatedList<ClubVO>), 200)]
        [ProducesResponseType(404)]
        public IActionResult InClub([FromBody] PageQO pq)
        {
            var userId = Utils.GetCurrentUserId(this.User);
            var username = Utils.GetCurrentUsername(this.User);
            var clubs = _studentService.SearchInClub(userId, pq.Query, pq.Status);
            if (clubs == null) return NotFound();
            return Ok(PaginatedList<ClubVO>.Create(clubs, pq.PageNumber ?? 1, pq.PageSize));
        }

        //获取学生姓名
        [HttpPost("getStudentName")]
        [ProducesResponseType(typeof(NameVO), 200)]
        public IActionResult GetName()
        {
            var userId = Utils.GetCurrentUserId(this.User);
            return Ok(new NameVO {Name = _studentService.GetStudentName(userId)});
        }
    }
}