
using BusinessLayer.Abstract;
using DtoLayer.EducationDto;
using Microsoft.AspNetCore.Mvc;

namespace WebApiLayer.Controllers;


[Route("api/[controller]")]
[ApiController]
public class EducationsController : ControllerBase
{
    private readonly IEducationService _educationService;

    public EducationsController(IEducationService educationService)
    {
        _educationService = educationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = await _educationService.GetAllAsync();
        return Ok(query);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var values = await _educationService.GetByIdAsync(id);
        if (values == null)
        {
            return NotFound();
        }
        return Ok(values);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEducationDto createEducationDto)
    {
        var values = await _educationService.AddAsync(createEducationDto);
        return CreatedAtAction(nameof(GetById), new { id = values.Id }, values);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEducationDto updateEducationDto)
    {
        updateEducationDto.Id = id;
        var values = await _educationService.UpdateAsync(updateEducationDto);
        if (values==null)
        {
            return NotFound(); ;
        }
        return Ok(values);

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var query = await _educationService.GetByIdAsync(id);
        if (query == null)
        {
            return NotFound();
        }
        await _educationService.DeleteAsync(id);
        return Ok(query);
    }




}
