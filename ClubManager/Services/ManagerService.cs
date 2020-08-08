using System;
using System.Linq;
using System.Security.Permissions;
using ClubManager.QueryObjects;
using ClubManager.ViewObjects;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Remotion.Linq.Parsing.Structure.IntermediateModel;

namespace ClubManager.Services
{
    public class ManagerService : IManagerService
    {
        private readonly ModelContext _context;

        public ManagerService(ModelContext context)
        {
            _context = context;
        }
        
        //获取社团名称
        public string GetClubName(long clubId)
        {
            return _context.Clubs.Find(clubId).Name;
        }

        //--------------------------------社团信息修改-----------------------------------
        public bool UpdateClubInfo(long clubId, ClubQO ClQO)
        {
            _context.Clubs.Find(clubId).Name = ClQO.Name;
            _context.Clubs.Find(clubId).Description = ClQO.Description;
            _context.Clubs.Find(clubId).Type = ClQO.Type;
            _context.SaveChanges();
            return true;
        }

        //--------------------------------活动增删改查-----------------------------------

        //获取活动列表
        public IQueryable<ActivityVO> GetActs(long clubId, string query)
        {
            var acts = _context.Activities
                .Where(a => a.ClubId == clubId)
                .OrderBy(a => a.ApplyDate)
                .Select(a => new ActivityVO {
                    ActivityId = a.ActivityId, Name = a.Name, Budget = a.Budget, ApplyDate = a.ApplyDate, Place = a.Place, Description = a.Description, EventTime = a.EventTime, IsPublic = a.IsPublic, Status = a.Status, Suggestion = a.Suggestion
                }).AsNoTracking();
            if (!String.IsNullOrEmpty(query))
            {
                acts = acts.Where(a => a.Name.Contains(query));
            }

            return acts;
        }

        //获取一条活动记录
        public ActivityVO GetOneAct(long clubId, long id)
        {
            var act = _context.Activities
                .Where(a => a.ClubId == clubId && a.ActivityId == id)
                .Select(a => new ActivityVO {
                        ActivityId = a.ActivityId, Name = a.Name, Budget = a.Budget, ApplyDate = a.ApplyDate, Place = a.Place, Description = a.Description, EventTime = a.EventTime, IsPublic = a.IsPublic, Status = a.Status, Suggestion = a.Suggestion
                    })
                .AsNoTracking()
                .FirstOrDefault();
            return act;
        }

        //增加一条活动记录
        public void AddAct(long clubId,ActivityQO actQO)
        {
            var newAct = new Activities {
                ClubId = clubId, Name = actQO.Name, Budget = actQO.Budget,Place = actQO.Place, EventTime = actQO.EventTime, ApplyDate = DateTime.Now, Description = actQO.Description, IsPublic = actQO.IsPublic
            };
            _context.Activities.Add(newAct);
            _context.SaveChanges();
        }

        //更新一条活动记录
        public bool UpdateAct(long clubId,ActivityQO actQO)
        {
            var act = _context.Activities.FirstOrDefault(a =>
                a.ClubId == clubId && a.ActivityId == actQO.ActivityId);

            if (act == null)
                return false;

            act.Name = actQO.Name; act.Budget = actQO.Budget; act.Place = actQO.Place; act.Description = actQO.Description; act.EventTime = actQO.EventTime; act.IsPublic = actQO.IsPublic;

            _context.SaveChanges();
            return true;
        }

        //删除一条活动记录
        public bool DeleteAct(long clubId,long id)
        {
            var act = _context.Activities.FirstOrDefault(a => a.ActivityId == id && a.ClubId == clubId);
            if (act == null) return false;
            _context.Activities.Remove(act);
            _context.SaveChanges();
            return true;
        }

        //--------------------------------公告增删改查-----------------------------------

        //获取公告列表
        public IQueryable<AnnouncementVO> GetAnnounces(long clubId, string query)
        {
            var announces=_context.Announcements
                .Where(a=>a.UserId==clubId)
                .OrderByDescending(a=>a.Time)
                .Select(a=> new AnnouncementVO {
                    AnnouncementId = a.AnnouncementId, Title = a.Title, Content = a.Content, Time = a.Time
                }).AsNoTracking();
            if (!String.IsNullOrEmpty(query))
            {
                announces = announces.Where(a => a.Title.Contains(query));
            }

            return announces;
        }

        //获取一条公告记录
        public AnnouncementVO GetOneAnnounce(long clubId, long id)
        {
            var announce = _context.Announcements
                .Where(a => a.AnnouncementId == id && a.UserId == clubId)
                .Select(a => new AnnouncementVO {
                    AnnouncementId = a.AnnouncementId, Title = a.Title, Content = a.Content, Time = a.Time
                })
                .AsNoTracking()
                .FirstOrDefault();
            return announce;
        }

        //增加一条公告记录
        public void AddAnnounce(long clubId,AnnouncementQO announceQO)
        {
            var newAnnounce = new Announcements {
                Content = announceQO.Content, UserId = clubId, Time = DateTime.Now, Title = announceQO.Title
            };
            _context.Announcements.Add(newAnnounce);
            _context.SaveChanges();
        }

