using System;
using System.Linq;
using ClubManager.QueryObjects;
using ClubManager.ViewObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Xml;

namespace ClubManager.Services
{
    public class AdminService : IAdminService
    {
        private readonly ModelContext _context;

        public AdminService(ModelContext context)
        {
            _context = context;
        }

        public IQueryable<SponsorshipVO> GetSponsorship(string status,string query)
        {
            var Spon = _context.Sponsorships.Select(
                s => new SponsorshipVO
                {
                    SponsorshipId = s.SponsorshipId,
                    ClubName = s.Club.Name,
                    ApplyDate = s.ApplyDate,
                    Sponsor = s.Sponsor,
                    Amount = s.Amount,
                    AdminName=s.Admin.Name,
                    Status = s.Status
                }).AsNoTracking();
            if (status=="unaudited")//如果是未被审核的
            {
                Spon = Spon.Where(s => s.Status == 0);
            }
            else if (status=="failed")//如果是审核未通过
            {
                Spon = Spon.Where(s => s.Status == 2);
            }
            else if (status=="pass")//如果是审核通过的
            {
                Spon = Spon.Where(s => s.Status == 1);
            }
            //如果是full表示都显示，不进行筛选

            //模糊搜索，搜索社团名或赞助者
            if (string.IsNullOrEmpty(query)) return Spon;//如果没有模糊搜索内容，表示只筛选状态，直接返回状态筛选后的赞助
            Spon = Spon.Where(s => s.ClubName.Contains(query) || s.Sponsor.Contains(query));
            return Spon;
        }
        
        public SponsorshipVO GetSponsorshipDetails(long id)
        {
            var Sponsorship = _context.Sponsorships.Select(
                s => new SponsorshipVO
                {
                    SponsorshipId = s.SponsorshipId,
                    ClubName = s.Club.Name,
                    ApplyDate = s.ApplyDate,
                    Sponsor = s.Sponsor,
                    Amount = s.Amount,
                    AdminName=s.Admin.Name,
                    Requirement = s.Requirement,
                    Suggestion = s.Suggestion,
                    Status = s.Status
                }).AsNoTracking().FirstOrDefault(s => s.SponsorshipId == id);
            return Sponsorship;
        }

        public bool UpdateSponSuggestion(SponsorshipSuggestionQO newSuggestion,long userId)
        {
            long SponsorshipId = newSuggestion.SponsorshipId;
            var Sponsorship = _context.Sponsorships.Find(SponsorshipId);
            if (Sponsorship == null) return false;
            _context.Sponsorships.Attach(Sponsorship);//仅修改某个属性中的元素值
            Sponsorship.Suggestion = newSuggestion.Suggestion;
            Sponsorship.AdminId = userId;
            _context.SaveChanges();
            return true;
        }

        public bool UpdateSponStatus(SponsorshipStatusQO newStatus,long UserId)
        {
            long SponsorshipId = newStatus.SponsorshipId;
            var Sponsorship = _context.Sponsorships.Find(SponsorshipId);
            if (Sponsorship == null) return false;
            _context.Sponsorships.Attach(Sponsorship);
            Sponsorship.Status = newStatus.Status;
            Sponsorship.AdminId = UserId;
            _context.SaveChanges();
            return true;
        }

        public IQueryable<ActivityVO> GetActivities(string status,string query)
        {
            var Activity = _context.Activities.Select(
                a => new ActivityVO
                {
                    ActivityId = a.ActivityId,
                    ClubName = a.Club.Name,
                    Name = a.Name,
                    Budget = a.Budget,
                    Place = a.Place,
                    EventTime = a.EventTime,
                    Status = a.Status
                }
                ).AsNoTracking();
            if (status == "unaudited")//如果是未被审核的
            {
                Activity = Activity.Where(a => a.Status == 0);
            }
            else if (status == "failed")//如果是审核未通过
            {
                Activity = Activity.Where(a => a.Status == 2);
            }
            else if (status == "pass")//如果是审核通过的
            {
                Activity = Activity.Where(a => a.Status == 1);
            }
            //如果是full表示都显示，不进行筛选

            //模糊搜索
            if (string.IsNullOrEmpty(query)) return Activity;//如果模糊搜索字符串为空，直接返回，不进行模糊搜索
            Activity = Activity.Where(a => a.Name.Contains(query) || a.ClubName.Contains(query) || a.Place.Contains(query));
            return Activity;
        }

