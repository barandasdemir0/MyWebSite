using BusinessLayer.Abstract;
using DtoLayer.TopicDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApiLayer.Controllers;

[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public sealed class TopicsController : CrudController<TopicDto,CreateTopicDto,UpdateTopicDto>
{
    private readonly ITopicService _topicService;

    public TopicsController(ITopicService topicService) : base(topicService)
    {
        _topicService = topicService;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin-all")]
    public async Task<IActionResult> GetAllAdmin(CancellationToken cancellationToken)
    {
        var query = await _topicService.GetAllAdminAsync(cancellationToken);
        return Ok(query);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("restore/{id}")]
    public async Task<IActionResult> Restore(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _topicService.RestoreAsync(id, cancellationToken);
        if (entity == null)
        {
            return NotFound();
        }
        return Ok(entity);
    }


}
