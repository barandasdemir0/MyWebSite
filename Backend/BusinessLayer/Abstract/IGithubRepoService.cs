using CV.EntityLayer.Entities;
using DtoLayer.GithubRepoDtos;
using SharedKernel.Shared;

namespace BusinessLayer.Abstract;

public interface IGithubRepoService:IGenericService<GithubRepo,GithubRepoDto,CreateGithubRepoDto,UpdateGithubRepoDto>
{
    Task<PagedResult<GithubRepoDto>> GetAllAdminAsync(PaginationQuery query, CancellationToken cancellationToken = default);

    Task<PagedResult<GithubApiRepoDto>> FetchFromGithubAsync(PaginationQuery query, string username, CancellationToken cancellationToken = default);
    Task<List<GithubRepoDto>> SyncSelectedAsync(string username, List<string> repoNames, CancellationToken cancellationToken = default);
    Task<GithubRepoDto?> ToggleVisibilityAsync(Guid id, CancellationToken cancellationToken = default);
}