        //更新一条公告记录
        public bool UpdateAnnounce(long clubId,AnnouncementQO announceQO)
        {
            var announce = _context.Announcements.FirstOrDefault(a =>
                a.UserId == clubId && a.AnnouncementId == announceQO.AnnouncementId);
            if (announce == null) return false;
            announce.Title = announceQO.Title;
            announce.Content = announceQO.Content;
            _context.SaveChanges();
            return true;
        }

        //删除一条公告记录
        public bool DeleteAnnounce(long clubId,long id)
        {
            var announce =
                _context.Announcements.FirstOrDefault(a => 
                    a.AnnouncementId == id && a.UserId==clubId);
            if (announce == null) return false;
            _context.Announcements.Remove(announce);
            _context.SaveChanges();
            return true;
        }

        //--------------------------------成员删查-----------------------------------
        //获取成员列表
        public IQueryable<MemberVO> GetClubMem(long clubId, string query)
        {
            var members = (
                from joinClub in _context.JoinClub
                join stu in _context.Students
                    on joinClub.StudentId equals stu.StudentId
                join stuMeta in _context.StudentMeta 
                    on stu.Number equals stuMeta.Number     
                where joinClub.ClubId == clubId && joinClub.Status && stuMeta.Status
                orderby stu.Number
                select new MemberVO {
                    StudentId = stu.StudentId, Number = stu.Number, Name = stuMeta.Name, Major = stuMeta.Major, Grade = stuMeta.Grade, Phone = stu.Phone
                }).AsNoTracking();
            if (!String.IsNullOrEmpty(query))
            {
                members = members.Where(m => m.Name.Contains(query));
            }

            return members;
        }

        //获取一条成员信息
        public MemberVO GetOneClubMem(long clubId, long id)
        {
            var mem = GetClubMem(clubId,"").FirstOrDefault(m => m.StudentId == id);
            return mem;
        }

        //清理社团成员
        public bool DeleteClubMem(long clubId,long id)
        {
            var mem = _context.JoinClub.FirstOrDefault(jc =>
                    jc.StudentId == id && jc.ClubId == clubId && jc.Status == true);
            if (mem == null) return false;
            _context.JoinClub.Remove(mem);
            _context.SaveChanges();
            return true;
        }
        //--------------------------------换届管理-----------------------------------

        //获取社长候选人
        public IQueryable<MemberVO> GetCandidates(long clubId, string query)
        {
            var members =(
                from joinClub in _context.JoinClub
                join stu in _context.Students on joinClub.StudentId equals stu.StudentId
                join stuMeta in _context.StudentMeta on stu.Number equals stuMeta.Number
                where joinClub.ClubId == clubId 
                      && joinClub.Status 
                      && stuMeta.Status 
                      && stu.StudentId!=_context.Managers
                          .FirstOrDefault(m=>m.ClubId==clubId && m.Term == DateTime.Now.Year).StudentId
                orderby stu.Number      
                select new MemberVO {
                StudentId = stu.StudentId, Number = stu.Number, Name = stuMeta.Name, Major = stuMeta.Major, Grade = stuMeta.Grade, Phone = stu.Phone
            }).AsNoTracking();
            
            if (!String.IsNullOrEmpty(query))
            {
                members = members.Where(m => m.Name.Contains(query));
            }

            return members;
        }

        //社长换届
        public bool ChangeManager(long clubId,long id)
        {
            var manager = _context.Managers.FirstOrDefault(m => m.Term == DateTime.Now.Year+1 && m.ClubId==clubId);
            if (manager != null)
            {
                manager.StudentId = id;
            }
            else
            {
                var newManager = new Managers
                {
                    ClubId = clubId, StudentId = id, Term = DateTime.Now.Year + 1
                };
                _context.Managers.Add(newManager);
            }
            _context.SaveChanges();
            return true;
        }
        
        //查看历届社长
        public IQueryable<ManagerVO> GetManagers(long clubId,string query)
        {
            var mans = (from m in _context.Managers
                join s in _context.Students on m.StudentId equals s.StudentId
                join sm in _context.StudentMeta on s.Number equals sm.Number
                where m.ClubId == clubId
                orderby m.Term descending
                select new ManagerVO
                {
                    StudentId = s.StudentId, Number = s.Number, Grade = sm.Grade, Major = sm.Major, Phone = s.Phone
                }).AsNoTracking();
            if (!String.IsNullOrEmpty(query))
            {
                mans = mans.Where(m => m.Name.Contains(query));
            }

            return mans;
        }

        //--------------------------------申请赞助-----------------------------------
        //申请赞助
        public void AddOneSponsorship(long clubId,SponsorshipQO sponsorshipQO)
        {
            var newSponsorship = new Sponsorships
            {
                ApplyDate = DateTime.Now,
                ClubId = clubId,
                Sponsor = sponsorshipQO.Sponsor,
                Amount = sponsorshipQO.Amount,
                Requirement = sponsorshipQO.Requirement,
                AdminId=null
            };
            _context.Sponsorships.Add(newSponsorship);
            _context.SaveChanges();
        }
    }
}