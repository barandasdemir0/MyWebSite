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


}
