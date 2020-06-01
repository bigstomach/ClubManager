using System.Linq;
using ClubManager.QueryObjects;
using ClubManager.ViewObjects;

namespace ClubManager.Services
{
    public interface IManagerService
    {
        IQueryable<ActivitiesVO> GetActivities(long userId,string query);
        Activities AddAct(ActQO aq, long userId);
        ActivitiesVO GetActivities(long userId, long id);
        bool UpdateAct(UpdateActQO aq, long userId);
        bool DeleteAct(long id,long userId);
    }
}