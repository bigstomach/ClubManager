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

        public IQueryable<SponsorshipVO> GetSponsorship(string query)
        {
            var Spon = _context.Sponsorships.Select(
                s => new SponsorshipVO
                {
                    SponsorshipId = s.SponsorshipId,
                    ClubName = s.Club.Name,
                    ApplyDate = s.ApplyDate,
                    Sponsor = s.Sponsor,
                    Amount = s.Amount,
                    adminName=s.Admin.Name,
                    Requirement=s.Requirement,
                    Suggestion=s.Suggestion,
                    Status = s.Status
                }).AsNoTracking();
            if (query=="unaudited")//如果是未被审核的
            {
                Spon = Spon.Where(s => s.Status == 0);
            }
            else if (query=="failed")//如果是审核未通过
            {
                Spon = Spon.Where(s => s.Status == 2);
            }
            else if (query=="pass")//如果是审核通过的
            {
                Spon = Spon.Where(s => s.Status == 1);
            }
            //如果是full表示都显示，不进行筛选

            return Spon;
        }

        public SponsorshipVO GetSponsorshipDetails(long id)
        {
            var Sponsorship = _context.Sponsorships.Select(
                s => new SponsorshipVO
                {
                    SponsorshipId = s.SponsorshipId,
                    ClubName = s.Club.Name,
                    ApplyTime = s.ApplyTime,
                    Sponsor = s.Sponsor,
                    Amount = s.Amount,
                    adminName=s.Admin.Name,
                    Requirement = s.Requirement,
                    Suggestion = s.Suggestion,
                    Status = s.Status
                }).AsNoTracking().FirstOrDefault(s => s.SponsorshipId == id);
            return Sponsorship;
        }

        public bool UpdateSuggestion(SponsorshipSuggestionQO newsuggestion,long userId)
        {
            long SponsorshipId = newsuggestion.sponsorshipId;
            var Sponsorship = _context.Sponsorships.Find(SponsorshipId);
            if (Sponsorship == null) return false;
            _context.Sponsorships.Attach(Sponsorship);//仅修改某个属性中的元素值
            Sponsorship.Suggestion = newsuggestion.suggestion;
            Sponsorship.AdminId = userId;
            _context.SaveChanges();
            return true;
        }

        public bool UpdateStatus(SponsorshipStatusQO newStatus,long UserId)
        {
            long SponsorshipId = newStatus.sponsorshipId;
            var Sponsorship = _context.Sponsorships.Find(SponsorshipId);
            if (Sponsorship == null) return false;
            _context.Sponsorships.Attach(Sponsorship);
            Sponsorship.Status = newStatus.status;
            Sponsorship.AdminId = UserId;
            _context.SaveChanges();
            return true;
        }

    }
}