using System;
using ClubManager.helpers;
using ClubManager.Helpers;
using ClubManager.QueryObjects;
using ClubManager.Services;
using ClubManager.ViewObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClubManager.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("[controller]")]
    [ApiController]
    public class AdminController:ControllerBase
    {
        private readonly IAdminService _adminService;
        
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        //获取所有社团制度并分页
        [HttpPost("getSpecifications")]
        [ProducesResponseType(typeof(PaginatedList<SpecVO>),200)]
        public ActionResult<PaginatedList<SpecVO>> GetSpec([FromBody] GetSpecQO gs)
        {
            var spec = _adminService.GetSpec(gs.Query);
            return Ok(PaginatedList<SpecVO>.Create(spec,gs.PageNumber ?? 1,gs.PageSize));
        }
        
        //提交一条新的社团制度
        [HttpPost("postOneSpecification")]
        [ProducesResponseType(typeof(SpecVO),200)]
        public IActionResult PostSpec([FromBody] PostSpecQO ps)
        {
            var userId = Utils.GetCurrentUserId(this.User);
            var spec=_adminService.PostSpec(ps,userId);
            return CreatedAtAction(nameof(GetSpecById), new {id = spec.SpecificationId}, spec);
        }
        
        //通过id获取一条社团制度
        [HttpPost("getOneSpecification/{id}")]
        [ProducesResponseType(typeof(SpecVO),200)]
        [ProducesResponseType(404)]
        public IActionResult GetSpecById(long id)
        {
            var spec = _adminService.GetSpec(id);
            if (spec == null)
            {
                return NotFound();
            }
            return Ok(spec);
        }
        
        //通过id修改一条社团制度
        [HttpPost("putOneSpecification/{id}")]
        [ProducesResponseType(204)]
        public IActionResult PutSpec([FromBody]PostSpecQO ps)
        {
            var userId = Utils.GetCurrentUserId(this.User);
            _adminService.PutSpec(ps,ps.SpecificationId,userId);
            return NoContent();
        }

        //通过id删除一条社团制度
        [HttpPost("deleteOneSpecification/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteSpec(long id)
        {
            var exist = _adminService.DeleteSpec(id);
            if (exist) return NoContent();
            else return NotFound();
        }
    }
}