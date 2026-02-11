using BusinessLayer.Abstract;
using DtoLayer.JobSkillsDtos;
using Microsoft.AspNetCore.Mvc;

namespace WebApiLayer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JobSkillsController : ControllerBase
{
    private readonly IJobSkillService _jobSkillService;

    public JobSkillsController(IJobSkillService jobSkillService)
    {
        _jobSkillService = jobSkillService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = await _jobSkillService.GetAllAsync();
        return Ok(query);
    }
    [HttpGet("admin-all")]
    public async Task<IActionResult> GetAllAdmin()
    {
        var query = await _jobSkillService.GetAdminAllAsync();
        return Ok(query);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = await _jobSkillService.GetByIdAsync(id);
        if (query == null)
        {
            return NotFound();
        }
        return Ok(query);
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateJobSkillDto createJobSkillDto)
    {
        var query = await _jobSkillService.AddAsync(createJobSkillDto);
        return CreatedAtAction(nameof(GetById), new { id = query.Id }, query);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id,[FromBody] UpdateJobSkillDto updateJobSkillDto)
    {
        var query = await _jobSkillService.UpdateAsync(id, updateJobSkillDto);
        if (query == null)
        {
            return NotFound();
        }
        return Ok(query);
       
    }

    [HttpPut("restore/{id}")]
    public async Task <IActionResult> Restore(Guid id)
    {
        var query = await _jobSkillService.RestoreAsync(id);
        return Ok(query);
    }
    [HttpDelete("{id}")]
    public async Task <IActionResult> Delete(Guid id)
    {
        var query = await _jobSkillService.GetByIdAsync(id);
        if (query == null)
        {
            return NotFound();
        }
        await _jobSkillService.DeleteAsync(id);
        return Ok(query);
    }










}
