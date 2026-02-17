using CV.EntityLayer.Entities;
using DtoLayer.GithubRepoDtos;
using SharedKernel.Shared;

namespace BusinessLayer.Abstract;

public interface IGithubRepoService:IGenericService<GithubRepo,GithubRepoDto,CreateGithubRepoDto,UpdateGithubRepoDto>
{
    Task<PagedResult<GithubRepoDto>> GetAllAdminAsync(PaginationQuery query, CancellationToken cancellationToken = default);
}
