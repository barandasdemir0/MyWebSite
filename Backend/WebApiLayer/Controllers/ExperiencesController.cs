using BusinessLayer.Abstract;
using DtoLayer.ExperienceDto;
using Microsoft.AspNetCore.Mvc;

namespace WebApiLayer.Controllers;


[Route("api/[controller]")]
[ApiController]
public class ExperiencesController:ControllerBase
{
    private readonly IExperienceService _experienceService;

    public ExperiencesController(IExperienceService experienceService)
    {
        _experienceService = experienceService;
    }



    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = await _experienceService.GetAllAsync();
        return Ok(query);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = await _experienceService.GetByIdAsync(id);
        if (query==null)
        {
            return NotFound();
        }
        return Ok(query);

    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateExperienceDto createExperienceDto)
    {
        var values = await _experienceService.AddAsync(createExperienceDto);
        return CreatedAtAction(nameof(GetById), new { id = values.Id }, values);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update (Guid id, [FromBody] UpdateExperienceDto updateExperienceDto)
    {
        updateExperienceDto.Id = id;
        var query = await _experienceService.UpdateAsync(updateExperienceDto);
        if (query == null)
        {
            return NotFound();
        }
        return Ok(query);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var query = await _experienceService.GetByIdAsync(id);
        if (query == null)
        {
            return NotFound();
        }
        await _experienceService.DeleteAsync(id);
        return Ok(query);
    }







}
