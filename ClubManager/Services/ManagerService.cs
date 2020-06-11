using System;
using System.Linq;
using ClubManager.QueryObjects;
using ClubManager.ViewObjects;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace ClubManager.Services
{
    public class ManagerService : IManagerService
    {
        private readonly ModelContext _context;

        public ManagerService(ModelContext context)
        {
            _context = context;
        }

        //根据id返回社团
        private Clubs GetRelatedClub(long id)
        {
            return _context.Clubs.FirstOrDefault(c => c.UserId == id);
        }

        //获取社团名称
        public string GetClubName(long userId)
        {
            return GetRelatedClub(userId).Name;
        }

        //获取活动并分页
        public IQueryable<ActivitiesVO> GetActs(long userId, string query)
        {
            var acts = from activity in _context.Activities
                where activity.ClubId == GetRelatedClub(userId).ClubId
                select new ActivitiesVO
                {
                    ActivityId = activity.ActivityId,
                    Name = activity.Name,
                    Fund = activity.Fund,
                    Cost = activity.Cost,
                    ApplyDate = activity.ApplyDate,
                    Place = activity.Place,
                    Description = activity.Description,
                    Time = activity.Time,
                    Status = activity.Status,
                    IsPublic = activity.IsPublic,
                    Suggestion = activity.Suggestion
                };
            if (!String.IsNullOrEmpty(query))
            {
                acts = acts.Where(a => a.Name.Contains(query));
            }

            return acts;
        }

        public Activities AddAct(ActQO aq, long userId)
        {
            var newAct = new Activities
            {
                Name = aq.Name,
                Fund = aq.Fund,
                Cost = aq.Cost,
                Place = aq.Place,
                Time = aq.Time,
                ApplyDate = DateTime.Now,
                Description = aq.Description,
                ClubId = GetRelatedClub(userId).ClubId,
                IsPublic = aq.IsPublic
            };
            
            _context.Activities.Add(newAct);
            _context.SaveChanges();
            
            return newAct;
        }

        public ActivitiesVO GetOneAct(long userId, long id)
        {
            var act = GetActs(userId, "").FirstOrDefault(a => a.ActivityId == id);
            return act;
        }

        public bool UpdateAct(ActQO aq, long userId)
        {
            var act = _context.Activities.FirstOrDefault(a =>
                a.ClubId == GetRelatedClub(userId).ClubId && a.ActivityId == aq.ActivityId);
            
            if (act == null)
                return false;
            
            act.Name = aq.Name;
            act.Fund = aq.Fund;
            act.Cost = aq.Cost;
            act.Place = aq.Place;
            act.Description = aq.Description;
            act.Time = aq.Time;
            act.IsPublic = aq.IsPublic;
            
            _context.SaveChanges();
            return true;
        }

        public bool DeleteAct(long id, long userId)
        {
            var act = _context.Activities.Find(id);
            if (act == null || act.ClubId != GetRelatedClub(userId).ClubId) return false;
            
            _context.Activities.Remove(act);
            _context.SaveChanges();
            return true;
        }
    }
}