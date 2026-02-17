
using BusinessLayer.Abstract;
using DtoLayer.EducationDtos;
using Microsoft.AspNetCore.Mvc;

namespace WebApiLayer.Controllers;


[Route("api/[controller]")]
public sealed class EducationsController : CrudController<EducationDto,CreateEducationDto,UpdateEducationDto>
{
    private readonly IEducationService _educationService;

    public EducationsController(IEducationService educationService) : base(educationService)
    {
        _educationService = educationService;
    }


    [HttpGet("admin-all")]
    public async Task<IActionResult> GetAllAdmin(CancellationToken cancellationToken)
    {
        var query = await _educationService.GetAllAdminAsync( cancellationToken);
        return Ok(query);
    }

    [HttpPut("restore/{id}")]
    public async Task<IActionResult> Restore(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _educationService.RestoreAsync(id, cancellationToken);
        if (entity == null)
        {
            return NotFound();
        }
        return Ok(entity);
    }


}
