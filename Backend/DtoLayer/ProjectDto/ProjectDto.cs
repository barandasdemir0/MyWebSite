using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.ProjectDto;

public class ProjectDto
{
    public Guid Id { get; set; }
    // Temel
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }

    // Bilgiler
    public string? ClientName { get; set; }
    public string? Duration { get; set; }
    public string? Role { get; set; }

    // Detaylar
    public string? Goals { get; set; }
    public string? Features { get; set; }
    public string? Results { get; set; }

    // Linkler & Tech
    public string? WebsiteUrl { get; set; }
    public string? GithubUrl { get; set; }
    public string Technologies { get; set; } = string.Empty;

    public bool IsFeatured { get; set; }
    public int DisplayOrder { get; set; }

    // İlişkiler (Okuma)
    public List<string> Topics { get; set; } = new();        // Topic isimleri
    public List<string> RelatedBlogs { get; set; } = new();  // Blog başlıkları
}
