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
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = await _experienceService.GetAllAsync(cancellationToken);
        return Ok(query);
    }

    [HttpGet("admin-all")]
    public async Task<IActionResult> GetAllAdmin(CancellationToken cancellationToken)
    {
        var query = await _experienceService.GetAllAdminAsync( cancellationToken);
        return Ok(query);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = await _experienceService.GetByIdAsync(id, cancellationToken);
        if (query==null)
        {
            return NotFound();
        }
        return Ok(query);

    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateExperienceDto createExperienceDto, CancellationToken cancellationToken)
    {
        var values = await _experienceService.AddAsync(createExperienceDto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = values.Id }, values);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update (Guid id, [FromBody] UpdateExperienceDto updateExperienceDto, CancellationToken cancellationToken)
    {
        //updateAboutDto.Id = id; --> hatamız 1 burası businessin işi idi
        var query = await _experienceService.UpdateAsync(id,updateExperienceDto, cancellationToken);
        if (query == null)
        {
            return NotFound();
        }
        return Ok(query);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var query = await _experienceService.GetByIdAsync(id, cancellationToken);
        if (query == null)
        {
            return NotFound();
        }
        await _experienceService.DeleteAsync(id, cancellationToken);
        return Ok(query);
    }

    [HttpPut("restore/{id}")]
    public async Task<IActionResult> Restore(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _experienceService.RestoreAsync(id, cancellationToken);
        if (entity == null)
        {
            return NotFound();
        }
        return Ok(entity);
    }





}
