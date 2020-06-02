using System.Linq;
using ClubManager.QueryObjects;
using ClubManager.ViewObjects;

namespace ClubManager.Services
{
    public interface IAdminService
    {
        IQueryable<SpecVO> GetSpec(string query);
        Specifications PostSpec(PostSpecQO ps,long userId);
        SpecVO GetSpec(long id);
        void PutSpec(PostSpecQO ps, long id, long userId);
        bool DeleteSpec(long id);
        Students AddNewStudent(NewStuQO stu);
    }
}