using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using ClubManager.QueryObjects;
using ClubManager.Helpers;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ClubManager.Services
{
    public class UserService : IUserService
    {
        private readonly ModelContext _context;
        private readonly AppSettings _appSettings;

        public UserService(ModelContext context, IOptions<AppSettings> options)
        {
            _context = context;
            _appSettings = options.Value;
        }

        public AuthUser Authenticate(LoginQO log)
        {
            var username = log.Username;
            var password = log.Password;

            var user = (from u in _context.Users
                where (u.UserName == username ||
                       (u.Students.FirstOrDefault() != null && u.Students.FirstOrDefault().Number == username)) &&
                      u.Password == password
                select u).FirstOrDefault();

            if (user == null)
                return null;

            var authUser = new AuthUser {UserId = user.UserId, UserType = user.UserType};

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret));

            string[] role = {"Student", "Manager", "Admin"};

            var jwt = new JwtSecurityToken(
                claims: new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName), new Claim(ClaimTypes.Role, role[user.UserType])
                },
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(2), // 两小时后过期
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            authUser.Token = tokenHandler.WriteToken(jwt);

            return authUser;
        }


        public void Register(RegisterQO reg)
        {
            var number = reg.Number;
            var password = reg.Password;
            var username = reg.Username;
            var stu = (from s in _context.Students
                where s.Number == number
                select s).FirstOrDefault();
            if (stu == null)
            {
                throw new InvalidCastException("invalid ID");
            }

            if (stu.UserId != null)
            {
                throw new InvalidCastException("already registered");
            }

            var user = _context.Users.SingleOrDefault(u => u.UserName == username);
            if (user != null)
            {
                throw new InvalidCastException("Username already exists");
            }

            var newUser = new Users {UserName = username, Password = password, UserType = 0};
            stu.User = newUser;
            _context.Users.Add(newUser);
            _context.SaveChanges();
        }
    }
}