using System;
using System.Linq;
using ClubManager.QueryObjects;
using ClubManager.ViewObjects;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
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

        //根据id返回社团
        private Clubs GetRelatedClub(long id)
        {
            return _context.Clubs.SingleOrDefault(c => c.UserId == id);
        }

        //获取社团名称
        public string GetClubName(long userId)
        {
            return GetRelatedClub(userId).Name;
        }

        //--------------------------------活动增删改查-----------------------------------

        //获取活动列表
        public IQueryable<ActivityVO> GetActs(long userId, string query)
        {
            var acts = (from activity in _context.Activities
                where activity.ClubId == GetRelatedClub(userId).ClubId
                orderby activity.ApplyDate descending
                select new ActivityVO
                {
                    ActivityId = activity.ActivityId,
                    Name = activity.Name,
                    Fund = activity.Fund,
                    Cost = activity.Cost,
                    ApplyDate = activity.ApplyDate,
                    Place = activity.Place,
                    Description = activity.Description,
                    Time = activity.Time,
                    Status = activity.Status,
                    IsPublic = activity.IsPublic,
                    Suggestion = activity.Suggestion
                }).AsNoTracking();
            if (!String.IsNullOrEmpty(query))
            {
                acts = acts.Where(a => a.Name.Contains(query));
            }

            return acts;
        }

        //获取一条活动记录
        public ActivityVO GetOneAct(long userId, long id)
        {
            var act = GetActs(userId, "").SingleOrDefault(a => a.ActivityId == id);
            return act;
        }

        //增加一条活动记录
        public void AddAct(ActivityQO actQuery, long userId)
        {
            var newAct = new Activities
            {
                Name = actQuery.Name,
                Fund = actQuery.Fund,
                Cost = actQuery.Cost,
                Place = actQuery.Place,
                Time = actQuery.Time,
                ApplyDate = DateTime.Now,
                Description = actQuery.Description,
                ClubId = GetRelatedClub(userId).ClubId,
                IsPublic = actQuery.IsPublic
            };

            _context.Activities.Add(newAct);
            _context.SaveChanges();
        }

        //更新一条活动记录
        public bool UpdateAct(ActivityQO actQuery, long userId)
        {
            var act = _context.Activities.SingleOrDefault(a =>
                a.ClubId == GetRelatedClub(userId).ClubId && a.ActivityId == actQuery.ActivityId);

            if (act == null)
                return false;

            act.Name = actQuery.Name;
            act.Fund = actQuery.Fund;
            act.Cost = actQuery.Cost;
            act.Place = actQuery.Place;
            act.Description = actQuery.Description;
            act.Time = actQuery.Time;
            act.IsPublic = actQuery.IsPublic;

            _context.SaveChanges();
            return true;
        }

        //删除一条活动记录
        public bool DeleteAct(long id, long userId)
        {
            var act = _context.Activities.Find(id);
            if (act == null || act.ClubId != GetRelatedClub(userId).ClubId) return false;

            _context.Activities.Remove(act);
            _context.SaveChanges();
            return true;
        }

        //--------------------------------公告增删改查-----------------------------------

        //获取公告列表
        public IQueryable<AnnouncementVO> GetAnnounces(long userId, string query)
        {
            var announces = (from announcements in _context.Announcements
                where announcements.ClubId == GetRelatedClub(userId).ClubId
                orderby announcements.Time descending
                select new AnnouncementVO
                {
                    AnnouncementId = announcements.AnnouncementId,
                    Title = announcements.Title,
                    Content = announcements.Content,
                    Time = announcements.Time
                }).AsNoTracking();
            if (!String.IsNullOrEmpty(query))
            {
                announces = announces.Where(a => a.Title.Contains(query));
            }

            return announces;
        }

        //获取一条公告记录
        public AnnouncementVO GetOneAnnounce(long userId, long id)
        {
            var announce = GetAnnounces(userId, "").SingleOrDefault(a => a.AnnouncementId == id);
            return announce;
        }

        //增加一条公告记录
        public void AddAnnounce(AnnouncementQO announceQuery, long userId)
        {
            var newAnnounce = new Announcements
            {
                Content = announceQuery.Content,
                ClubId = GetRelatedClub(userId).ClubId,
                Time = DateTime.Now,
                Title = announceQuery.Title
            };
            _context.Announcements.Add(newAnnounce);
            _context.SaveChanges();
        }

        //更新一条公告记录
        public bool UpdateAnnounce(AnnouncementQO announceQuery, long userId)
        {
            var announce = _context.Announcements.SingleOrDefault(a =>
                a.ClubId == GetRelatedClub(userId).ClubId && a.AnnouncementId == announceQuery.AnnouncementId);
            if (announce == null) return false;
            announce.Title = announceQuery.Title;
            announce.Content = announceQuery.Content;
            _context.SaveChanges();
            return true;
        }

        //删除一条公告记录
        public bool DeleteAnnounce(long id, long userId)
        {
            var announce =
                _context.Announcements.SingleOrDefault(a =>
                    a.AnnouncementId == id && a.ClubId == GetRelatedClub(userId).ClubId);
            if (announce == null) return false;

            _context.Announcements.Remove(announce);
            _context.SaveChanges();
            return true;
        }

        //--------------------------------成员删查-----------------------------------
        //获取成员列表
        public IQueryable<MemberVO> GetClubMem(long userId, string query)
        {
            var mems = (from joinClub in _context.JoinClubs
                join stu in _context.Students
                    on joinClub.StudentId equals stu.StudentId
                where joinClub.ClubId == GetRelatedClub(userId).ClubId && joinClub.Status == true
                orderby stu.Number
                select new MemberVO
                {
                    StudentId = stu.StudentId,
                    Number = stu.Number,
                    Name = stu.Name,
                    Major = stu.Major,
                    Grade = stu.Grade,
                    Phone = stu.Phone
                }).AsNoTracking();
            if (!String.IsNullOrEmpty(query))
            {
                mems = mems.Where(m => m.Name.Contains(query));
            }

            return mems;
        }

        //获取一条成员信息
        public MemberVO GetOneClubMem(long userId, long id)
        {
            var mem = GetClubMem(userId, "").SingleOrDefault(m => m.StudentId == id);
            return mem;
        }

        //清理社团成员
        public bool DeleteClubMem(long id, long userId)
        {
            var mem = _context.JoinClubs.SingleOrDefault(jc =>
                    jc.StudentId == id && jc.ClubId == GetRelatedClub(userId).ClubId && jc.Status == true);
            if (mem == null) return false;
            _context.JoinClubs.Remove(mem);
            _context.SaveChanges();
            return true;
        }
        //--------------------------------换届管理-----------------------------------

        //获取下届成员列表
        public IQueryable<MemberVO> GetNextMem(long userId, string query)
        {
            var club = GetRelatedClub(userId);
            var grade = _context.Students.Single(s => s.StudentId == club.StudentId).Grade;
            var mems = (from joinClub in _context.JoinClubs
                join stu in _context.Students
                    on joinClub.StudentId equals stu.StudentId
                where joinClub.ClubId == club.ClubId && joinClub.Status == true && stu.Grade == grade + 1
                select new MemberVO
                {
                    StudentId = stu.StudentId,
                    Number = stu.Number,
                    Name = stu.Name,
                    Major = stu.Major,
                    Grade = stu.Grade,
                    Phone = stu.Phone,
                }).AsNoTracking();
            if (!String.IsNullOrEmpty(query))
            {
                mems = mems.Where(m => m.Name.Contains(query));
            }

            return mems;
        }

        //社长换届
        public bool ChangeManager(long id, long userId)
        {
            var club = GetRelatedClub(userId);
            var grade = _context.Students.Single(s => s.StudentId == club.StudentId).Grade;
            var newLeaderGrade = (from joinClub in _context.JoinClubs
                join stu in _context.Students
                    on joinClub.StudentId equals stu.StudentId
                where joinClub.ClubId == club.ClubId && joinClub.Status == true && stu.StudentId== id
                select stu.Grade).SingleOrDefault();
            if (newLeaderGrade == null || newLeaderGrade != grade + 1) return false;
            club.StudentId = id;
            _context.SaveChanges();
            return true;
        }

        //--------------------------------申请赞助-----------------------------------
        //申请赞助
        public void AddOneSponsorship(SponsorshipQO sponsorshipQuery, long userId)
        {
            var newSponsorship = new Sponsorships
            {
                ApplyTime = DateTime.Now,
                ClubId = GetRelatedClub(userId).ClubId,
                Sponsor = sponsorshipQuery.Sponsor,
                Amount = sponsorshipQuery.Amount,
                Requirement = sponsorshipQuery.Requirement
            };
            _context.Sponsorships.Add(newSponsorship);
            _context.SaveChanges();
        }
    }
}