using System.Linq;
using ClubManager.QueryObjects;
using ClubManager.ViewObjects;

namespace ClubManager.Services
{
    public interface IAdminService
    {
        IQueryable<StudentVO> GetStudentInfo(string query);
        IQueryable<SpecVO> GetSpec(string query);
        Specifications AddSpec(SpecQO ps, long userId);
        SpecVO GetSpec(long id);
        void PutSpec(SpecQO ps, long id, long userId);
        bool DeleteSpec(long id);
        Students AddNewStudent(NewStuQO stu);
        IQueryable<SponsorshipVO> GetSponsorship(string query);
    }
}