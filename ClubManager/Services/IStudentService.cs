using System.Collections.Generic;
using System.Linq;
using ClubManager.ViewObject;

namespace ClubManager.Services
{
    public interface IStudentService
    {
        IQueryable<ClubVO> SearchInClub(long id,string query,bool status);
    }
}