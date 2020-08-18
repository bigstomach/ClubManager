using System.Collections.Generic;
using System.Linq;
using ClubManager.ViewObjects;
using ClubManager.QueryObjects;

namespace ClubManager.Services
{
    public interface IStudentService
    {
        //个人部分（修改学生个人信息）
        IQueryable<NameVO> GetStudentName(long studentId);
        bool ChangeStudentInfo(long studentId, StudentsQO stuQO);
        IQueryable<StudentAllVO> GetStudentInfo(long studentId);
        IQueryable<InClubAnnouncementVO> GetInClubAnnouncements(long studentId);
        bool SendMessage(MessageQO message);
        //社团部分（查询社团信息/申请加入社团）
        IQueryable<ClubVO> GetClubInfo(string query);
        string GetClubDescription(long clubId);
        IQueryable<ClubVO> SearchInClub(long id, string query);
        bool JoinClub(long StudentId, JoinClub1QO newJoinClubQO);
        bool QuitOneClub(long studentId,long id);
        //活动部分（查询活动信息/申请参加活动）
        IQueryable<ActivityVO> GetInActivitiesInfo(long studentId,string query);
        IQueryable<ActivityVO> GetOutActivitiesInfo(string query);
        IQueryable<ActivityVO> SearchInActivity(long id, string query);
        bool ParticipateActivity(long StudentId, ParticipateActivityQO newParticipateActivityQO);
    }
}
