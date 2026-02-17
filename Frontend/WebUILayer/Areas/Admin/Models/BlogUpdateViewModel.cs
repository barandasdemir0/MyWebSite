using DtoLayer.BlogPostDtos;
using DtoLayer.TopicDtos;

namespace WebUILayer.Areas.Admin.Models;

public class BlogUpdateViewModel
{
    public UpdateBlogPostDto updateBlogPostDto { get; set; } = new();
    public List<TopicDto> topicDtos { get; set; } = new();
}
