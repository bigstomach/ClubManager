using System.Collections.Generic;
using System.Linq;
using ClubManager.ViewObjects;

namespace ClubManager.Services
{
    public interface IStudentService
    {
        IQueryable<ClubVO> GetClubInfo(string query);
        string GetClubDescription(long clubId);
        IQueryable<ClubVO> SearchInClub(long id, string query);
        string GetStudentName(long userId);
    }
}
