using System;
using System.Linq;
using ClubManager.QueryObjects;
using ClubManager.ViewObjects;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace ClubManager.Services
{
    public class ManagerService: IManagerService
    {
        private readonly ModelContext _context;
        
        public ManagerService(ModelContext context)
        {
            _context = context;
        }

        private Clubs GetRelatedClub(long id)
        {
            return _context.Clubs.FirstOrDefault(c => c.UserId == id);
        }

        public IQueryable<ActivitiesVO> GetActivities(long userId, string query)
        {
            var acts = _context.Activities
                .Where(a => a.ClubId== GetRelatedClub(userId).ClubId)
                .Select(act=>new ActivitiesVO
                {
                    ActivityId = act.ActivityId,
                    Name = act.Name,
                    Fund = act.Fund,
                    Cost =act.Cost,
                    ApplyDate = act.ApplyDate,
                    Place = act.Place,
                    Description = act.Description,
                    Time = act.Time,
                    Status = act.Status,
                    IsPublic = act.IsPublic
                });
            if (!String.IsNullOrEmpty(query))
            {
                acts = acts.Where(a => a.Name.Contains(query));
            }

            return acts;
        }

        public Activities AddAct(ActQO aq, long userId)
        {
            var newAct=new Activities
            {
                Name = aq.Name,
                Fund = aq.Fund,
                Cost =aq.Cost,
                Place = aq.Place,
                Time = aq.Time,
                ApplyDate = DateTime.Now,
                Status = false,
                Description = aq.Description,
                ClubId= GetRelatedClub(userId).ClubId,
                IsPublic = aq.IsPublic
            };
            _context.Activities.Add(newAct);
            _context.SaveChanges();
            var newActAudit=new ActivityAudit
            {
                ActivityId = newAct.ActivityId,
                Status = false,
            };
            _context.ActivityAudit.Add(newActAudit);
            _context.SaveChanges();
            return newAct;
        }

        public ActivitiesVO GetActivities(long userId, long id)
        {
            var oneAct = _context.Activities
                .Where(a => a.ClubId == GetRelatedClub(userId).ClubId && a.ActivityId == id)
                .Select(act => new ActivitiesVO
                {
                    ActivityId = act.ActivityId,
                    Name = act.Name,
                    Fund = act.Fund,
                    Cost = act.Cost,
                    ApplyDate = act.ApplyDate,
                    Place = act.Place,
                    Description = act.Description,
                    Time = act.Time,
                    Status = act.Status,
                    IsPublic = act.IsPublic
                }).FirstOrDefault();
            return oneAct;
        }

        public bool UpdateAct(UpdateActQO aq, long userId)
        {
            var Act = _context.Activities.FirstOrDefault(a =>
                a.ClubId == GetRelatedClub(userId).ClubId && a.ActivityId == aq.ActivityId);
            if (Act == null)
                return false;
            Act.Fund = aq.Fund;
            Act.Cost = aq.Cost;
            Act.Description = aq.Description;
           _context.SaveChanges();
            return true;
        }

        public bool DeleteAct(long id, long userId)
        {
            var a = _context.Activities.Find(id);
            if (a == null||a.ClubId!=GetRelatedClub(userId).ClubId) return false;
            _context.Activities.Remove(a);
            _context.SaveChanges();
            return true;
        }

    }
}