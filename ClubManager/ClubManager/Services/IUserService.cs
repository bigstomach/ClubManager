using ClubManager.QueryObjects;

namespace ClubManager.Services
{
    public interface IUserService
    {
        AuthUser Authenticate(LoginQO log);
        void Register(RegisterQO reg);
    }
}