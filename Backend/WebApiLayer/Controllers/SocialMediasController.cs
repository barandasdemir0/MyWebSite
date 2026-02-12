using BusinessLayer.Abstract;
using DtoLayer.HeroDtos;
using DtoLayer.SkillDtos;
using DtoLayer.SocialMediaDtos;
using Microsoft.AspNetCore.Mvc;

namespace WebApiLayer.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class SocialMediasController : ControllerBase
{
    private readonly ISocialMediaService _socialMediaService;

    public SocialMediasController(ISocialMediaService socialMediaService)
    {
        _socialMediaService = socialMediaService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = await _socialMediaService.GetAllAsync();
        return Ok(query);
    }

    [HttpGet("admin-all")]
    public async Task<IActionResult> GetAllAdminAsync(CancellationToken cancellationToken)
    {
        var query = await _socialMediaService.GetAllAdminAsync();
        return Ok(query);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = await _socialMediaService.GetByIdAsync(id);
        if (query == null)
        {
            return NotFound();
        }
        return Ok(query);
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSocialMediaDto createSocialMediaDto, CancellationToken cancellationToken)
    {
        var query = await _socialMediaService.AddAsync(createSocialMediaDto);
        return CreatedAtAction(nameof(GetById), new { id = query.Id }, query);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSocialMediaDto updateSocialMediaDto, CancellationToken cancellationToken)
    {
        //updateAboutDto.Id = id; --> hatamız 1 burası businessin işi idi
        var query = await _socialMediaService.UpdateAsync(id, updateSocialMediaDto);
        if (query == null)
        {
            return NotFound();
        }
        return Ok(query);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var query = await _socialMediaService.GetByIdAsync(id);
        if (query == null)
        {
            return NotFound();
        }
        await _socialMediaService.DeleteAsync(id);
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
