namespace DtoLayer.BlogPostDtos;

public class CreateBlogPostDto
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string CoverImage { get; set; } = string.Empty;
    public string Technologies { get; set; } = string.Empty;
    public int ReadTime { get; set; }
    public bool IsPublished { get; set; }

    // İlişkiler (Seçim)
    public List<Guid> TopicIds { get; set; } = new();     // Seçilen topic ID'leri
}
