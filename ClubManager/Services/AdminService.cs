using System;
using System.Linq;
using ClubManager.QueryObjects;
using ClubManager.ViewObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Xml;

namespace ClubManager.Services
{
    public class AdminService: IAdminService
    {
        private readonly ModelContext _context;
        
        public AdminService(ModelContext context)
        {
            _context = context;
        }

        public SpecVO GetSpec(long id)
        {
            var spec = _context.Specifications
                .Select(s => new SpecVO
                {
                    SpecificationId = s.SpecificationId,
                    AdminName = s.User.Name,
                    Name = s.Name,
                    Content = s.Content,
                    Date = s.Date
                }).FirstOrDefault(s => s.SpecificationId == id);
            return spec;
        }
        
        public IQueryable<SpecVO> GetSpec(string query)
        {
            var spec = _context.Specifications
                .Select(s=>new SpecVO
                {
                    SpecificationId = s.SpecificationId,
                    AdminName =s.User.Name,
                    Name = s.Name,
                    Content = s.Content,
                    Date = s.Date
                })
                .AsNoTracking();
            if (!String.IsNullOrEmpty(query))
            {
                spec = spec.Where(c => c.Name.Contains(query));
            }
            return spec;
        }
        
        public Specifications PostSpec(PostSpecQO ps,long userId)
        {
            var newSpec=new Specifications{Content = ps.Content,Date = DateTime.Now,Name =ps.Name,UserId = userId};
            _context.Specifications.Add(newSpec);
            _context.SaveChanges();
            return newSpec;
        }

        public void PutSpec(PostSpecQO ps,long id,long userId)
        {
            var newSpec=new Specifications{SpecificationId = id,Content = ps.Content,Date = DateTime.Now,Name =ps.Name,UserId = userId};
            _context.Entry(newSpec).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public bool DeleteSpec(long id)
        {
            var s = _context.Specifications.Find(id);
            if (s == null) return false;
            _context.Specifications.Remove(s);
            _context.SaveChanges();
            return true;
        }

    }
}