using BusinessLayer.Abstract;
using DtoLayer.GithubRepoDtos;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Shared;

namespace WebApiLayer.Controllers;


[Route("api/[controller]")]

public sealed class GithubReposController : CrudController<GithubRepoDto,CreateGithubRepoDto,UpdateGithubRepoDto>
{
    private readonly IGithubRepoService _githubRepoService;

    public GithubReposController(IGithubRepoService githubRepoService) : base(githubRepoService)
    {
        _githubRepoService = githubRepoService;
    }


    [HttpGet("admin-all")] // hepsini getirme 
    public async Task<IActionResult> GetAllAdmin([FromQuery] PaginationQuery query, CancellationToken cancellationToken)
    {
        var result = await _githubRepoService.GetAllAdminAsync(query, cancellationToken);
        return Ok(result);
    }


    [HttpGet("fetch/{username}")]
    public async Task<IActionResult> FetchFromGithub([FromQuery] PaginationQuery query, string username, CancellationToken cancellationToken)
    {
        var repos = await _githubRepoService.FetchFromGithubAsync(query, username, cancellationToken);
        return Ok(repos);
    }
    [HttpPost("sync")]
    public async Task<IActionResult> SyncSelected([FromBody] SyncGithubRequest request, CancellationToken cancellationToken)
    {
        var result = await _githubRepoService.SyncSelectedAsync(request.Username, request.RepoNames, cancellationToken);
        return Ok(result);
    }
    [HttpPut("toggle/{id}")]
    public async Task<IActionResult> ToggleVisibility(Guid id, CancellationToken cancellationToken)
    {
        var result = await _githubRepoService.ToggleVisibilityAsync(id, cancellationToken);
        return result == null ? NotFound() : Ok(result);
    }



}
