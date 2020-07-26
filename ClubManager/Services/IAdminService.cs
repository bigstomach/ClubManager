using System.Linq;
using ClubManager.QueryObjects;
using ClubManager.ViewObjects;

namespace ClubManager.Services
{
    public interface IAdminService
    {
        IQueryable<StudentVO> GetStudentInfo(string query);
        IQueryable<SpecificationVO> GetSpec(string query);
        Specifications AddSpec(SpecificationQO ps, long userId);
        SpecificationVO GetSpec(long id);
        void PutSpec(SpecificationQO ps, long id, long userId);
        bool DeleteSpec(long id);
        Students AddNewStudent(NewStudentQO student);
        IQueryable<SponsorshipVO> GetSponsorship(string query);
        SponsorshipVO GetSponsorshipDetails(long id);
        bool UpdateSuggestion(SponsorshipSuggestionQO newsuggestion, long userId);
        bool UpdateStatus(SponsorshipStatusQO newStatus, long UserId);
    }
}