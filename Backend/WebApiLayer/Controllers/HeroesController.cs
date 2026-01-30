using BusinessLayer.Abstract;
using DtoLayer.GithubRepoDto;
using DtoLayer.HeroDto;
using Microsoft.AspNetCore.Mvc;

namespace WebApiLayer.Controllers;


[Route("api/[controller]")]
[ApiController]
public sealed class HeroesController:ControllerBase
{
    private readonly IHeroService _heroService;

    public HeroesController(IHeroService heroService)
    {
        _heroService = heroService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = await _heroService.GetAllAsync();
        return Ok(query);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = await _heroService.GetByIdAsync(id);
        if (query == null)
        {
            return NotFound();
        }
        return Ok(query);
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateHeroDto createHeroDto)
    {
        var query = await _heroService.AddAsync(createHeroDto);
        return CreatedAtAction(nameof(GetById), new { id = query.Id }, query);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateHeroDto updateHeroDto)
    {
        //updateAboutDto.Id = id; --> hatamız 1 burası businessin işi idi
        var query = await _heroService.UpdateAsync(id,updateHeroDto);
        if (query == null)
        {
            return NotFound();
        }
        return Ok(query);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var query = await _heroService.GetByIdAsync(id);
        if (query == null)
        {
            return NotFound();
        }
        await _heroService.DeleteAsync(id);
        return Ok(query);
    }

}
