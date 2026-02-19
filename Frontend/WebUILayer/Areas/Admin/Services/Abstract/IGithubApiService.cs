using DtoLayer.GithubRepoDtos;
using SharedKernel.Shared;

namespace WebUILayer.Areas.Admin.Services.Abstract;

public interface IGithubApiService:IGenericApiService<GithubRepoDto,CreateGithubRepoDto,UpdateGithubRepoDto>
{
    Task<PagedResult<GithubApiRepoDto>> FetchFromGithubAsync(string username, PaginationQuery query);
    Task<List<GithubRepoDto>> SyncSelectedAsync(string username, List<string> repoNames);
    Task ToggleVisibilityAsync(Guid id);
}
