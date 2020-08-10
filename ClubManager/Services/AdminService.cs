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

        public IQueryable<StudentMetaVO> GetStudentInfo(string query)
        {
            var stu = _context.StudentMeta
                .Select(s => new StudentMetaVO
                {
                    Number = s.Number,
                    Name = s.Name,
                    Grade = s.Grade,
                    Major = s.Major,
                    Status = s.Status
                }).AsNoTracking();
            if (!String.IsNullOrEmpty(query))
            {
                stu = stu.Where(s => s.Name.Contains(query));
            }

            return stu;
        }
        

        public bool AddNewStudent(NewStudentQO stuQO)
        {
            if (_context.StudentMeta.FirstOrDefault(s => s.Number == stuQO.Number) != null) return false;
            var newStu = new StudentMeta
                { Number = stuQO.Number, Name = stuQO.Name, Grade = stuQO.Grade, Major = stuQO.Major};
            _context.StudentMeta.Add(newStu);
            _context.SaveChanges();
            return true;
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

        public bool UpdateSuggestion(SponsorshipSuggestionQO newsuggestion,long userId)
        {
            long SponsorshipId = newsuggestion.SponsorshipId;
            var Sponsorship = _context.Sponsorships.Find(SponsorshipId);
            if (Sponsorship == null) return false;
            _context.Sponsorships.Attach(Sponsorship);//仅修改某个属性中的元素值
            Sponsorship.Suggestion = newsuggestion.Suggestion;
            Sponsorship.AdminId = userId;
            _context.SaveChanges();
            return true;
        }

        public bool UpdateStatus(SponsorshipStatusQO newStatus,long UserId)
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

    }
}