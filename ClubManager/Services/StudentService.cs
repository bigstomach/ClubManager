using System;
using System.Collections.Generic;
using System.Linq;
using ClubManager.ViewObjects;
using Microsoft.EntityFrameworkCore;

namespace ClubManager.Services
{
    public class StudentService : IStudentService
    {
        private readonly ModelContext _context;

        public StudentService(ModelContext context)
        {
            _context = context;
        }
        
        //根据社团id 获取社团简介
        public string GetClubDescription(long clubId)
        {
            return _context.Clubs.Find(clubId).Description;
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
                    Type = s.Type,
                });
            if (!String.IsNullOrEmpty(query))
            {
                clubs = clubs.Where(s => s.Name.Contains(query));
            }

            return clubs;
        }

        //返回已加入社团
        public IQueryable<ClubVO> SearchInClub(long id, string query)
        {
            var clubs = from stu in _context.Students
                join joinClub in _context.JoinClub on stu.StudentId equals joinClub.StudentId
                join club in _context.Clubs on joinClub.ClubId equals club.ClubId
                where stu.StudentId == id && joinClub.Status == true
                select new ClubVO
                {
                    ClubId = club.ClubId,
                    Name = club.Name,
                    Description = club.Description,
                    EstablishmentDate = club.EstablishmentDate,
                    PresidentName =
                        (from man in _context.Managers
                            join stu in _context.Students on man.StudentId equals stu.StudentId
                            join meta in _context.StudentMeta on stu.Number equals meta.Number
                            where man.ClubId == club.ClubId && man.Term == DateTime.Now.Year
                            select meta.Name)
                        .AsNoTracking().ToString(),
                    Type = club.Type,
                };

            if (!String.IsNullOrEmpty(query))
            {
                clubs = clubs.Where(c => c.Name.Contains(query));
            }

            return clubs;
        }

        //返回学生姓名
        public string GetStudentName(long studentId)
        {
            var name = (from stu in _context.Students
                join meta in _context.StudentMeta on stu.Number equals meta.Number
                where stu.StudentId == studentId
                select meta.Name).ToString();
            return name;
        }
    }
}