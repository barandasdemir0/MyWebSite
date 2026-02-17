using BusinessLayer.Abstract;
using DtoLayer.SocialMediaDtos;
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
    public async Task<IActionResult> GetAllAdminAsync(CancellationToken cancellationToken)
    {
        var query = await _socialMediaService.GetAllAdminAsync();
        return Ok(query);
    }

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
