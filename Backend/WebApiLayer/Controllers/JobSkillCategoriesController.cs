using BusinessLayer.Abstract;
using DtoLayer.JobSkillCategoryDtos;
using Microsoft.AspNetCore.Mvc;

namespace WebApiLayer.Controllers;

[Route("api/[controller]")]
public class JobSkillCategoriesController : CrudController<JobSkillCategoryDto,CreateJobSkillCategoryDto,UpdateJobSkillCategoryDto>
{
    private readonly IJobSkillCategoryService _jobSkillCategoryService;

    public JobSkillCategoriesController(IJobSkillCategoryService jobSkillCategoryService) : base(jobSkillCategoryService)
    {
        _jobSkillCategoryService = jobSkillCategoryService;
    }


    [HttpGet("admin-all")]
    public async Task<IActionResult> GetAllAdmin(CancellationToken cancellationToken)
    {
        var query = await _jobSkillCategoryService.GetAdminAllAsync(cancellationToken);
        return Ok(query);
    }

    

    [HttpPut("restore/{id}")]
    public async Task<IActionResult> Restore(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _jobSkillCategoryService.RestoreAsync(id, cancellationToken);
        if (entity == null)
        {
            return NotFound();
        }
        return Ok(entity);
    }

}
