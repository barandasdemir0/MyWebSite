using System.Text.Json.Serialization;

namespace DtoLayer.GithubRepoDtos;

public class GithubApiRepoDto
{
    [JsonPropertyName("name")]
    public string RepoName { get; set; } = string.Empty;


    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;


    [JsonPropertyName("language")]
    public string Language { get; set; } = string.Empty;


    [JsonPropertyName("stargazers_count")]
    public int StarCount { get; set; }


    [JsonPropertyName("forks_count")]
    public int ForkCount { get; set; }


    [JsonPropertyName("html_url")]
    public string RepoUrl { get; set; } = string.Empty;

    [JsonPropertyName("fork")]
    public bool Fork { get; set; }


}
