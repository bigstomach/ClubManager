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