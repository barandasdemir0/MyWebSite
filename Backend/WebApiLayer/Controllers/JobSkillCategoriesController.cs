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
    public async Task<IActionResult> GetAll()
    {
        var query = await _jobSkillCategoryService.GetAllAsync();
        return Ok(query);
    }

    [HttpGet("admin-all")]
    public async Task<IActionResult> GetAllAdmin()
    {
        var query = await _jobSkillCategoryService.GetAdminAllAsync();
        return Ok(query);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = await _jobSkillCategoryService.GetByIdAsync(id);
        if (query == null)
        {
            return NotFound();
        }
        return Ok(query);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateJobSkillCategoryDto createJobSkillCategoryDto)
    {
        var query = await _jobSkillCategoryService.AddAsync(createJobSkillCategoryDto);
        return CreatedAtAction(nameof(GetById), new { id = query.Id }, query);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateJobSkillCategoryDto updateJobSkillCategoryDto)
    {
        var query = await _jobSkillCategoryService.UpdateAsync(id, updateJobSkillCategoryDto);
        if (query == null)
        {
            return NotFound();
        }
        return Ok(query);
    }

    [HttpPut("restore/{id}")]
    public async Task<IActionResult> Restore(Guid id)
    {
        var entity = await _jobSkillCategoryService.RestoreAsync(id);
        return Ok(entity);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var entity = await _jobSkillCategoryService.GetByIdAsync(id);
        if (entity == null)
        {
            return NotFound();
        }
        await _jobSkillCategoryService.DeleteAsync(id);
        return Ok(entity);
    }













}
