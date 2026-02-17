using SharedKernel.Shared;

namespace DtoLayer.BlogPostDtos;

public class BlogPostListDto : IHasId
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? CoverImage { get; set; }
    public int ReadTime { get; set; }
    public DateTime? PublishedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
    public bool IsPublished { get; set; } = false;

    // Kartta kategori isimlerini göstermek şık olur
    public List<string> Topics { get; set; } = new();
}
