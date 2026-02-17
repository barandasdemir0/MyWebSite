using BusinessLayer.Abstract;
using DtoLayer.ExperienceDtos;
using Microsoft.AspNetCore.Mvc;

namespace WebApiLayer.Controllers;


[Route("api/[controller]")]
public sealed class ExperiencesController:CrudController<ExperienceDto,CreateExperienceDto,UpdateExperienceDto>
{
    private readonly IExperienceService _experienceService;

    public ExperiencesController(IExperienceService experienceService) : base(experienceService)
    {
        _experienceService = experienceService;
    }


    [HttpGet("admin-all")]
    public async Task<IActionResult> GetAllAdmin(CancellationToken cancellationToken)
    {
        var query = await _experienceService.GetAllAdminAsync( cancellationToken);
        return Ok(query);
    }


    [HttpPut("restore/{id}")]
    public async Task<IActionResult> Restore(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _experienceService.RestoreAsync(id, cancellationToken);
        if (entity == null)
        {
            return NotFound();
        }
        return Ok(entity);
    }


}
