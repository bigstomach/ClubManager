using System;
using System.Collections.Generic;
using System.Linq;
using ClubManager.ViewObjects;
using ClubManager.QueryObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Xml;
namespace ClubManager.Services
{
    public class StudentService : IStudentService
    {
        private readonly ModelContext _context;

        public StudentService(ModelContext context)
        {
            _context = context;
        }


        //返回学生姓名
        public IQueryable<NameVO> GetStudentName(long studentId)
        {
            var name = (from stu in _context.Students
                        join meta in _context.StudentMeta on stu.Number equals meta.Number
                        where stu.StudentId == studentId
                        select new NameVO
                        {
                            Name = meta.Name,
                        }).AsNoTracking();
            return name;
        }

        //根据学生id获取全部个人信息
        public IQueryable<StudentAllVO> GetStudentInfo(long studentId)
        {
            var stu = (
                      from stu1 in _context.Students
                      join meta in _context.StudentMeta on stu1.Number equals meta.Number
                      where stu1.StudentId == studentId
                      select new StudentAllVO
                      {
                          Number = stu1.Number,
                          Name = meta.Name,
                          Grade = meta.Grade,
                          Major = meta.Major,
                          Status = meta.Status,
                          Phone = stu1.Phone,
                          Signature = stu1.Signature,
                          Mail = stu1.Mail,
                          Birthday = stu1.Birthday
                      }).AsNoTracking();
            return stu;
        }

        //根据学生id修改个人信息
        public bool ChangeStudentInfo(long studentId, StudentsQO stuQO)
        {
            var stu = _context.Students.Find(studentId);
            //var stu = _context.Students.FirstOrDefault(a =>
            //    a.StudentId == studentId);
            if (stu == null)
                return false;          //如果找不到该学生Id，返回失败
                                       
            stu.Phone = stuQO.Phone;
            stu.Signature = stuQO.Signature;
            stu.Mail = stuQO.Mail;
            stu.Birthday = stuQO.Birthday;
            _context.SaveChanges();
            return true;
        }

        //发送消息
        public bool SendMessage(MessageQO message)
        {
            var User = _context.Users.Find(message.UserId);
            if (string.IsNullOrEmpty(message.Title)) return false;//设置消息的标题不能为空
            if (User == null) return false;
            var Message = new Messages
            {
                //MessageId = _context.Messages.Select(m => m.MessageId).Max() + 1,
                UserId = message.UserId,
                Title = message.Title,
                Content = message.Content,
                Time = DateTime.Now,
                Read = false
            };
            _context.Messages.Add(Message);
            _context.SaveChanges();
            return true;
        }

