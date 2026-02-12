using BusinessLayer.Abstract;
using DtoLayer.JobSkillCategoryDtos;
using Microsoft.AspNetCore.Mvc;

namespace WebApiLayer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JobSkillCategoriesController : ControllerBase
{
    private readonly IJobSkillCategoryService _jobSkillCategoryService;

    public JobSkillCategoriesController(IJobSkillCategoryService jobSkillCategoryService)
    {
        _jobSkillCategoryService = jobSkillCategoryService;
    }


    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = await _jobSkillCategoryService.GetAllAsync(cancellationToken);
        return Ok(query);
    }

    [HttpGet("admin-all")]
    public async Task<IActionResult> GetAllAdmin(CancellationToken cancellationToken)
    {
        var query = await _jobSkillCategoryService.GetAdminAllAsync(cancellationToken);
        return Ok(query);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = await _jobSkillCategoryService.GetByIdAsync(id, cancellationToken);
        if (query == null)
        {
            return NotFound();
        }
        return Ok(query);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateJobSkillCategoryDto createJobSkillCategoryDto, CancellationToken cancellationToken)
    {
        var query = await _jobSkillCategoryService.AddAsync(createJobSkillCategoryDto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = query.Id }, query);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateJobSkillCategoryDto updateJobSkillCategoryDto, CancellationToken cancellationToken)
    {
        var query = await _jobSkillCategoryService.UpdateAsync(id, updateJobSkillCategoryDto, cancellationToken);
        if (query == null)
        {
            return NotFound();
        }
        return Ok(query);
    }

    [HttpPut("restore/{id}")]
    public async Task<IActionResult> Restore(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _jobSkillCategoryService.RestoreAsync(id, cancellationToken);
        return Ok(entity);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _jobSkillCategoryService.GetByIdAsync(id, cancellationToken);
        if (entity == null)
        {
            return NotFound();
        }
        await _jobSkillCategoryService.DeleteAsync(id, cancellationToken);
        return Ok(entity);
    }













}
