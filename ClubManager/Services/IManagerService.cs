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
        bool EditClubInfo(long clubId, ClubQO ClQO);
        bool DissolveClub(long clubId);
        IQueryable<SponsorshipVO> GetClubHadSponsorship(long clubId,string Query);
        IQueryable<MemberVO> GetActivityMem(long ActivityId, string query);

        IQueryable<JoinClubVO> GetJoin(long clubId, string query);
        SponsorshipVO GetOneHadSponsorship(long SponsorshipId);
        ClubVO GetClubInfo(long clubId);
        JoinClubVO GetOneJoin(long ClubId, long StudentId);
        void DeleteJoin(long clubId, long studentId);
        void OkJoin(long clubId, long studentId);
        IQueryable<MemberVO> GetWaitActivityMem(long ActivityId, string query);
        ParticipateActivityVO GetOneWaitActivityMembers(long studentId, long activityId);
        void DeleteParticipate(long activityId, long studentId);

        void OkParticipate(long activityId, long studentId);
        bool SendMessage(MessageQO message);
    }
}
