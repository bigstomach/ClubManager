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
                }).FirstOrDefault(s => s.SpecificationId == id);
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
                    AdminName=s.Admin.Name,
                    Status = s.Status
                }).AsNoTracking();
            if (query=="unaudited")//���Ҵ���˵�����
            {
                Spon = Spon.Where(s => s.Status == 0);
            }
            else if (query=="failed")//�������δͨ��������
            {
                Spon = Spon.Where(s => s.Status == 2);
            }
            else if (query=="pass")//���������ͨ��������
            {
                Spon = Spon.Where(s => s.Status == 1);
            }
            //��������������ֱ�ӷ�������ֵ

            return Spon;
        }
    }
}