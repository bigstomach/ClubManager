using System.Collections.Generic;
using System.Linq;
using ClubManager.ViewObjects;

namespace ClubManager.Services
{
    public interface IStudentService
    {
        IQueryable<ClubVO> SearchInClub(long id, string query, bool status);
        IQueryable<ClubVO> GetClubInfo(string query);
        string GetClubDescription(long ClubId);
        string GetStudentName(long userId);
    }
}
