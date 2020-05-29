using System;
using System.Collections.Generic;
using ClubManager;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using ClubManager.QueryObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using ClubManager.Services;
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

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthUser),200)]
        [ProducesResponseType(400)]
        public IActionResult Login([FromBody] LoginQO log)
        {
            var user =_userService.Authenticate(log);
            if (user == null) return BadRequest(new {msg = "invalid name and password"});
            return Ok(user);
        }
        
        
        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult Register([FromBody] RegisterQO reg)
        {
            try
            {
                _userService.Register(reg);
            }
            catch (InvalidCastException  e)
            {
                return BadRequest(new {msg = e.Message});
            }
            return Ok(new {msg = "success"});
        }
    }
}