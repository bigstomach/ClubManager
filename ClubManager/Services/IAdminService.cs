using System.Linq;
using ClubManager.QueryObjects;
using ClubManager.ViewObjects;

namespace ClubManager.Services
{
    public interface IAdminService
    {
        //赞助审核
        IQueryable<SponsorshipVO> GetSponsorship(string status,string query);
        SponsorshipVO GetSponsorshipDetails(long id);
        bool UpdateSponSuggestion(SponsorshipSuggestionQO newSuggestion, long userId);
        bool UpdateSponStatus(SponsorshipStatusQO newStatus, long UserId);
        //活动审核
        IQueryable<ActivityVO> GetActivities(string status, string query);
        ActivityVO GetActivityDetails(long id);
        bool UpdateActSuggestion(ActivitySuggestionQO newActSuggestion, long UserId);
        bool UpdateActStatus(ActivityStatusQO newActStatus, long UserId);
        //社团审核
        IQueryable<ClubVO> GetClubs(string status, string query);
        ClubVO GetClubDetails(long ClubId,long ManagerId);
        bool SendMessage(MessageQO message);
        bool UpdateClubStatus(ClubStatusQO newClubStatus);
        //学生管理
        IQueryable<StudentMetaVO> GetStudentMetas(string status, string query);
        bool UpdateStudentMeta(StudentMetaQO newStudentMeta);
        bool UpdateGraduate(int number);
        bool DeleteStudentMeta(int number);
        bool InsertStudentMeta(StudentMetaQO newStudentMetaQO);

        IQueryable<AnnouncementVO> GetAnnounces(long adminId, string query);
        AnnouncementVO GetOneAnnounce(long adminId, long id);
        void AddAnnounce(long adminId, AnnouncementQO announceQO);
        bool UpdateAnnounce(long adminId, AnnouncementQO announceQO);
        bool DeleteAnnounce(long adminId, long id);
    }
}