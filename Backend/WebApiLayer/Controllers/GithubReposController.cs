using BusinessLayer.Abstract;
using DtoLayer.GithubRepoDto;
using Microsoft.AspNetCore.Mvc;

namespace WebApiLayer.Controllers;


[Route("api/[controller]")]
[ApiController]

public sealed class GithubReposController:ControllerBase
{
    private readonly IGithubRepoService _githubRepoService;

    public GithubReposController(IGithubRepoService githubRepoService)
    {
        _githubRepoService = githubRepoService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = await _githubRepoService.GetAllAsync();
        return Ok(query);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = await _githubRepoService.GetByIdAsync(id);
        if (query == null)
        {
            return NotFound();
        }
        return Ok(query);
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGithubRepoDto createGithubRepoDto)
    {
        var query = await _githubRepoService.AddAsync(createGithubRepoDto);
        return CreatedAtAction(nameof(GetById), new { id = query.Id }, query);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateGithubRepoDto updateGithubRepoDto)
    {
        //updateAboutDto.Id = id; --> hatamız 1 burası businessin işi idi
        var query = await _githubRepoService.UpdateAsync(id,updateGithubRepoDto);
        if (query == null)
        {
            return NotFound();
        }
        return Ok(query);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var query = await _githubRepoService.GetByIdAsync(id);
        if (query == null)
        {
            return NotFound();
        }
        await _githubRepoService.DeleteAsync(id);
        return Ok(query);
    }





}
