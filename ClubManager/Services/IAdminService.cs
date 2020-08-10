using System.Linq;
using ClubManager.QueryObjects;
using ClubManager.ViewObjects;

namespace ClubManager.Services
{
    public interface IAdminService
    {
        IQueryable<StudentMetaVO> GetStudentInfo(string query);
        bool AddNewStudent (NewStudentQO student);
        //‘ﬁ÷˙…Û∫À
        IQueryable<SponsorshipVO> GetSponsorship(string status,string query);
        SponsorshipVO GetSponsorshipDetails(long id);
        bool UpdateSponSuggestion(SponsorshipSuggestionQO newSuggestion, long userId);
        bool UpdateSponStatus(SponsorshipStatusQO newStatus, long UserId);
        //ªÓ∂Ø…Û∫À
        IQueryable<ActivityVO> GetActivities(string status, string query);
        ActivityVO GetActivityDetails(long id);
        bool UpdateActSuggestion(ActivitySuggestionQO newActSuggestion, long UserId);
        bool UpdateActStatus(ActivityStatusQO newActStatus, long UserId);
        //…ÁÕ≈…Û∫À

        ClubVO GetClubDetails(long ClubId,long ManagerId);
        bool SendMessage(MessageQO message);
    }
}