        //获取已加入社团的公告并分页
        public IQueryable<InClubAnnouncementVO> GetInClubAnnouncements(long studentId)
        {
            var announce = (
                        from club in _context.Clubs
                        join ann in _context.Announcements on club.ClubId equals ann.UserId
                        join joinClub in _context.JoinClub on club.ClubId equals joinClub.ClubId
                        where joinClub.StudentId == studentId && joinClub.Status == true
                        select new InClubAnnouncementVO
                        {
                            Name = club.Name,
                            AnnouncementId = ann.AnnouncementId,
                            Title = ann.Title,
                            Content = ann.Content,
                            Time = ann.Time
                        }).AsNoTracking();
            return announce;
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
        //申请加入社团
        public bool JoinClub(long StudentId, JoinClub1QO newJoinClubQO)
        {
            var test = _context.JoinClub.FirstOrDefault(a =>
                a.ClubId == newJoinClubQO.ClubId && a.StudentId == StudentId);
            if (test != null) return false;                 //检测是否已存在该申请，如果已经存在该就添加失败
            var newJoin = new JoinClub                      //插入新申请，Status初始值为0，待审核
            {
                ClubId = newJoinClubQO.ClubId,
                StudentId = StudentId,
                ApplyDate = DateTime.Now,
                ApplyReason = newJoinClubQO.ApplyReason,
                Status = false
            };
            _context.JoinClub.Add(newJoin);
            _context.SaveChanges();
            return true;
        }

        //返回已加入社团
        public IQueryable<ClubVO> SearchInClub(long id, string query)
        {
            var Pname = from man in _context.Managers
                        join stu in _context.Students on man.StudentId equals stu.StudentId
                        join meta in _context.StudentMeta on stu.Number equals meta.Number
                        where man.Term == DateTime.Now.Year
                        select new PresidentVO
                        {
                            ClubId = man.ClubId,
                            Name = meta.Name,
                        };
            var clubs = from stu in _context.Students
                        join joinClub in _context.JoinClub on stu.StudentId equals joinClub.StudentId
                        join club in _context.Clubs on joinClub.ClubId equals club.ClubId
                        join pname in Pname on club.ClubId equals pname.ClubId
                        where stu.StudentId == id && joinClub.Status == true
                        select new ClubVO
                        {
                            ClubId = club.ClubId,
                            Name = club.Name,
                            Description = club.Description,
                            EstablishmentDate = club.EstablishmentDate,
                            PresidentName =pname.Name,
                            Type = club.Type,
                        };
            if (!String.IsNullOrEmpty(query))
            {
                clubs = clubs.Where(c => c.Name.Contains(query));
            }

            return clubs;
        }
        //退出某一社团
        public bool QuitOneClub(long studentId, long id)
        {
            var join= _context.JoinClub.FirstOrDefault(jc =>
                    jc.StudentId == studentId && jc.ClubId == id && jc.Status == true);
            if (join == null) return false;
            _context.JoinClub.Remove(join);
            _context.SaveChanges();
            return true;
        }
        //获取已加入社团内部活动信息
        public IQueryable<ActivityVO> GetInActivitiesInfo(long studentId,string query)
        {
            DateTime time = DateTime.Now;
            var acts = from stu in _context.Students
                       join joinClub in _context.JoinClub on stu.StudentId equals joinClub.StudentId
                       join act in _context.Activities on joinClub.ClubId equals act.ClubId
                       join club in _context.Clubs on act.ClubId equals club.ClubId
                       where stu.StudentId == studentId && joinClub.Status == true&& act.Status==1 &&DateTime.Compare(time,act.EventTime)<0
                       //确保是已加社团的活动，保证活动审核通过且活动举办日期未过
                       select new ActivityVO
                 {
                     ActivityId = act.ActivityId,
                     Name = act.Name,
                     Description = act.Description,
                     Place = act.Place,
                     EventTime=act.EventTime,
                     ClubName=club.Name
                 };
            if (!String.IsNullOrEmpty(query))
            {
                acts = acts.Where(s => s.Name.Contains(query));
            }
            return acts;
        }

        //获取全部公开活动信息
        public IQueryable<ActivityVO> GetOutActivitiesInfo(string query)
        {
            DateTime time = DateTime.Now;
            var acts = from act in _context.Activities
                       join club in _context.Clubs on act.ClubId equals club.ClubId
                       where act.IsPublic == true && act.Status == 1 && DateTime.Compare(time, act.EventTime) < 0
                       select new ActivityVO
                       {
                           ActivityId = act.ActivityId,
                           Name = act.Name,
                           Description = act.Description,
                           Place = act.Place,
                           EventTime = act.EventTime,
                           ClubName = club.Name
                       };
            if (!String.IsNullOrEmpty(query))
            {
                acts = acts.Where(s => s.Name.Contains(query));
            }
            return acts;
        }
        //返回已参加活动
        public IQueryable<ActivityVO> SearchInActivity(long id, string query)
        {
            var acts = from stu in _context.Students
                       join joinAct in _context.ParticipateActivity on stu.StudentId equals joinAct.StudentId
                       join Act in _context.Activities on joinAct.ActivityId equals Act.ActivityId
                       join club in _context.Clubs on Act.ClubId equals club.ClubId
                       where stu.StudentId == id && joinAct.Status == true
                       select new ActivityVO
                       {
                           ActivityId = Act.ActivityId,
                           Name = Act.Name,
                           Description = Act.Description,
                           Place = Act.Place,
                           Suggestion = Act.Suggestion,
                           ClubName = club.Name
                       };
            if (!String.IsNullOrEmpty(query))
            {
                acts = acts.Where(c => c.Name.Contains(query));
            }

            return acts;
        }
        //申请参加活动
        public bool ParticipateActivity(long StudentId, ParticipateActivityQO newParticipateActivityQO)
        {
            var test = _context.ParticipateActivity.FirstOrDefault(a =>
                a.ActivityId == newParticipateActivityQO.ActivityId && a.StudentId == StudentId);
            if (test != null) return false;//检测是否已存在该申请，如果已经存在该就添加失败
            var newParticipate = new ParticipateActivity//插入新申请，Status初始值为0，待审核
            {
                ActivityId = newParticipateActivityQO.ActivityId,
                StudentId = StudentId,
                ApplyDate = DateTime.Now,
                ApplyReason = newParticipateActivityQO.ApplyReason,
                Status = false
            };
            _context.ParticipateActivity.Add(newParticipate);
            _context.SaveChanges();
            return true;
        }
    }
}
