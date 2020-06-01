using System.Linq;
using ClubManager.QueryObjects;
using ClubManager.ViewObjects;

namespace ClubManager.Services
{
    public interface IManagerService
    {
        IQueryable<ActivitiesVO> GetActs(long userId,string query);
        Activities AddAct(ActQO aq, long userId);
        ActivitiesVO GetOneAct(long userId, long id);
        bool UpdateAct(UpdateActQO aq, long userId);
        bool DeleteAct(long id,long userId);
    }
}