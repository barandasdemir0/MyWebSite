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
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = await _jobSkillService.GetAllAsync(cancellationToken);
        return Ok(query);
    }
    [HttpGet("admin-all")]
    public async Task<IActionResult> GetAllAdmin(CancellationToken cancellationToken)
    {
        var query = await _jobSkillService.GetAdminAllAsync(cancellationToken);
        return Ok(query);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = await _jobSkillService.GetByIdAsync(id, cancellationToken);
        if (query == null)
        {
            return NotFound();
        }
        return Ok(query);
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateJobSkillDto createJobSkillDto, CancellationToken cancellationToken)
    {
        var query = await _jobSkillService.AddAsync(createJobSkillDto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = query.Id }, query);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateJobSkillDto updateJobSkillDto, CancellationToken cancellationToken)
    {
        var query = await _jobSkillService.UpdateAsync(id, updateJobSkillDto, cancellationToken);
        if (query == null)
        {
            return NotFound();
        }
        return Ok(query);

    }

    [HttpPut("restore/{id}")]
    public async Task<IActionResult> Restore(Guid id, CancellationToken cancellationToken)
    {
        var query = await _jobSkillService.RestoreAsync(id, cancellationToken);
        if (query == null)
        {
            return NotFound();
        }
        return Ok(query);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var query = await _jobSkillService.GetByIdAsync(id, cancellationToken);
        if (query == null)
        {
            return NotFound();
        }
        await _jobSkillService.DeleteAsync(id, cancellationToken);
        return Ok(query);
    }










}
