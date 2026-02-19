using BusinessLayer.Abstract;
using BusinessLayer.Extensions;
using CV.EntityLayer.Entities;
using DataAccessLayer.Abstract;
using DtoLayer.GithubRepoDtos;
using MapsterMapper;
using SharedKernel.Shared;
using System.Net.Http.Json;

namespace BusinessLayer.Concrete;

public class GithubRepoManager : GenericManager<GithubRepo,GithubRepoDto,CreateGithubRepoDto,UpdateGithubRepoDto> , IGithubRepoService
{

    private readonly IGithubRepoDal _githubRepoDal;
    private readonly IHttpClientFactory _httpClientFactory;

    public GithubRepoManager(IGithubRepoDal githubRepoDal, IMapper mapper, IHttpClientFactory httpClientFactory) : base(githubRepoDal, mapper)
    {
        _githubRepoDal = githubRepoDal;
        _httpClientFactory = httpClientFactory;
    }

    public override async Task<List<GithubRepoDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entity = await _githubRepoDal.GetAllAsync(filter:x=>x.IsVisible,tracking: false, cancellationToken:cancellationToken);
        return _mapper.Map<List<GithubRepoDto>>(entity.OrderBy(x => x.DisplayOrder));
    }

    public async Task<PagedResult<GithubRepoDto>> GetAllAdminAsync(PaginationQuery query, CancellationToken cancellationToken = default)
    {
        var (items, totalCount) = await _githubRepoDal.GetUserListPagesAsync(query.PageNumber, query.PageSize, cancellationToken);
        return _mapper.Map<List<GithubRepoDto>>(items).ToPagedResult(query.PageNumber, query.PageSize, totalCount);
    }

    public override async Task<GithubRepoDto?> UpdateAsync(Guid guid, UpdateGithubRepoDto dto, CancellationToken cancellationToken = default)
    {
        if (dto.IsVisible)
        {
            var visibleCount = (await _repository.GetAllAsync(
                filter: x => x.IsVisible && x.Id != guid,
                tracking: false,
                cancellationToken: cancellationToken
                )).Count;
            if (visibleCount>=4)
            {
                throw new InvalidOperationException("En fazla 4 repo görünür yapılabilir!");
            }
        }
        return await base.UpdateAsync(guid, dto, cancellationToken);
    }

    // GitHub API'den repoları çek (sayfalı)
    public async Task<PagedResult<GithubApiRepoDto>> FetchFromGithubAsync(PaginationQuery query, string username, CancellationToken cancellationToken = default)
    {
        var allRepos = await FetchAllFromGithubAsync(username, cancellationToken);
        var totalCount = allRepos.Count;
        var items = allRepos
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToList();
        return items.ToPagedResult(query.PageNumber, query.PageSize, totalCount);
    }
    // Seçilenleri kaydet
    public async Task<List<GithubRepoDto>> SyncSelectedAsync(string username, List<string> repoNames, CancellationToken cancellationToken = default)
    {
        var githubRepos = await FetchAllFromGithubAsync(username, cancellationToken);
        var selected = githubRepos
            .Where(r => repoNames.Contains(r.RepoName, StringComparer.OrdinalIgnoreCase))
            .ToList();
        int order = 1;
        foreach (var repo in selected)
        {
            var existing = await _repository.GetAsync(x => x.RepoUrl == repo.RepoUrl, cancellationToken: cancellationToken);
            if (existing != null)
            {
                _mapper.Map(repo, existing);
                existing.Description ??= string.Empty;
                existing.Language ??= string.Empty;
                existing.DisplayOrder = order++;
                await _repository.UpdateAsync(existing, cancellationToken);
            }
            else
            {
                var entity = _mapper.Map<GithubRepo>(repo);
                entity.Description ??= string.Empty;
                entity.Language ??= string.Empty;
                entity.IsVisible = false;
                await _repository.AddAsync(entity, cancellationToken);
            }
        }
        await _repository.SaveAsync(cancellationToken);
        return await GetAllAsync(cancellationToken);
    }
    // Görünürlük toggle — tüm mantık burada
    public async Task<GithubRepoDto?> ToggleVisibilityAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken: cancellationToken);
        if (entity == null) return null;
        if (!entity.IsVisible)
        {
            var visibleCount = (await _repository.GetAllAsync(
                filter: x => x.IsVisible,
                tracking: false,
                cancellationToken: cancellationToken)).Count;
            if (visibleCount >= 4)
                throw new InvalidOperationException("En fazla 4 repo görünür yapılabilir!");
            entity.DisplayOrder = visibleCount + 1;
        }
        else
        {
            entity.DisplayOrder = 0;
        }
        entity.IsVisible = !entity.IsVisible;
        await _repository.UpdateAsync(entity, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
        return _mapper.Map<GithubRepoDto>(entity);
    }
    // GitHub API çağrısı (private helper)
    private async Task<List<GithubApiRepoDto>> FetchAllFromGithubAsync(string username, CancellationToken cancellationToken = default)
    {
        var client = _httpClientFactory.CreateClient("GithubApi");
        var repos = await client.GetFromJsonAsync<List<GithubApiRepoDto>>(
            $"users/{username}/repos?per_page=100&sort=updated",
            cancellationToken);
        return repos?.Where(r => !r.Fork).ToList() ?? [];
    }
}
