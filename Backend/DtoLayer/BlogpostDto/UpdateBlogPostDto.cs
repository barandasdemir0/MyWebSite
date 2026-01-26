using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.BlogpostDto;

public class UpdateBlogPostDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? CoverImage { get; set; }
    public int ReadTime { get; set; }
    public bool IsPublished { get; set; }

    // İlişkiler (Seçim)
    public List<Guid> TopicIds { get; set; } = new();
    public List<Guid> ProjectIds { get; set; } = new();
}
