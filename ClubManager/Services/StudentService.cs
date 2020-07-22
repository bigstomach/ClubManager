using System;
using System.Collections.Generic;
using System.Linq;
using ClubManager.ViewObjects;

namespace ClubManager.Services
{
    public class StudentService : IStudentService
    {
        private readonly ModelContext _context;

        public StudentService(ModelContext context)
        {
            _context = context;
        }

         //根据社团id 返回社团
        private Clubs GetRelatedClub(long id)
        {
            return _context.Clubs.FirstOrDefault(c => c.ClubId == id);
        }

        //根据社团id 获取社团简介
        public string GetClubDescription(long ClubId)
        {
            return GetRelatedClub(ClubId).Description;
        }
        
        //返回所有社团
        public IQueryable<ClubVO> GetClubInfo(string query)
        {
            var clubs = _context.Clubs
               .Select(s => new ClubVO
               {
                   ClubId = s.ClubId,
                   Name = s.Name,
                   Description = s.Description,
                   EstablishmentDate = s.EstablishmentDate,
                   PresidentName = s.Student.Name,
                   Type = s.Type,
               });
            if (!String.IsNullOrEmpty(query))
            {
                clubs = clubs.Where(s => s.Name.Contains(query));
            }
            return clubs;
          }

        //返回已加入社团
        public IQueryable<ClubVO> SearchInClub(long id, string query, bool status)
        {
            var clubs = from stu in _context.Set<Students>()
                join joinclub in _context.Set<JoinClubs>() on stu equals joinclub.Student
                join club in _context.Set<Clubs>() on joinclub.Club equals club
                where stu.UserId == id && joinclub.Status == status
                select new ClubVO
                {
                    ClubId = club.ClubId,
                    Name = club.Name,
                    Description = club.Description,
                    EstablishmentDate = club.EstablishmentDate,
                    PresidentName = club.Student.Name,
                    Type = club.Type,
                };
            
            if (!String.IsNullOrEmpty(query))
            {
                clubs = clubs.Where(c => c.Name.Contains(query));
            }

            return clubs;
        }
         //返回学生姓名
        public string GetStudentName(long userId)
        {
            return _context.Students.First(s => s.UserId == userId).Name;
        }
    }
}
