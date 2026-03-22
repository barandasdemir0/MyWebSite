using BusinessLayer.Abstract;
using DtoLayer.AuthDtos;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApiLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize (Roles =RoleConsts.Admin)]
    public class UserAdminController : ControllerBase
    {
        private readonly IUserAdminService _userAdminService;

        public UserAdminController(IUserAdminService userAdminService)
        {
            _userAdminService = userAdminService;
        }


        [HttpPost("assign-role")]
        [Authorize(Roles = RoleConsts.Admin)]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleDto assignRoleDto, CancellationToken cancellationToken)
        {
            var ok = await _userAdminService.AssignRoleAsync(assignRoleDto.UserId, assignRoleDto.Role, cancellationToken);
            if (ok)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Rol atanamadı");
            }
        }

        [HttpGet("pending-users")]
        [Authorize(Roles = RoleConsts.Admin)]
        public async Task<IActionResult> GetPendingUsers(CancellationToken cancellationToken)
        {
            var users = await _userAdminService.GetPendingUsersAsync(cancellationToken);
            return Ok(users);
        }

        [HttpPost("approve-user/{userId}")]
        [Authorize(Roles = RoleConsts.Admin)]
        public async Task<IActionResult> ApproveUser(string userId, [FromBody] string role, CancellationToken cancellationToken)
        {
            var ok = await _userAdminService.ApproveUserAsync(userId, role, cancellationToken);
            if (ok)
            {
                return Ok();
            }
            return BadRequest("Rol onaylanamadı");
        }

        [HttpPost("reject-user/{userId}")]
        [Authorize(Roles = RoleConsts.Admin)]
        public async Task<IActionResult> RejectUser(string userId, CancellationToken cancellationToken)
        {
            var ok = await _userAdminService.RejectUserAsync(userId, cancellationToken);
            if (ok)
            {
                return Ok();
            }
            return BadRequest("Kullanıcı Silinemedi");
        }

        [HttpGet("all-users")]
        [Authorize(Roles = RoleConsts.Admin)]
        public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken)
        {
            var users = await _userAdminService.GetAllUserAsync(cancellationToken);
            return Ok(users);
        }
    }
}
