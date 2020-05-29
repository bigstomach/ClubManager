using System.Linq;

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
        
    }
}