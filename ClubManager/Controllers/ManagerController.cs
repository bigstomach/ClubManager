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
            var clubId = Utils.GetCurrentUserId(this.User);
            return Ok(new NameVO {Name = _managerService.GetClubName(clubId)});
        }

        //负责人获取本社团信息
        [HttpPost("getClubInfo")]
        [ProducesResponseType(typeof(ClubVO),200)]
        public IActionResult GetClubInfo()
        {
            var clubId = Utils.GetCurrentUserId(this.User);
            var club = _managerService.GetClubInfo(clubId);
            return Ok(club);
        }

        // ---------------------------------------------------------------------------------------
        // ------------------------------------社团信息修改-----------------------------------------
        [HttpPost("editClubInfo")]
        [ProducesResponseType(200)]
        public IActionResult UpdateClubInfo([FromBody] ClubQO aq)
        {
            var clubId = Utils.GetCurrentUserId(this.User);
            _managerService.EditClubInfo(clubId, aq);
            return Ok();
        }
         // ---------------------------------------------------------------------------------------
        // ------------------------------------社团解散-----------------------------------------
        [HttpPost("dissolveClub")]
        [ProducesResponseType(200)]
        public IActionResult DissolveClub()
        {
            var clubId = Utils.GetCurrentUserId(this.User);
            _managerService.DissolveClub(clubId);
            return Ok();
        }
        // 查看入社申请
        [HttpPost("getJoin")]
        [ProducesResponseType(typeof(PaginatedList<JoinClubVO>), 200)]
        public IActionResult GetJoin([FromBody] PageQO pq)
        {
            var clubId = Utils.GetCurrentUserId(this.User);
            var mems = _managerService.GetJoin(clubId,null);
            return Ok(PaginatedList<JoinClubVO>.Create(mems, pq.PageNumber ?? 1, pq.PageSize));
        }

        // 查看一个入社申请
        [HttpPost("getOneJoin/{studentId}")]
        [ProducesResponseType(typeof(JoinClubVO), 200)]
        public IActionResult GetOneJoin(long studentId)
        {
            var clubId = Utils.GetCurrentUserId(this.User);
            var join = _managerService.GetOneJoin(clubId, studentId);
            return Ok(join);
        }
        //入社审核结果
        [HttpPost("joinResult")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public IActionResult JoinResult(JoinClubStatusQO newStatus)
        {
            var clubId = Utils.GetCurrentUserId(this.User);
            if (newStatus.Status == false)
            {
                _managerService.DeleteJoin(clubId, newStatus.StudentId);
                return Ok();
            }
            else
            {
                _managerService.OkJoin(clubId, newStatus.StudentId);
                return Ok();
            }
        
        }



        //消息发送
        [HttpPost("sendMessage")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult SendMessage([FromBody] MessageQO message)
        {
            var exist = _managerService.SendMessage(message);
            if (exist) return Ok();
            else return NotFound();
        }
        // ---------------------------------------------------------------------------------------
        // ------------------------------------活动管理--------------------------------------------
        // ---------------------------------------------------------------------------------------    



        //-------------------------------------活动查询--------------------------------------------

        //获取活动列表并分页
        [HttpPost("getActivities")]
        [ProducesResponseType(typeof(PaginatedList<ActivityVO>), 200)]
        public IActionResult GetActivities([FromBody] PageQO pq)
        {
            var clubId = Utils.GetCurrentUserId(this.User);
            var acts = _managerService.GetActs(clubId, pq.Query);
            return Ok(PaginatedList<ActivityVO>.Create(acts, pq.PageNumber ?? 1, pq.PageSize));
        }
        
        //通过id获取一条活动记录
        [HttpPost("getOneActivity/{id}")]
        [ProducesResponseType(typeof(ActivityVO), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetOneActivity(long id)
        {
            var clubId = Utils.GetCurrentUserId(this.User);
            var act = _managerService.GetOneAct(clubId, id);
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
            var clubId = Utils.GetCurrentUserId(this.User);
            _managerService.AddAct(clubId,aq);
            return Ok();
        }

        //-------------------------------------活动更新--------------------------------------------
        //根据id更新一条活动记录
        [HttpPost("updateOneActivity/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult UpdateOneActivity([FromBody] ActivityQO aq)
        {
            var clubId = Utils.GetCurrentUserId(this.User);
            bool success = _managerService.UpdateAct(clubId,aq);
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
            var clubId = Utils.GetCurrentUserId(this.User);
            var exist = _managerService.DeleteAct(clubId,id);
            if (exist) return Ok();
            return NotFound();
        }
        //查看所有活动人员
        [HttpPost("getAllActivityMembers")]
        [ProducesResponseType(typeof(PaginatedList<ParticipateActivityVO>), 200)]
        public IActionResult GetAllActivityMembers([FromBody] PageQO pq)
        {
            var clubId = Utils.GetCurrentUserId(this.User);
            var memb = _managerService.GetAllActivityApply( pq.Query,  clubId);
            return Ok(PaginatedList<ParticipateActivityVO>.Create(memb, pq.PageNumber ?? 1, pq.PageSize));
        }
        
        // 查看一个参加活动申请
        [HttpPost("getOneActivityMember")]
        [ProducesResponseType(typeof(ParticipateActivityVO), 200)]
        public IActionResult GetOneActivityMember(OneWaitActivityMemberQO aq)
        {
            var part = _managerService.GetOneWaitActivityMembers( aq.studentId,aq.activityId);
            return Ok(part);
        }
        //参加活动审核结果
        [HttpPost("participateResult")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public IActionResult ParticipateResult(ParticipateActivityStatusQO newStatus)
        {
            if (newStatus.Status == false)
            {
                _managerService.DeleteParticipate(newStatus.ActivityId, newStatus.StudentId);
                return Ok();
            }
            else
            {
                _managerService.OkParticipate(newStatus.ActivityId, newStatus.StudentId);
                return Ok();
            }

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
            var clubId = Utils.GetCurrentUserId(this.User);
            var announces = _managerService.GetAnnounces(clubId, pq.Query);
            return Ok(PaginatedList<AnnouncementVO>.Create(announces, pq.PageNumber ?? 1, pq.PageSize));
        }
        
        //根据id获取自己社团的一条公告
        [HttpPost("getOneAnnouncement/{id}")]
        [ProducesResponseType(typeof(AnnouncementVO),200)]
        [ProducesResponseType(404)]
        public IActionResult GetOneAnnouncement(long id)
        {
            var clubId = Utils.GetCurrentUserId(this.User);
            var announce = _managerService.GetOneAnnounce(clubId, id);
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
            var clubId = Utils.GetCurrentUserId(this.User);
            _managerService.AddAnnounce(clubId,aq);
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
            var clubId = Utils.GetCurrentUserId(this.User);
            var success = _managerService.UpdateAnnounce(clubId,aq);
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
            var clubId = Utils.GetCurrentUserId(this.User);
            var exist = _managerService.DeleteAnnounce(clubId,id);
            if (exist) return Ok();
            return NotFound();
        }

        
        // ---------------------------------------------------------------------------------------
        // ------------------------------------成员管理--------------------------------------------
        // ---------------------------------------------------------------------------------------   
        
        //-------------------------------------成员查询--------------------------------------------
        
        //获取成员列表并分页
        [HttpPost("getClubMembers")]
        [ProducesResponseType(typeof(PaginatedList<MemberVO>), 200)]
        public IActionResult GetClubMembers([FromBody] PageQO pq)
        {
            var clubId = Utils.GetCurrentUserId(this.User);
            var mems = _managerService.GetClubMem(clubId, pq.Query);
            return Ok(PaginatedList<MemberVO>.Create(mems, pq.PageNumber ?? 1, pq.PageSize));
        }
        
        //获取除社长外成员列表并分页
        [HttpPost("getCandidates")]
        [ProducesResponseType(typeof(PaginatedList<MemberVO>), 200)]
        public IActionResult GetCandidates([FromBody]PageQO pq)
        {
            var clubId = Utils.GetCurrentUserId(this.User);
            var candidates = _managerService.GetCandidates(clubId, pq.Query);
            return Ok(PaginatedList<MemberVO>.Create(candidates, pq.PageNumber ?? 1, pq.PageSize));
        }
        
        
        //根据id获取成员信息
        [HttpPost("getOneClubMember/{id}")]
        [ProducesResponseType(typeof(MemberVO), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetOneClubMember(long id)
        {
            var clubId = Utils.GetCurrentUserId(this.User);
            var mem = _managerService.GetOneClubMem(clubId, id);
            if (mem == null)
            {
                return NotFound();
            }
            return Ok(mem);
        }


        //--------------------------------------成员删除--------------------------------------------
        
        //清理社团成员
        [HttpPost("deleteClubMember/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult DeleteClubMember(long id)
        {
            var clubId = Utils.GetCurrentUserId(this.User);
            var exist = _managerService.DeleteClubMem( clubId,id);
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
            var clubId = Utils.GetCurrentUserId(this.User);
            var success = _managerService.ChangeManager(clubId,id);
            if (success) return Ok();
            return NotFound();
        }
        
        //历届社长查看
        [HttpPost("getManagers")]
        [ProducesResponseType(typeof(PaginatedList<ManagerVO>),200)]
        public IActionResult GetManagers([FromBody] PageQO pq)
        {
            var clubId = Utils.GetCurrentUserId(this.User);
            var managers = _managerService.GetManagers(clubId, pq.Query);
            return Ok(PaginatedList<ManagerVO>.Create(managers, pq.PageNumber ?? 1, pq.PageSize));
        }


        //--------------------------------------申请赞助--------------------------------------------
        //申请赞助
        [HttpPost("addOneSponsorship")]
        [ProducesResponseType(200)]
        public IActionResult AddOneSponsorship([FromBody] SponsorshipQO aq)
        {
            var clubId = Utils.GetCurrentUserId(this.User);
            _managerService.AddOneSponsorship(clubId,aq);
            return Ok();
        }
        
        //查看已有赞助
        [HttpPost("getClubHadSponsorship")]
        [ProducesResponseType(typeof(PaginatedList<SponsorshipVO>), 200)]
        public IActionResult GetClubHadSponsorship([FromBody] PageQO pq)
        {
            var clubId = Utils.GetCurrentUserId(this.User);
            var mems = _managerService.GetClubHadSponsorship(clubId,pq.Query);
            return Ok(PaginatedList<SponsorshipVO>.Create(mems, pq.PageNumber ?? 1, pq.PageSize));
        }
        //查看一个详细已有赞助
        [HttpPost("getOneHadSponsorship/{SponsorshipId}")]
        [ProducesResponseType(typeof(SponsorshipVO), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetOneHadSponsorship(long SponsorshipId)
        {
            var mem = _managerService.GetOneHadSponsorship(SponsorshipId);
            if (mem == null)
            {
                return NotFound();
            }
            return Ok(mem);
        }
        
        //--------------------------------------获取图表数据--------------------------------------------

        [HttpPost("getCommunityGraph")]
        [ProducesResponseType(typeof(ClubGraphVO), 200)]
        public IActionResult GetCommunityGraph()
        {
            var clubId = Utils.GetCurrentUserId(User);
            var data = _managerService.GetCommunityGraph(clubId);
            return Ok(data);
        }
        

    }
}
