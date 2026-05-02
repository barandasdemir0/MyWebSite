using SharedKernel.Shared;

namespace DtoLayer.GithubRepoDtos;

public record GithubRepoDto : IHasId
{
    public Guid Id { get; init; }
    public string RepoName { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;     // Repo açıklaması
    public string Language { get; init; } = string.Empty;        // "C#", "Python", "JavaScript"
    public int StarCount { get; init; }                          // ⭐ sayısı
    public int ForkCount { get; init; }                          // 🍴 sayısı
    public string RepoUrl { get; init; } = string.Empty;         // GitHub linki
    public int? DisplayOrder { get; init; }
    public bool IsVisible { get; init; }
    public bool IsDeleted { get; init; } = false;
}
