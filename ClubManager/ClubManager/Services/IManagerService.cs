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
<<<<<<< Updated upstream
        IQueryable<StudentVO> GetClubMem(long userId, string query);
        bool DeleteClubMem(long id, long userId);
        bool ChangeManager(long id, long userId);
=======
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
>>>>>>> Stashed changes
    }
}