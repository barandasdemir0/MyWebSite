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
    public async Task<IActionResult> GetAll()
    {
        var query = await _projectService.GetAllAsync();
        return Ok(query);
    }


    [HttpGet("admin-all")]
    public async Task<IActionResult> GetAllAdmin([FromQuery] PaginationQuery query)
    {
        var result = await _projectService.GetAllAdminAsync(query);
        return Ok(result);
    }

    [HttpGet("slug/{slug}")]
    public async Task<IActionResult> GetDetails(string slug)
    {
        var query = await _projectService.GetBySlugAsync(slug);
        if (query == null)
        {
            return NotFound();
        }
        return Ok(query);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetDetailById(Guid id)
    {
        var query = await _projectService.GetDetailsByIdAsync(id);
        if (query == null)
        {
            return NotFound();
        }
        return Ok(query);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProjectDto createProjectDto)
    {
        var query = await _projectService.AddAsync(createProjectDto);
        return CreatedAtAction(nameof(GetDetailById), new { id = query.Id }, query);

    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProjectDto updateProjectDto)
    {
        //updateAboutDto.Id = id; --> hatamız 1 burası businessin işi idi
        var query = await _projectService.UpdateAsync(id,updateProjectDto);
        if (query == null)
        {
            return NotFound();
        }
        return Ok(query);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var query = await _projectService.GetByIdAsync(id);
        if (query == null)
        {
            return NotFound();
        }
        await _projectService.DeleteAsync(id);
        return Ok(query);
    }


    [HttpPut("restore/{id}")]
    public async Task<IActionResult> Restore(Guid id)
    {
        var entity = await _projectService.RestoreAsync(id);
        return Ok(entity);
    }



}
