using DtoLayer.GithubRepoDtos;
using SharedKernel.Shared;
using WebUILayer.Areas.Admin.Services.Abstract;

namespace WebUILayer.Areas.Admin.Services.Concrete;

public class GithubRepoApiService : GenericApiService<GithubRepoDto, CreateGithubRepoDto, UpdateGithubRepoDto>, IGithubApiService
{
    public GithubRepoApiService(HttpClient httpClient/*, string endpoint*/) : base(httpClient, "githubrepos")
    {
    }

    public async Task<PagedResult<GithubApiRepoDto>> FetchFromGithubAsync(string username, PaginationQuery query)
    {
        var url = $"{_endpoint}/fetch/{username}?PageNumber={query.PageNumber}&PageSize={query.PageSize}";
        var result = await _httpClient.GetFromJsonAsync<PagedResult<GithubApiRepoDto>>(url);
        return result ?? new PagedResult<GithubApiRepoDto>();
    }
    public async Task<List<GithubRepoDto>> SyncSelectedAsync(string username, List<string> repoNames)
    {
        var request = new SyncGithubRequest { Username = username, RepoNames = repoNames };
        var response = await _httpClient.PostAsJsonAsync($"{_endpoint}/sync", request);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }
        return await response.Content.ReadFromJsonAsync<List<GithubRepoDto>>() ?? [];
    }
    public async Task ToggleVisibilityAsync(Guid id)
    {
        var response = await _httpClient.PutAsync($"{_endpoint}/toggle/{id}", null);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }
    }
}
