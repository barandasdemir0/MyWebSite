using BusinessLayer.Abstract;
using DtoLayer.HeroDto;
using DtoLayer.SkillDto;
using Microsoft.AspNetCore.Mvc;

namespace WebApiLayer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SkillsController:ControllerBase
{
    private readonly ISkillService _skillService;

    public SkillsController(ISkillService skillService)
    {
        _skillService = skillService;
    }

   
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = await _skillService.GetAllAsync();
        return Ok(query);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = await _skillService.GetByIdAsync(id);
        if (query == null)
        {
            return NotFound();
        }
        return Ok(query);
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSkillDto createSkillDto)
    {
        var query = await _skillService.AddAsync(createSkillDto);
        return CreatedAtAction(nameof(GetById), new { id = query.Id }, query);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSkillDto updateSkillDto)
    {
        updateSkillDto.Id = id;
        var query = await _skillService.UpdateAsync(updateSkillDto);
        if (query == null)
        {
            return NotFound();
        }
        return Ok(query);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var query = await _skillService.GetByIdAsync(id);
        if (query == null)
        {
            return NotFound();
        }
        await _skillService.DeleteAsync(id);
        return Ok(query);
    }

}
