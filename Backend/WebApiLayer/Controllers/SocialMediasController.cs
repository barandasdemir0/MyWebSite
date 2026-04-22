using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DtoLayer.SocialMediaDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApiLayer.Controllers;

[Route("api/[controller]")]
public sealed class SocialMediasController : CrudController<SocialMediaDto,CreateSocialMediaDto,UpdateSocialMediaDto>
{
    private readonly ISocialMediaService _socialMediaService;

    public SocialMediasController(ISocialMediaService socialMediaService) : base(socialMediaService)
    {
        _socialMediaService = socialMediaService;
    }


    [HttpGet("admin-all")]
    [Authorize(Roles = RoleConsts.Admin)]
    public async Task<IActionResult> GetAllAdminAsync(CancellationToken cancellationToken)
    {
        var query = await _socialMediaService.GetAllAdminAsync();
        return Ok(query);
    }

    [Authorize(Roles = RoleConsts.Admin)]
    [HttpPut("restore/{id}")]
    public async Task<IActionResult> Restore(Guid id, CancellationToken cancellationToken)
    {
        var query = await _socialMediaService.RestoreAsync(id);
        if (query == null)
        {
            return NotFound();
        }
        return Ok(query);
    }


}
