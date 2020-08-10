using System.Linq;
using ClubManager.QueryObjects;
using ClubManager.ViewObjects;

namespace ClubManager.Services
{
    public interface IManagerService
    {
        //获取社团名称
        string GetClubName(long clubId);
        //活动管理
        IQueryable<ActivityVO> GetActs(long clubId, string query);
        ActivityVO GetOneAct(long clubId, long id);
        void AddAct(long clubId,ActivityQO actQO);
        bool UpdateAct(long clubId,ActivityQO actQO);
        bool DeleteAct(long clubId,long id);
        //公告管理
        IQueryable<AnnouncementVO> GetAnnounces(long clubId, string query);
        AnnouncementVO GetOneAnnounce(long clubId, long id);
        void AddAnnounce(long clubId,AnnouncementQO announceQO);
        bool UpdateAnnounce(long clubId,AnnouncementQO announceQO);
        bool DeleteAnnounce(long clubId,long id);
        //成员管理
        IQueryable<MemberVO> GetClubMem(long clubId, string query);
        MemberVO GetOneClubMem(long clubId, long id);
        bool DeleteClubMem(long clubId,long id);
        IQueryable<MemberVO> GetCandidates(long clubId, string query);
        bool ChangeManager(long clubId,long id);
        IQueryable<ManagerVO> GetManagers(long clubId, string query);
        void AddOneSponsorship(long clubId,SponsorshipQO sponsorshipQO);
        bool UpdateClubInfo(long clubId, ClubQO ClQO);
        IQueryable<SponsorshipVO> GetClubHadSponsorship(long clubId);
        IQueryable<MemberVO> GetActivityMem(long ActivityId, string query);


    }
}