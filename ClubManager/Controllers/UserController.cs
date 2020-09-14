using System;
using System.Collections.Generic;
using ClubManager;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using ClubManager.helpers;
using ClubManager.Helpers;
using ClubManager.QueryObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using ClubManager.Services;
using ClubManager.ViewObjects;
using Microsoft.AspNetCore.Authorization;

namespace ClubManager.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;


        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        //登录
        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthUser), 200)]
        [ProducesResponseType(404)]
        public IActionResult Login([FromBody] LoginQO log)
        {
            var user = _userService.Authenticate(log);
            if (user == null) return NotFound(new {msg = "用户名或密码错误"});
            return Ok(user);
        }

        //注册
        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult Register([FromBody] RegisterQO reg)
        {
            try
            {
                _userService.Register(reg);
            }
            catch (InvalidCastException e)
            {
                return NotFound(new {msg = e.Message});
            }

            return Ok(new {msg = "注册成功"});
        }

        //修改密码
        [HttpPost("changePassword")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult ChangePassword([FromBody] PasswordQO pas)
        {
            var userId = Utils.GetCurrentUserId(User);
            var success = _userService.ChangePassword(userId,pas);
            if (success) return Ok(new {msg="修改密码成功"});
            return NotFound(new {msg = "原密码错误"});
        }

        //获取系统消息
        [HttpPost("getMessages")]
        [ProducesResponseType(typeof(PaginatedList<MessageVO>),200)]
        public IActionResult GetMessages([FromBody] PageQO pq)
        {
            var userId = Utils.GetCurrentUserId(User);
            var messages = _userService.GetMessages(userId);
            return Ok(PaginatedList<MessageVO>.Create(messages, pq.PageNumber ?? 1, pq.PageSize));
        }

        //根据id获取一条系统消息
        [HttpPost("getOneMessage/{id}")]
        [ProducesResponseType(typeof(MessageVO), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetOneMessage(long id)
        {
            var userId = Utils.GetCurrentUserId(User);
            var message = _userService.GetOneMessage(userId, id);
            if (message == null) return NotFound();
            return Ok(message);
        }

        //将消息设为已读
        [HttpPost("setMessageRead/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult SetMessageRead(long id)
        {
            var userId = Utils.GetCurrentUserId(User);
            var success = _userService.SetMessageRead(userId, id);
            if (success) return Ok();
            return NotFound();
        }

        //获取系统公告
        [HttpPost("getSysAnnouncements")]
        [ProducesResponseType(typeof(PaginatedList<AnnouncementVO>),200)]
        public IActionResult GetSysAnnouncements([FromBody] PageQO pq)
        {
            var announces = _userService.GetAnnouncements();
            return Ok(PaginatedList<AnnouncementVO>.Create(announces, pq.PageNumber ?? 1, pq.PageSize));
        }

        //根据id获取一条系统公告
        [HttpPost("getOneAnnouncement/{id}")]
        [ProducesResponseType(typeof(AnnouncementVO), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetOneAnnouncement(long id)
        {
            var announce = _userService.GetOneAnnouncement(id);
            if (announce == null) return NotFound();
            return Ok(announce);
        }

       
    }
}
