using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.BlogpostDtos;

public class BlogPostDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? CoverImage { get; set; }
    public int ReadTime { get; set; }
    public bool IsPublished { get; set; }
    public DateTime? PublishedAt { get; set; }
    public int ViewCount { get; set; }
    public bool IsDeleted { get; set; } = false;

    // İlişkiler (Okuma)
    public List<string> Topics { get; set; } = new();        // Topic isimleri
    public List<string> RelatedProjects { get; set; } = new(); // Proje isimleri
}
