using DtoLayer.GithubRepoDtos;

namespace WebUILayer.Areas.Admin.Models;

public class GithubIndexViewModel:BasePaginationViewModel
{
    public List<GithubApiRepoDto> GithubRepos { get; set; } = [];
    public string Username { get; set; } = string.Empty;
    public HashSet<string> SavedRepoNames { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    public bool IsSaved(string repoName) => SavedRepoNames.Contains(repoName);
}
