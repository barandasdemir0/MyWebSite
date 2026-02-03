using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.ProjectDtos;

public class UpdateProjectDto
{
    //public Guid Id { get; set; } // --> güncelleme işleminde ıdyi almamız gerekir
    public string Name { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string? ClientName { get; set; }
    public string? Duration { get; set; }
    public string? Role { get; set; }
    public string? Goals { get; set; }
    public string? Features { get; set; }
    public string? Results { get; set; }
    public string? WebsiteUrl { get; set; }
    public string? GithubUrl { get; set; }
    public string Technologies { get; set; } = string.Empty;
    public bool IsFeatured { get; set; }
    public int DisplayOrder { get; set; }

    // İlişkiler (Seçim)
    public List<Guid> TopicIds { get; set; } = new();
}
