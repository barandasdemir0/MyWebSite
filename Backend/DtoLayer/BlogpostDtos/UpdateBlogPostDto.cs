using DtoLayer.TopicDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.BlogpostDtos;

public class UpdateBlogPostDto
{
    public Guid Id { get; set; } // --> güncelleme işleminde ıdyi almamız gerekir
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string CoverImage { get; set; } = string.Empty;
    public string Technologies { get; set; } = string.Empty;
    public int ReadTime { get; set; }
    public bool IsPublished { get; set; }

    // İlişkiler (Seçim)
    public List<Guid> TopicIds { get; set; } = new();
    public List<TopicDto>? topicList { get; set; }
}
