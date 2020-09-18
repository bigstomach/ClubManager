using System;
using System.Collections.Generic;
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

        //获取本社团信息
        public ClubVO GetClubInfo(long clubId)
        {
            var club = _context.Clubs
                .Where(a => a.ClubId == clubId )
                .Select(a => new ClubVO
                {
                    ClubId=a.ClubId,
                    Name=a.Name,
                    Description=a.Description,
                    EstablishmentDate=a.EstablishmentDate,
                    Type=a.Type,
                    Logo=a.Logo
                })
                .AsNoTracking()
                .First();
            var manager = (from man in _context.Managers
                join stu in _context.Students on man.StudentId equals stu.StudentId
                join stuMeta in _context.StudentMeta on stu.Number equals stuMeta.Number
                where man.ClubId == clubId && man.Term == DateTime.Now.Year
                select new StudentAllVO
                {
                    Number = stuMeta.Number,
                    Name = stuMeta.Name,
                    Grade = stuMeta.Grade,
                    Major = stuMeta.Major,
                    Mail = stu.Mail,
                    Phone = stu.Phone
                }).First();
            club.Number = manager.Number;
            club.PresidentName = manager.Name;
            club.Grade = manager.Grade;
            club.Major = manager.Major;
            club.Mail = manager.Mail;
            club.Phone = manager.Phone;
            return club;
        }
        //--------------------------------社团信息修改-----------------------------------
        public bool EditClubInfo(long clubId, ClubQO ClQO)
        {
            _context.Clubs.Find(clubId).Name = ClQO.Name;
            _context.Clubs.Find(clubId).Description = ClQO.Description;
            _context.Clubs.Find(clubId).Type = ClQO.Type;
            _context.SaveChanges();
            return true;
        }
         //--------------------------------社团解散-----------------------------------
        public bool DissolveClub(long clubId)
        {
            _context.Clubs.Find(clubId).Status = 0;
            _context.SaveChanges();
            return true;
        }

        //查看入社申请
        public IQueryable<JoinClubVO> GetJoin(long clubId,string query)
        {
            var joinclub = (
                from jc in _context.JoinClub
                join stu in _context.Students on jc.StudentId equals stu.StudentId
                where jc.ClubId == clubId && jc.Status==false
                orderby jc.ApplyDate descending
                select new JoinClubVO
                {
                    
                    Number=stu.Number,
                    StudentName=stu.NumberNavigation.Name,
                    ApplyDate=jc.ApplyDate,
                    ApplyReason=jc.ApplyReason,
                    Status=jc.Status
                }).AsNoTracking();
                if (!String.IsNullOrEmpty(query))
            {
                joinclub = joinclub.Where(JoinClub => JoinClub.StudentName.Contains(query));
            }
            return joinclub;
        }
        //查看一个入社申请
        public JoinClubVO GetOneJoin(long clubId, long StudentId)
        {
            var join = _context.JoinClub
                .Where(a => a.ClubId == clubId && a.StudentId == StudentId)
                .Select(a => new JoinClubVO
                {
                   StudentName =a.Student.NumberNavigation.Name,
                    Number = a.Student.Number,
                    ApplyDate = a.ApplyDate,
                    ApplyReason = a.ApplyReason,
                    Status =a.Status
                })
                .AsNoTracking()
                .FirstOrDefault();
            return join;
        }
        public void DeleteJoin(long clubId, long studentId)
        {
            var joinclub = _context.JoinClub.FirstOrDefault(a =>
               a.ClubId == clubId && a.StudentId == studentId);
          
            _context.JoinClub.Remove(joinclub);
           
            _context.SaveChanges();
         
        }
        public void OkJoin(long clubId, long studentId)
        {
            var joinclub = _context.JoinClub.FirstOrDefault(a =>
               a.ClubId == clubId && a.StudentId == studentId);
            joinclub.Status = true;
          
           
            _context.SaveChanges();
        }
        //发送消息
        public bool SendMessage(MessageQO message)
        {
            var User = _context.Users.Find(message.UserId);
            if (string.IsNullOrEmpty(message.Title)) return false;//设置消息的标题不能为空
            if (User == null) return false;
            var Message = new Messages
            {
                MessageId = _context.Messages.Select(m => m.MessageId).Max() + 1,
                UserId = message.UserId,
                Title = message.Title,
                Content = message.Content,
                Time = DateTime.Now,
                Read = false
            };
            _context.Messages.Add(Message);
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
       //获取所有申请参加本社团活动的申请
        public IQueryable<ParticipateActivityVO> GetAllActivityApply( string query,long clubId)
        {
            var apply = (
                from ParticipateActivity in _context.ParticipateActivity
                join activity in _context.Activities
                    on ParticipateActivity.ActivityId equals activity.ActivityId
                where  activity.ClubId==clubId
                orderby activity.ActivityId
                select new ParticipateActivityVO
                {
                    Number = ParticipateActivity.Student.Number,
                    StudentName= ParticipateActivity.Student.NumberNavigation.Name,
                    ActivityId = ParticipateActivity.ActivityId,
                    ActivityName = ParticipateActivity.Activity.Name,
                    ApplyDate = ParticipateActivity.ApplyDate,
                    ApplyReason= ParticipateActivity.ApplyReason,
                    Status= ParticipateActivity.Status
                }).AsNoTracking();
            if (!String.IsNullOrEmpty(query))
            {
                apply = apply.Where(m => m.ActivityName.Contains(query));
            }

            return apply;
        }
        
       //获取一个待审核的活动人员
       public ParticipateActivityVO GetOneWaitActivityMembers(long studentId, long activityId)
        {
            var part =( from pa in _context.ParticipateActivity
                       join stu in _context.Students on pa.StudentId equals stu.StudentId
                       join stumeta in _context.StudentMeta on stu.Number equals stumeta.Number
                       join act in _context.Activities on pa.ActivityId equals act.ActivityId
                       where  stu.StudentId == studentId && pa.ActivityId == activityId 
                       select new ParticipateActivityVO
                      {
                        Number = stu.Number,
                        StudentName = stumeta.Name,
                        ActivityId = pa.ActivityId,
                        ActivityName = act.Name,
                        ApplyDate = pa.ApplyDate,
                        ApplyReason = pa.ApplyReason,
                        Status = pa.Status
                      })
                .AsNoTracking()
                 .FirstOrDefault();
            return part;
        }
        public void DeleteParticipate(long activityId, long studentId)
        {
            var part = _context.ParticipateActivity.FirstOrDefault(a =>
               a.ActivityId == activityId && a.StudentId == studentId);
            _context.ParticipateActivity.Remove(part);
            _context.SaveChanges();

        }
        public void OkParticipate(long activityId, long studentId)
        {
            var part = _context.ParticipateActivity.FirstOrDefault(a =>
               a.ActivityId == activityId && a.StudentId == studentId);
            part.Status = true;
            _context.SaveChanges();
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
            var clubName = GetClubName(clubId);
            var msg = new Messages
            {
                Content = "很抱歉地通知您，您被" + clubName + "清退", Time = DateTime.Now, Read = false, UserId = id,
                Title = "社团清退消息"
            };
            _context.JoinClub.Remove(mem);
            _context.Messages.Add(msg);
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
        public bool ChangeManager(long clubId, long id)
        {
            var manager = _context.Managers.FirstOrDefault(m => m.Term == DateTime.Now.Year + 1 && m.ClubId == clubId);
            if (manager != null)
            {
                _context.Managers.Remove(manager);
            }
            var newManager = new Managers
            {
                ClubId = clubId, StudentId = id, Term = DateTime.Now.Year + 1
            };
            var clubName = GetClubName(clubId);
            var msg = new Messages
            {
                Content = "恭喜您！您被" + clubName + "选为" + (DateTime.Now.Year + 1) + "届社长", Time = DateTime.Now,
                Read = false, UserId = id, Title = "竞选成功消息"
            };
            _context.Managers.Add(newManager);
            _context.Messages.Add(msg);
            _context.SaveChanges();
            return true;
        }
        
        //查看历届社长
        public IQueryable<ManagerVO> GetManagers(long clubId, string query)
        {
            var mans = (from m in _context.Managers
                join s in _context.Students on m.StudentId equals s.StudentId
                join sm in _context.StudentMeta on s.Number equals sm.Number
                where m.ClubId == clubId
                orderby m.Term descending
                select new ManagerVO
                {
                    Term = m.Term, Number = s.Number, Name = sm.Name, Grade = sm.Grade, Major = sm.Major,
                    Phone = s.Phone
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
                 Status =0,
                AdminId=null
            };
            _context.Sponsorships.Add(newSponsorship);
            _context.SaveChanges();
        }
        
        //查看已有赞助
        public IQueryable<SponsorshipVO> GetClubHadSponsorship(long clubId,string Query)
        {
            var sponsorships = (
                from Sponsorships in _context.Sponsorships
                where Sponsorships.ClubId == clubId
                orderby Sponsorships.ApplyDate descending
                select new SponsorshipVO
                {
                    SponsorshipId = Sponsorships.SponsorshipId,
                    Sponsor = Sponsorships.Sponsor,
                    Amount = Sponsorships.Amount,
                    Requirement = Sponsorships.Requirement,
                    ApplyDate = Sponsorships.ApplyDate,
                    Status = Sponsorships.Status,
                    Suggestion = Sponsorships.Suggestion,
                    AdminId = Sponsorships.AdminId
                }).AsNoTracking();
                if (!String.IsNullOrEmpty(Query))
            {
                sponsorships = sponsorships.Where(Sponsorships => Sponsorships.Sponsor.Contains(Query));
            }
            return sponsorships;
        }
        //获取一条详细赞助信息
        public SponsorshipVO GetOneHadSponsorship(long SponsorshipId)
        {
            var spo = _context.Sponsorships
                .Where(a => a.SponsorshipId == SponsorshipId)
                .Select(a => new SponsorshipVO
                {
                    SponsorshipId = a.SponsorshipId,
                    Sponsor = a.Sponsor,
                    Amount = a.Amount,
                    ApplyDate = a.ApplyDate,
                    Requirement = a.Requirement,
                    Status = a.Status,
                    Suggestion = a.Suggestion,
                    AdminId = a.AdminId
                })
                .AsNoTracking()
                .FirstOrDefault();
            return spo;
        }
        
        //获取社团图表数据
        public ClubGraphVO GetCommunityGraph(long clubId)
        {
            var graph = (from jc in _context.JoinClub
                join stu in _context.Students on jc.StudentId equals stu.StudentId
                join stuMeta in _context.StudentMeta on stu.Number equals stuMeta.Number
                where jc.ClubId == clubId && stuMeta.Status && jc.Status
                select new StuClub
                {
                    StudentId=stu.StudentId,
                    Grade = stuMeta.Grade,
                    Major = stuMeta.Major
                }).AsNoTracking();

            var gradeGraph = graph
                .GroupBy(g => g.Grade)
                .Select(g => new GradeGroup
                {
                    Grade = g.Key,
                    Count = g.Count()
                }).ToList();

            List<string> gradeGraphDescription=new List<string>();
            List<int> gradeGraphData=new List<int>();
            
            foreach (var gradeData in gradeGraph)
            {
                gradeGraphDescription.Add(gradeData.Grade-2000+"级");
                gradeGraphData.Add(gradeData.Count);
            }
            

            var majorGraph = graph
                .GroupBy(g => g.Major)
                .Select(g => new MajorGroup
                {
                    Major = g.Key.ToString(),
                    Count = g.Count()
                }).ToList();

            List<KeyValue> majorGraphData = majorGraph.Select(majorData => new KeyValue {Value = majorData.Count, Name = majorData.Major}).ToList();

            var clubGraph = new ClubGraphVO
            {
                GradeGraphDescription = gradeGraphDescription,
                GradeGraphData = gradeGraphData,
                MajorGraphData = majorGraphData
            };
            
            return clubGraph;
        }
    }
}
