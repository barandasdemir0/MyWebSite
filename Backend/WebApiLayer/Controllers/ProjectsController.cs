using BusinessLayer.Abstract;
using DtoLayer.ProjectDtos;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Shared;

namespace WebApiLayer.Controllers;

[Route("api/[controller]")]

public sealed class ProjectsController : CrudController<ProjectDto,CreateProjectDto,UpdateProjectDto>
{
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService) : base(projectService)
    {
        _projectService = projectService;
    }

    
    [HttpGet("admin-all")]
    public async Task<IActionResult> GetAllAdmin([FromQuery] PaginationQuery query, CancellationToken cancellationToken)
    {
        var result = await _projectService.GetAllAdminAsync(query, cancellationToken);
        return Ok(result);
    }
    [HttpGet("user-all")]
    public async Task<IActionResult> GetAllUser([FromQuery] PaginationQuery query, CancellationToken cancellationToken)
    {
        var result = await _projectService.GetAllUserAsync(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("slug/{slug}")]
    public async Task<IActionResult> GetDetails(string slug, CancellationToken cancellationToken)
    {
        var query = await _projectService.GetBySlugAsync(slug, cancellationToken);
        if (query == null)
        {
            return NotFound();
        }
        return Ok(query);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetDetailById(Guid id, CancellationToken cancellationToken)
    {
        var query = await _projectService.GetDetailsByIdAsync(id, cancellationToken);
        if (query == null)
        {
            return NotFound();
        }
        return Ok(query);
    }

    [HttpPut("restore/{id}")]
    public async Task<IActionResult> Restore(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _projectService.RestoreAsync(id, cancellationToken);
        if (entity == null)
        {
            return NotFound();
        }
        return Ok(entity);
    }

    [HttpGet("latest/{count}")]
    public async Task<IActionResult> GetLatest(int count,CancellationToken cancellationToken)
    {
        var values = await _projectService.GetLatestAsync(count,cancellationToken);
        if (values == null)
        {
            return NotFound();
        }
        return Ok(values);
    }

}
