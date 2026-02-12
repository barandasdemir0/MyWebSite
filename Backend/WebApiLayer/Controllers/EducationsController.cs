
using BusinessLayer.Abstract;
using DtoLayer.EducationDtos;
using Microsoft.AspNetCore.Mvc;

namespace WebApiLayer.Controllers;


[Route("api/[controller]")]
[ApiController]
public sealed class EducationsController : ControllerBase
{
    private readonly IEducationService _educationService;

    public EducationsController(IEducationService educationService)
    {
        _educationService = educationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = await _educationService.GetAllAsync( cancellationToken);
        return Ok(query);
    }

    [HttpGet("admin-all")]
    public async Task<IActionResult> GetAllAdmin(CancellationToken cancellationToken)
    {
        var query = await _educationService.GetAllAdminAsync( cancellationToken);
        return Ok(query);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var values = await _educationService.GetByIdAsync(id, cancellationToken);
        if (values == null)
        {
            return NotFound();
        }
        return Ok(values);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEducationDto createEducationDto, CancellationToken cancellationToken)
    {
        var values = await _educationService.AddAsync(createEducationDto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = values.Id }, values);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEducationDto updateEducationDto, CancellationToken cancellationToken)
    {
        //updateAboutDto.Id = id; --> hatamız 1 burası businessin işi idi
        var values = await _educationService.UpdateAsync(id,updateEducationDto, cancellationToken);
        if (values==null)
        {
            return NotFound(); ;
        }
        return Ok(values);

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var query = await _educationService.GetByIdAsync(id, cancellationToken);
        if (query == null)
        {
            return NotFound();
        }
        await _educationService.DeleteAsync(id, cancellationToken);
        return Ok(query);
    }

    [HttpPut("restore/{id}")]
    public async Task<IActionResult> Restore(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _educationService.RestoreAsync(id, cancellationToken);
        if (entity == null)
        {
            return NotFound();
        }
        return Ok(entity);
    }


}
