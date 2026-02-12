using BusinessLayer.Abstract;
using DtoLayer.ProjectDtos;
using DtoLayer.Shared;
using Microsoft.AspNetCore.Mvc;

namespace WebApiLayer.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = await _projectService.GetAllAsync(cancellationToken);
        return Ok(query);
    }


    [HttpGet("admin-all")]
    public async Task<IActionResult> GetAllAdmin([FromQuery] PaginationQuery query, CancellationToken cancellationToken)
    {
        var result = await _projectService.GetAllAdminAsync(query, cancellationToken);
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

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProjectDto createProjectDto, CancellationToken cancellationToken)
    {
        var query = await _projectService.AddAsync(createProjectDto, cancellationToken);
        return CreatedAtAction(nameof(GetDetailById), new { id = query.Id }, query);

    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProjectDto updateProjectDto, CancellationToken cancellationToken)
    {
        //updateAboutDto.Id = id; --> hatamız 1 burası businessin işi idi
        var query = await _projectService.UpdateAsync(id, updateProjectDto, cancellationToken);
        if (query == null)
        {
            return NotFound();
        }
        return Ok(query);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var query = await _projectService.GetByIdAsync(id, cancellationToken);
        if (query == null)
        {
            return NotFound();
        }
        await _projectService.DeleteAsync(id, cancellationToken);
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



}
