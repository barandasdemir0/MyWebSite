using BusinessLayer.Abstract;
using DtoLayer.SiteSettingDtos;
using Microsoft.AspNetCore.Mvc;

namespace WebApiLayer.Controllers;


[Route("api/[controller]")]

public class SiteSettingsController:CrudController<SiteSettingDto,CreateSiteSettingDto,UpdateSiteSettingDto>
{
    private readonly ISiteSettingsService _siteSettingsService;

    public SiteSettingsController(ISiteSettingsService siteSettingsService) : base(siteSettingsService)
    {
        _siteSettingsService = siteSettingsService;
    }

    [HttpGet("single")] //upsert için işlemler
    public async Task<IActionResult> GetSingle(CancellationToken cancellationToken)
    {
        var values = await _siteSettingsService.GetSingleAsync(cancellationToken);
        if (values == null)
        {
            return NotFound();
        }
        return Ok(values);
    }

    [HttpPost("save")]
    public async Task<IActionResult> Save([FromBody] UpdateSiteSettingDto update, CancellationToken cancellationToken)
    {
        var query = await _siteSettingsService.SaveAsync(update, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = query.Id }, query);
    }
}
