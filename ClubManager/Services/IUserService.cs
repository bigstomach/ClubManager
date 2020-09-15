using System.Linq;
using ClubManager.QueryObjects;
using ClubManager.ViewObjects;

namespace ClubManager.Services
{
    public interface IUserService
    {
        AuthUser Authenticate(LoginQO log);
        void Register(RegisterQO reg);
        bool ChangePassword(long userId,PasswordQO pas);
        IQueryable<MessageVO> GetMessages(long userId);
        MessageVO GetOneMessage(long userId, long id);
        bool SetMessageRead(long userId,long id);
        IQueryable<AnnouncementVO> GetAnnouncements();
        AnnouncementVO GetOneAnnouncement(long id);

        
    }
}
