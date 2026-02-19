namespace DtoLayer.GithubRepoDtos;

public class SyncGithubRequest
{
    public string Username { get; set; } = string.Empty;
    public List<string> RepoNames { get; set; } = new();
}
