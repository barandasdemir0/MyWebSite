using BusinessLayer.Abstract;
using DtoLayer.HeroDto;
using DtoLayer.TopicDto;
using Microsoft.AspNetCore.Mvc;

namespace WebApiLayer.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class TopicsController:ControllerBase
{
    private readonly ITopicService _topicService;

    public TopicsController(ITopicService topicService)
    {
        _topicService = topicService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = await _topicService.GetAllAsync();
        return Ok(query);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = await _topicService.GetByIdAsync(id);
        if (query == null)
        {
            return NotFound();
        }
        return Ok(query);
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTopicDto createTopicDto)
    {
        var query = await _topicService.AddAsync(createTopicDto);
        return CreatedAtAction(nameof(GetById), new { id = query.Id }, query);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTopicDto updateTopicDto)
    {
        updateTopicDto.Id = id;
        var query = await _topicService.UpdateAsync(updateTopicDto);
        if (query == null)
        {
            return NotFound();
        }
        return Ok(query);
    }

    [HttpPut("restore/{id}")]
    public async Task<IActionResult> Restore(Guid id)
    {
        var entity = await _topicService.RestoreAsync(id);
        return Ok(entity);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var query = await _topicService.GetByIdAsync(id);
        if (query == null)
        {
            return NotFound();
        }
        await _topicService.DeleteAsync(id);
        return Ok(query);
    }

}