        public ActivityVO GetActivityDetails(long id)
        {
            var Activity = _context.Activities.Select(
                a => new ActivityVO
                {
                    ActivityId = a.ActivityId,
                    ClubName = a.Club.Name,
                    Name = a.Name,
                    Budget = a.Budget,
                    Place = a.Place,
                    EventTime = a.EventTime,
                    Status = a.Status,
                    ApplyDate = a.ApplyDate,
                    Description = a.Description,
                    IsPublic = a.IsPublic,
                    AdminName = a.Admin.Name,
                    Suggestion = a.Suggestion
                }
                ).AsNoTracking().FirstOrDefault(a => a.ActivityId == id);
            return Activity;
        }

        public bool UpdateActSuggestion(ActivitySuggestionQO newActSuggestion,long UserId)
        {
            var Activity = _context.Activities.Find(newActSuggestion.ActivityId);
            if (Activity == null) return false; //如果找不到，修改失败
            _context.Activities.Attach(Activity);
            Activity.AdminId = UserId;
            Activity.Suggestion = newActSuggestion.Suggestion;
            _context.SaveChanges();
            return true;
        }

        public bool UpdateActStatus(ActivityStatusQO newActStatus,long UserId)
        {
            var Activity = _context.Activities.Find(newActStatus.ActivityId);
            if (Activity == null) return false;//如果找不到，修改失败
            _context.Activities.Attach(Activity);
            Activity.AdminId = UserId;
            Activity.Status = newActStatus.Status;
            _context.SaveChanges();
            return true;
        }

        public IQueryable<ClubVO> GetClubs(string status, string query)
        {
            var nowyear = DateTime.Now.Year;
            var ManagersOfClub = (
                from Managers in _context.Managers
                where Managers.Term == nowyear
                select new
                {
                    ManageId = Managers.StudentId,
                    ClubId = Managers.ClubId,
                }).AsNoTracking();
            var Club = (
                from Clubs in _context.Clubs
                join Managers in ManagersOfClub on Clubs.ClubId equals Managers.ClubId
                join Students in _context.Students on Managers.ManageId equals Students.StudentId
                join StudentMeta in _context.StudentMeta on Students.Number equals StudentMeta.Number
                select new ClubVO
                {
                    ClubId = Clubs.ClubId,
                    Name = Clubs.Name,
                    PresidentName = StudentMeta.Name,
                    Type = Clubs.Type,
                    EstablishmentDate = Clubs.EstablishmentDate,
                    ManagerId = Managers.ManageId,
                    Status = Clubs.Status
                }).AsNoTracking();
            if (status == "unaudited")//社团状态
            {
                Club = Club.Where(c => c.Status == 2);
            }
            else if (status == "dissolved")
            {
                Club = Club.Where(c => c.Status == 0);
            }
            else if (status == "pass")
            {
                Club = Club.Where(c => c.Status == 1);
            }

            //模糊搜索
            if (string.IsNullOrEmpty(query)) return Club;
            Club = Club.Where(c => c.Name.Contains(query) || c.PresidentName.Contains(query));
            return Club;
        }

        public ClubVO GetClubDetails(long ClubId,long ManagerId)
        {
            var Club = (
                from Students in _context.Students
                join StudentMeta in _context.StudentMeta on Students.Number equals StudentMeta.Number
                from Clubs in _context.Clubs
                select new ClubVO
                {
                    ClubId = Clubs.ClubId,
                    Name = Clubs.Name,
                    Type = Clubs.Type,
                    Description = Clubs.Description,
                    EstablishmentDate = Clubs.EstablishmentDate,
                    ManagerId = Students.StudentId,
                    PresidentName = StudentMeta.Name,
                    Phone = Students.Phone,
                    Mail = Students.Mail,
                    Number = Students.Number,
                    Grade = StudentMeta.Grade,
                    Major = StudentMeta.Major
                }).AsNoTracking().FirstOrDefault(m => m.ManagerId == ManagerId && m.ClubId == ClubId);
            return Club;
        }

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

        public bool UpdateClubStatus(ClubStatusQO newClubStatus)
        {
            var Club = _context.Clubs.Find(newClubStatus.ClubId);
            if (Club == null) return false;
            _context.Clubs.Attach(Club);
            Club.Status = newClubStatus.Status;
            _context.SaveChanges();
            return true;
        }

        public IQueryable<StudentMetaVO> GetStudentMetas(string status,string query)
        {
            var student = _context.StudentMeta.Select(
                s => new StudentMetaVO
                {
                    Number = s.Number,
                    Name = s.Name,
                    Major = s.Major,
                    Grade = s.Grade,
                    Status = s.Status
                }).AsNoTracking();
            if (status == "graduated") student = student.Where(s => s.Status == false);//查找毕业学生
            if (status == "atSchool") student = student.Where(s => s.Status == true);//查找当前在读学生
            //all表示查找所有学生，不筛选状态
            //模糊搜索学生姓名，专业
            if (string.IsNullOrEmpty(query)) return student;//如果为空，直接返回，不进行模糊搜索
            student=student.Where(s => s.Name.Contains(query) || s.Major.Contains(query));
            return student;
        }

