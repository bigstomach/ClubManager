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
        
        [HttpGet("inClub")]
        [ProducesResponseType(typeof(PaginatedList<ClubVO>),200)]
        public IActionResult InClub([FromBody]StuInClubQO sc)
        {
            var id = Utils.GetCurrentUserId(this.User);
            var username = Utils.GetCurrentUsername(this.User);
            var clubs = _studentService.SearchInClub(id,sc.Query,sc.Status);
            return Ok(PaginatedList<ClubVO>.Create(clubs,sc.PageNumber ?? 1,sc.PageSize));
        }

    }

}