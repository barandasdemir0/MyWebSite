using System.Text.Json.Serialization;

namespace DtoLayer.GithubRepoDtos;

public record GithubApiRepoDto
{
    [JsonPropertyName("name")]
    public string RepoName { get; init; } = string.Empty;


    [JsonPropertyName("description")]
    public string Description { get; init; } = string.Empty;


    [JsonPropertyName("language")]
    public string Language { get; init; } = string.Empty;


    [JsonPropertyName("stargazers_count")]
    public int StarCount { get; init; }


    [JsonPropertyName("forks_count")]
    public int ForkCount { get; init; }


    [JsonPropertyName("html_url")]
    public string RepoUrl { get; init; } = string.Empty;

    [JsonPropertyName("fork")]
    public bool Fork { get; init; }


}
