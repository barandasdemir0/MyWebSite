using DtoLayer.GithubRepoDtos;

namespace WebUILayer.Areas.Admin.Models;

public class GithubIndexViewModel:BasePaginationViewModel
{
    public List<GithubApiRepoDto> GithubRepos { get; set; } = [];
    public string Username { get; set; } = string.Empty;
    public List<string> SavedRepoNames { get; set; } = [];  // ← YENİ
}
