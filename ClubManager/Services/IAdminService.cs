using System.Linq;
using ClubManager.QueryObjects;
using ClubManager.ViewObjects;

namespace ClubManager.Services
{
    public interface IAdminService
    {
        IQueryable<StudentMetaVO> GetStudentInfo(string query);
        bool AddNewStudent (NewStudentQO student);
        IQueryable<SponsorshipVO> GetSponsorship(string query);
        SponsorshipVO GetSponsorshipDetails(long id);
        bool UpdateSuggestion(SponsorshipSuggestionQO newsuggestion, long userId);
        bool UpdateStatus(SponsorshipStatusQO newStatus, long UserId);
    }
}