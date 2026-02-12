using BusinessLayer.Abstract;
using DtoLayer.HeroDtos;
using DtoLayer.TopicDtos;
using Microsoft.AspNetCore.Mvc;

namespace WebApiLayer.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class TopicsController : ControllerBase
{
    private readonly ITopicService _topicService;

    public TopicsController(ITopicService topicService)
    {
        _topicService = topicService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = await _topicService.GetAllAsync(cancellationToken);
        return Ok(query);
    }

    [HttpGet("admin-all")]
    public async Task<IActionResult> GetAllAdmin(CancellationToken cancellationToken)
    {
        var query = await _topicService.GetAllAdminAsync(cancellationToken);
        return Ok(query);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = await _topicService.GetByIdAsync(id, cancellationToken);
        if (query == null)
        {
            return NotFound();
        }
        return Ok(query);
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTopicDto createTopicDto, CancellationToken cancellationToken)
    {
        var query = await _topicService.AddAsync(createTopicDto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = query.Id }, query);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTopicDto updateTopicDto, CancellationToken cancellationToken)
    {
        //updateAboutDto.Id = id; --> hatamız 1 burası businessin işi idi
        var query = await _topicService.UpdateAsync(id, updateTopicDto, cancellationToken);
        if (query == null)
        {
            return NotFound();
        }
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var query = await _topicService.GetByIdAsync(id, cancellationToken);
        if (query == null)
        {
            return NotFound();
        }
        await _topicService.DeleteAsync(id, cancellationToken);
        return Ok(query);
    }

}
