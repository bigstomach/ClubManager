using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using ClubManager.QueryObjects;
using ClubManager.Helpers;
using ClubManager.ViewObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
                where (u.UserName == username || u.Students.Number.ToString() == username) && u.Password == password
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
            var stu = _context.StudentMeta.FirstOrDefault(s => s.Number == number);
            if (stu == null)
            {
                throw new InvalidCastException("学号不存在");
            }

            var studentId = _context.Students.FirstOrDefault(s => s.Number == number);

            if (studentId != null)
            {
                throw new InvalidCastException("此学号已注册");
            }

            var name = _context.Users.FirstOrDefault(u => u.UserName == username);
            if (name != null)
            {
                throw new InvalidCastException("此用户名已存在");
            }

            var newUser = new Users {UserName = username, Password = password, UserType = 0};
            _context.Users.Add(newUser);
            _context.SaveChanges();

            var newStudent = new Students
                {StudentId = _context.Users.First(u => u.UserName == username).UserId, Number = number};
            _context.Students.Add(newStudent);
            _context.SaveChanges();
        }


        public bool ChangePassword(long userId, PasswordQO pas)
        {
            var pre = pas.PrePassword;
            var user = _context.Users.Find(userId);
            if (pre != user.Password) return false;
            user.Password = pas.NewPassword;
            _context.SaveChanges();
            return true;
        }

        public IQueryable<MessageVO> GetMessages(long userId)
        {
            var messages = _context.Messages
                .Where(m => m.UserId == userId)
                .OrderByDescending(m => m.Time)
                .Select(m => new MessageVO
                {
                    MessageId = m.MessageId, Title = m.Title, Content = m.Content, Time = m.Time, Read = m.Read
                }).AsNoTracking();

            return messages;

        }

        public MessageVO GetOneMessage(long userId, long id)
        {
            var message = _context.Messages
                .Where(m => m.UserId == userId && m.MessageId == id)
                .Select(m => new MessageVO
                {
                    MessageId = m.MessageId, Title = m.Title, Content = m.Content, Time = m.Time,Read = m.Read
                }).AsNoTracking()
                .FirstOrDefault();
            return message;
        }

        public bool SetMessageRead(long userId, long id)
        {
            var message = _context.Messages
                .FirstOrDefault(m => m.UserId == userId && m.MessageId == id);
            if (message == null) return false;
            message.Read = true;
            _context.SaveChanges();
            return true;
        }

        public IQueryable<AnnouncementVO> GetAnnouncements()
        {
            var announce = (from admin in _context.Administrators
                join ann in _context.Announcements on admin.AdminId equals ann.UserId
                orderby ann.Time descending
                select new AnnouncementVO
                {
                    AnnouncementId = ann.AnnouncementId, Title = ann.Title, Content = ann.Content, Time = ann.Time
                }).AsNoTracking();
            return announce;
        }

        public AnnouncementVO GetOneAnnouncement(long id)
        {
            var ann = _context.Announcements.FirstOrDefault(a=>a.AnnouncementId==id);
            if (ann==null) return null;
            var userType = _context.Users.Find(ann.UserId).UserType;
            if (userType != 2) return null;
            var announce=new AnnouncementVO{
                AnnouncementId = id, Title = ann.Title,Content = ann.Content, Time = ann.Time
            };
            return announce;
        }
    }
}