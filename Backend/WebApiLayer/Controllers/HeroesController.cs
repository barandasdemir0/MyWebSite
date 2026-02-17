using BusinessLayer.Abstract;
using DtoLayer.HeroDtos;
using Microsoft.AspNetCore.Mvc;

namespace WebApiLayer.Controllers;


[Route("api/[controller]")]
public sealed class HeroesController : CrudController<HeroDto,CreateHeroDto,UpdateHeroDto>
{
    private readonly IHeroService _heroService;

    public HeroesController(IHeroService heroService) : base(heroService)
    {
        _heroService = heroService;
    }

    [HttpGet("single")] //upsert için işlemler
    public async Task<IActionResult> GetSingle(CancellationToken cancellationToken)
    {
        var values = await _heroService.GetSingleAsync(cancellationToken);
        if (values == null)
        {
            return NotFound();
        }
        return Ok(values);
    }


    [HttpPost("save")]
    public async Task<IActionResult> Save([FromBody] UpdateHeroDto update, CancellationToken cancellationToken)
    {
        var query = await _heroService.SaveAsync(update, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = query.Id }, query);
    }


}