        public bool UpdateStudentMeta(StudentMetaQO newStudentMeta)
        {
            var studentMeta = _context.StudentMeta.Find(newStudentMeta.Number);
            if (studentMeta == null) return false;//如果找不到该学号的学生就更新失败
            _context.StudentMeta.Attach(studentMeta);
            studentMeta.Name = newStudentMeta.Name;
            studentMeta.Major = newStudentMeta.Major;
            studentMeta.Grade = newStudentMeta.Grade;
            studentMeta.Status = newStudentMeta.Status;
            _context.SaveChanges();
            return true;
        }

        public bool UpdateGraduate(int number)
        {
            var studentMeta = _context.StudentMeta.Find(number);
            if (studentMeta == null) return false;
            _context.StudentMeta.Attach(studentMeta);
            studentMeta.Status = false;
            _context.SaveChanges();
            return true;
        }

        public bool DeleteStudentMeta(int number)
        {
            var studentMeta = _context.StudentMeta.Find(number);
            if (studentMeta == null) return false;//如果找不到该学号的学生就删除失败
            var student = _context.Students.FirstOrDefault(s => s.Number == number);
            if (student != null) return false;//如果该学生元信息已经申请了学生账号就不能删除
            _context.StudentMeta.Remove(studentMeta);
            _context.SaveChanges();
            return true;
        }

        public bool InsertStudentMeta(StudentMetaQO newStudentMetaQO)
        {
            var studentMeta = _context.StudentMeta.Find(newStudentMetaQO.Number);
            if (studentMeta != null) return false;//如果已经存在该学号就添加失败
            var newStudentMeta = new StudentMeta
            {
                Number = newStudentMetaQO.Number,
                Name = newStudentMetaQO.Name,
                Major = newStudentMetaQO.Major,
                Grade = newStudentMetaQO.Grade,
                Status = newStudentMetaQO.Status
            };
            _context.StudentMeta.Add(newStudentMeta);
            _context.SaveChanges();
            return true;
        }

        //--------------------------------公告增删改查-----------------------------------

        //获取自己写的系统公告列表
        public IQueryable<AnnouncementVO> GetAnnounces(long adminId, string query)
        {
            var announces = _context.Announcements
                .Where(a => a.UserId == adminId)
                .OrderByDescending(a => a.Time)
                .Select(a => new AnnouncementVO
                {
                    AnnouncementId = a.AnnouncementId,
                    Title = a.Title,
                    Content = a.Content,
                    Time = a.Time
                }).AsNoTracking();
            if (!String.IsNullOrEmpty(query))
            {
                announces = announces.Where(a => a.Title.Contains(query));
            }

            return announces;
        }

        //获取一条公告记录
        public AnnouncementVO GetOneAnnounce(long adminId, long id)
        {
            var announce = _context.Announcements
                .Where(a => a.AnnouncementId == id && a.UserId == adminId)
                .Select(a => new AnnouncementVO
                {
                    AnnouncementId = a.AnnouncementId,
                    Title = a.Title,
                    Content = a.Content,
                    Time = a.Time
                })
                .AsNoTracking()
                .FirstOrDefault();
            return announce;
        }

        //增加一条公告记录
        public void AddAnnounce(long adminId, AnnouncementQO announceQO)
        {
            var newAnnounce = new Announcements
            {
                Content = announceQO.Content,
                UserId = adminId,
                Time = DateTime.Now,
                Title = announceQO.Title
            };
            _context.Announcements.Add(newAnnounce);
            _context.SaveChanges();
        }

        //更新一条公告记录
        public bool UpdateAnnounce(long adminId, AnnouncementQO announceQO)
        {
            var announce = _context.Announcements.FirstOrDefault(a =>
                a.UserId == adminId && a.AnnouncementId == announceQO.AnnouncementId);
            if (announce == null) return false;
            announce.Title = announceQO.Title;
            announce.Content = announceQO.Content;
            _context.SaveChanges();
            return true;
        }

        //删除一条公告记录
        public bool DeleteAnnounce(long adminId, long id)
        {
            var announce =
                _context.Announcements.FirstOrDefault(a =>
                    a.AnnouncementId == id && a.UserId == adminId);
            if (announce == null) return false;
            _context.Announcements.Remove(announce);
            _context.SaveChanges();
            return true;
        }
    }
}