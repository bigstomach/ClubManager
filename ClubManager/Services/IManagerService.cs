using System.Linq;
using ClubManager.QueryObjects;
using ClubManager.ViewObjects;

namespace ClubManager.Services
{
    public interface IManagerService
    {
        //获取社团名称
        string GetClubName(long userId);
        //活动管理
        IQueryable<ActivityVO> GetActs(long userId, string query);
        ActivityVO GetOneAct(long userId, long id);
        void AddAct(ActivityQO actQuery, long userId);
        bool UpdateAct(ActivityQO actQuery, long userId);
        bool DeleteAct(long id, long userId);
        //公告管理
        IQueryable<AnnouncementVO> GetAnnounces(long userId, string query);
        AnnouncementVO GetOneAnnounce(long userId, long id);
        void AddAnnounce(AnnouncementQO announceQuery, long userId);
        bool UpdateAnnounce(AnnouncementQO announceQuery, long userId);
        bool DeleteAnnounce(long id, long userId);
        //成员管理
        IQueryable<MemberVO> GetClubMem(long userId, string query);
        MemberVO GetOneClubMem(long userId, long id);
        bool DeleteClubMem(long id, long userId);
        IQueryable<MemberVO> GetNextMem(long userId, string query);
        bool ChangeManager(long id, long userId);
        void AddOneSponsorship(SponsorshipQO sponsorshipQuery, long userId);

    }
}