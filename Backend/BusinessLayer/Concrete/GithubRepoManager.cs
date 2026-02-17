using BusinessLayer.Abstract;
using BusinessLayer.Extensions;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DtoLayer.GithubRepoDtos;
using MapsterMapper;
using SharedKernel.Shared;

namespace BusinessLayer.Concrete;

public class GithubRepoManager : GenericManager<GithubRepo,GithubRepoDto,CreateGithubRepoDto,UpdateGithubRepoDto> , IGithubRepoService
{

    private readonly IGithubRepoDal _githubRepoDal;

    public GithubRepoManager(IGithubRepoDal githubRepoDal, IMapper mapper) : base(githubRepoDal, mapper)
    {
        _githubRepoDal = githubRepoDal;
    }

    public override async Task<List<GithubRepoDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entity = await _githubRepoDal.GetAllAsync(tracking: false, cancellationToken:cancellationToken);
        return _mapper.Map<List<GithubRepoDto>>(entity.OrderBy(x => x.DisplayOrder));
    }

    public async Task<PagedResult<GithubRepoDto>> GetAllAdminAsync(PaginationQuery query, CancellationToken cancellationToken = default)
    {
        var (items, totalCount) = await _githubRepoDal.GetUserListPagesAsync(query.PageNumber, query.PageSize, cancellationToken);
        return _mapper.Map<List<GithubRepoDto>>(items).ToPagedResult(query.PageNumber, query.PageSize, totalCount);
    }
}
