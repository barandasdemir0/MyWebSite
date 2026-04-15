using BusinessLayer.Abstract;
using DtoLayer.SkillDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApiLayer.Controllers;

[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public sealed class SkillsController : CrudController<SkillDto,CreateSkillDto,UpdateSkillDto>
{
    private readonly ISkillService _skillService;

    public SkillsController(ISkillService skillService) : base(skillService)
    {
        _skillService = skillService;
    }


    [Authorize(Roles = "Admin")]
    [HttpPut("restore/{id}")]
    public async Task<IActionResult> Restore(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _skillService.RestoreAsync(id, cancellationToken);
        if (entity == null)
        {
            return NotFound();
        }
        return Ok(entity);
    }


}
