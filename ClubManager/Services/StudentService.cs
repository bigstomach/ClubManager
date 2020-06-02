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

        public string GetStudentName(long userId)
        {
            return _context.Students.First(s => s.UserId == userId).Name;
        }
    }
}