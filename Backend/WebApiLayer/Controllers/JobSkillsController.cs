using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DtoLayer.JobSkillsDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApiLayer.Controllers;

[Authorize(Roles = RoleConsts.Admin)]
[Route("api/[controller]")]
public class JobSkillsController : CrudController<JobSkillDto, CreateJobSkillDto, UpdateJobSkillDto>
{
    private readonly IJobSkillService _jobSkillService;

    public JobSkillsController(IJobSkillService jobSkillService) : base(jobSkillService)
    {
        _jobSkillService = jobSkillService;
    }

    [Authorize(Roles = RoleConsts.Admin)]
    [HttpGet("admin-all")]
    public async Task<IActionResult> GetAllAdmin(CancellationToken cancellationToken)
    {
        var query = await _jobSkillService.GetAdminAllAsync(cancellationToken);
        return Ok(query);

    }
    [Authorize(Roles = RoleConsts.Admin)]
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

}
