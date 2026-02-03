using System;
using System.Collections.Generic;
using System.Text;


namespace DtoLayer.BlogpostDtos;

public class CreateBlogPostDto
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? CoverImage { get; set; }
    public int ReadTime { get; set; }
    public bool IsPublished { get; set; }

    // İlişkiler (Seçim)
    public List<Guid> TopicIds { get; set; } = new();     // Seçilen topic ID'leri
    //public List<DtoLayer.TopicDtos.TopicDto>? TopicList { get; set; }
}
