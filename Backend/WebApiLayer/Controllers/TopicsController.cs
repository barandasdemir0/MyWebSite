using BusinessLayer.Abstract;
using DtoLayer.TopicDtos;
using Microsoft.AspNetCore.Mvc;

namespace WebApiLayer.Controllers;

[Route("api/[controller]")]
public sealed class TopicsController : CrudController<TopicDto,CreateTopicDto,UpdateTopicDto>
{
    private readonly ITopicService _topicService;

    public TopicsController(ITopicService topicService) : base(topicService)
    {
        _topicService = topicService;
    }

    [HttpGet("admin-all")]
    public async Task<IActionResult> GetAllAdmin(CancellationToken cancellationToken)
    {
        var query = await _topicService.GetAllAdminAsync(cancellationToken);
        return Ok(query);
    }

  
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
