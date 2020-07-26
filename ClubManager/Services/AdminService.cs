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

        public IQueryable<StudentVO> GetStudentInfo(string query)
        {
            var stu = _context.Students
                .Select(s => new StudentVO
                {
                    Grade = s.Grade,
                    Major = s.Major,
                    Name = s.Name,
                    Number = s.Number,
                    Phone = s.Phone,
                    StudentId = s.StudentId
                }).AsNoTracking();
            if (!String.IsNullOrEmpty(query))
            {
                stu = stu.Where(s => s.Name.Contains(query));
            }

            return stu;
        }

        public SpecificationVO GetSpec(long id)
        {
            var spec = _context.Specifications
                .Select(s => new SpecificationVO
                {
                    SpecificationId = s.SpecificationId,
                    AdminName = s.User.Name,
                    Name = s.Name,
                    Content = s.Content,
                    Date = s.Date
                }).AsNoTracking().FirstOrDefault(s => s.SpecificationId == id);
            return spec;
        }

        public IQueryable<SpecificationVO> GetSpec(string query)
        {

            var spec = _context.Specifications
                .Select(s => new SpecificationVO
                {
                    SpecificationId = s.SpecificationId,
                    AdminName = s.User.Name,
                    Name = s.Name,
                    Content = s.Content,
                    Date = s.Date
                })
                .AsNoTracking();
            
            if (!String.IsNullOrEmpty(query))
            {
                spec = spec.Where(c => c.Name.Contains(query));
            }

            return spec;
        }

        public Specifications AddSpec(SpecificationQO ps, long userId)
        {
            var newSpec = new Specifications
                {Content = ps.Content, Date = DateTime.Now, Name = ps.Name, UserId = userId};
            _context.Specifications.Add(newSpec);
            _context.SaveChanges();
            return newSpec;
        }

        public void PutSpec(SpecificationQO ps, long id, long userId)
        {
            var newSpec = new Specifications
                {SpecificationId = id, Content = ps.Content, Date = DateTime.Now, Name = ps.Name, UserId = userId};
            _context.Entry(newSpec).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public bool DeleteSpec(long id)
        {
            var s = _context.Specifications.Find(id);
            if (s == null) return false;
            _context.Specifications.Remove(s);
            _context.SaveChanges();
            return true;
        }

        public Students AddNewStudent(NewStudentQO student)
        {
            if (_context.Students.FirstOrDefault(s => s.Number == student.Number) != null) return null;
            var newStu = new Students
                {Number = student.Number, Name = student.Name, Grade = student.Grade, Major = student.Major, Phone = student.Phone};
            _context.Students.Add(newStu);
            _context.SaveChanges();
            return newStu;
        }

        public IQueryable<SponsorshipVO> GetSponsorship(string query)
        {
            var Spon = _context.Sponsorships.Select(
                s => new SponsorshipVO
                {
                    SponsorshipId = s.SponsorshipId,
                    ClubName = s.Club.Name,
                    ApplyTime = s.ApplyTime,
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