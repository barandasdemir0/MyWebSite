using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DtoLayer.AboutDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApiLayer.Controllers;

[Authorize(Roles = RoleConsts.Admin)]
[Route("api/[controller]")]
public sealed class AboutsController : CrudController<AboutDto, CreateAboutDto, UpdateAboutDto>
{
    private readonly IAboutService _aboutService;

    public AboutsController(IAboutService aboutService) : base(aboutService)
    {
        _aboutService = aboutService;
    }

    [AllowAnonymous]
    [HttpGet("single")] //upsert için işlemler
    public async Task<IActionResult> GetSingle(CancellationToken cancellationToken)
    {
        var values = await _aboutService.GetSingleAsync(cancellationToken);
        if (values == null)
        {
            return NotFound();
        }
        return Ok(values);
    }
    [Authorize(Roles = RoleConsts.Admin)]
    [HttpPost("save")]
    public async Task<IActionResult> Save([FromBody] UpdateAboutDto updateAboutDto, CancellationToken cancellation)
    {
        var result = await _aboutService.SaveAsync(updateAboutDto, cancellation);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

}
