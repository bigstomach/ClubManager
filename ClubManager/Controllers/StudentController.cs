using System.Linq;
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
        // ---------------------------------------------------------------------------------------
        // ------------------------------------学生部分--------------------------------------------
        // ---------------------------------------------------------------------------------------  

        //获取学生姓名
        [HttpPost("getStudentName")]
        [ProducesResponseType(typeof(NameVO), 200)]
        public IActionResult GetName()
        {
            var studentId = Utils.GetCurrentUserId(this.User);
            var name = _studentService.GetStudentName(studentId);
            return Ok(name);
        }

        //获取学生全部个人信息
        [HttpPost("getStudentInfo")]
        [ProducesResponseType(typeof(StudentAllVO), 200)]
        public IActionResult GetStudentInfo()
        {
            var studentId = Utils.GetCurrentUserId(this.User);
            var stu = _studentService.GetStudentInfo(studentId);
            if (stu == null)
            {
                return NotFound();
            }
            return Ok(stu);
        }

        //修改学生账户信息
        [HttpPost("changeStudentInfo")]
        [ProducesResponseType(typeof(SuccessVO), 200)]
        public IActionResult ChangeStudentInfo([FromBody] StudentsQO stuQO)
        {
            var studentId = Utils.GetCurrentUserId(this.User);
            SuccessVO success = new SuccessVO
            {
                IsSuccess = _studentService.ChangeStudentInfo(studentId, stuQO)
            };
            return Ok(success);
        }

        //获取已加入社团的公告
        [HttpPost("inClubAnnouncements")]
        [ProducesResponseType(typeof(PaginatedList<InClubAnnouncementVO>), 200)]
        public IActionResult GetClubAnnouncements([FromBody] PageQO pq)
        {
            var studentId = Utils.GetCurrentUserId(this.User);
            var announces = _studentService.GetInClubAnnouncements(studentId);
            return Ok(PaginatedList<InClubAnnouncementVO>.Create(announces, pq.PageNumber ?? 1, pq.PageSize));
        }

        //消息发送（学生主动退出社团及退出活动，会向相应账号发送消息）
        [HttpPost("sendMessage")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult SendMessage([FromBody] MessageQO message)
        {
            var exist = _studentService.SendMessage(message);
            if (exist) return Ok();
            else return NotFound();
        }

        // ---------------------------------------------------------------------------------------
        // ------------------------------------社团部分--------------------------------------------
        // ---------------------------------------------------------------------------------------  

        //获取全部社团信息并分页
        [HttpPost("getClubInfo")]
        [ProducesResponseType(typeof(PaginatedList<ClubVO>), 200)]
        public ActionResult<PaginatedList<ClubVO>> GetClubInfo([FromBody] PageQO pq)
        {
            var clubs = _studentService.GetClubInfo(pq.Query);
            return Ok(PaginatedList<ClubVO>.Create(clubs, pq.PageNumber ?? 1, pq.PageSize));
        }

        //获取学生已加入社团的列表并分页
        [HttpPost("inClub")]
        [ProducesResponseType(typeof(PaginatedList<ClubVO>), 200)]
        [ProducesResponseType(404)]
        public IActionResult InClub([FromBody] PageQO pq)
        {
            var userId = Utils.GetCurrentUserId(this.User);
            var clubs = _studentService.SearchInClub(userId, pq.Query);
            return Ok(PaginatedList<ClubVO>.Create(clubs, pq.PageNumber ?? 1, pq.PageSize));
        }

        //加入社团之前先通过社团id判断是否加入或已申请
        [HttpPost("judgeClubJoin/{id}")]
        [ProducesResponseType(typeof(SuccessVO), 200)]
        [ProducesResponseType(404)]
        public IActionResult JudgeClubJoin(long id)
        {
            var studentId = Utils.GetCurrentUserId(this.User);
            SuccessVO success = new SuccessVO
            {
                IsSuccess = _studentService.JudgeClubJoin(id,studentId)
            };
            return Ok(success);
        }
        //学生申请加入社团
        [HttpPost("joinClub")]
        [ProducesResponseType(typeof(SuccessVO), 200)]
        public IActionResult JoinClub(JoinClub1QO newJoinClubQO)
        {
            var studentId = Utils.GetCurrentUserId(this.User);
            SuccessVO success = new SuccessVO
            {
                IsSuccess = _studentService.JoinClub(studentId, newJoinClubQO)
            };
            return Ok(success);
        }

        //通过社团ID返回社团简介
        [HttpPost("getClubDescription/{id}")]
        [ProducesResponseType(typeof(PaginatedList<ClubVO>), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetClubDescription(long id)
        {
            var userId = Utils.GetCurrentUserId(this.User);
            var des = _studentService.GetClubDescription(id);
            if (des == null)
            {
                return NotFound();
            }

            return Ok(des);
        }

        //退出某一社团
        [HttpPost("quitOneClub/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult QuitOneClub(long id)
        {
            var studentId = Utils.GetCurrentUserId(this.User);
            var exist = _studentService.QuitOneClub(studentId, id);
            if (exist) return Ok(new { msg = "成功退出社团" });
            return NotFound();
        }

        // ---------------------------------------------------------------------------------------
        // ------------------------------------活动部分--------------------------------------------
        // ---------------------------------------------------------------------------------------  
        //获取已加入社团内部活动信息并分页
        [HttpPost("getInActivityInfo")]
        [ProducesResponseType(typeof(PaginatedList<ActivityVO>), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetInActivityInfo([FromBody] PageQO pq)
        {
            var studentId = Utils.GetCurrentUserId(this.User);
            var acts = _studentService.GetInActivitiesInfo(studentId,pq.Query);
            //var inacts = _studentService.SearchInActivity(studentId, pq.Query);
            //var dif = acts.Except(inacts);
            return Ok(PaginatedList<ActivityVO>.Create(acts, pq.PageNumber ?? 1, pq.PageSize));
        }

        //获取所有公开活动信息并分页
        [HttpPost("getOutActivityInfo")]
        [ProducesResponseType(typeof(PaginatedList<ActivityVO>), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetOutActivityInfo([FromBody] PageQO pq)
        {
            //var studentId = Utils.GetCurrentUserId(this.User);
            //var inacts = _studentService.SearchInActivity(studentId, pq.Query);
            var acts = _studentService.GetOutActivitiesInfo( pq.Query);
            //var dif = acts.Except(inacts);
            return Ok(PaginatedList<ActivityVO>.Create(acts, pq.PageNumber ?? 1, pq.PageSize));
        }

        //获取已参加活动信息并分页
        [HttpPost("inActivity")]
        [ProducesResponseType(typeof(PaginatedList<ActivityVO>), 200)]
        [ProducesResponseType(404)]
        public IActionResult InActivity([FromBody] PageQO pq)
        {
            var userId = Utils.GetCurrentUserId(this.User);
            var acts = _studentService.SearchInActivity(userId, pq.Query);
            return Ok(PaginatedList<ActivityVO>.Create(acts, pq.PageNumber ?? 1, pq.PageSize));
        }

        //学生申请参加活动
        [HttpPost("joinActivity")]
        [ProducesResponseType(typeof(SuccessVO), 200)]
        public IActionResult ParticipateActivity(ParticipateActivityQO newParticipateActivityQO)
        {
            var studentId = Utils.GetCurrentUserId(this.User);
            SuccessVO success = new SuccessVO
            {
                IsSuccess = _studentService.ParticipateActivity(studentId, newParticipateActivityQO)
            };
            return Ok(success);
        }
    }
}
