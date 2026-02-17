using DtoLayer.BlogPostDtos;
using DtoLayer.TopicDtos;

namespace WebUILayer.Areas.Admin.Models;

public class BlogCreateViewModel
{
    public CreateBlogPostDto createBlogPostDto { get; set; } = new();
    public List<TopicDto> topicDtos { get; set; } = new();
}
