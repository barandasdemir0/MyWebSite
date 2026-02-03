using BusinessLayer.Abstract;
using DtoLayer.ExperienceDtos;
using Microsoft.AspNetCore.Mvc;

namespace WebApiLayer.Controllers;


[Route("api/[controller]")]
[ApiController]
public sealed class ExperiencesController:ControllerBase
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
        //updateAboutDto.Id = id; --> hatamız 1 burası businessin işi idi
        var query = await _experienceService.UpdateAsync(id,updateExperienceDto);
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

    [HttpPut("restore/{id}")]
    public async Task<IActionResult> Restore(Guid id)
    {
        var entity = await _experienceService.RestoreAsync(id);
        return Ok(entity);
    }





}
