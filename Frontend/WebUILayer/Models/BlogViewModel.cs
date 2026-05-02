using DtoLayer.AboutDtos;
using DtoLayer.BlogPostDtos;
using DtoLayer.ProjectDtos;
using DtoLayer.SocialMediaDtos;
using DtoLayer.TopicDtos;

namespace WebUILayer.Models;

public class BlogViewModel:BasePaginationViewModel
{
    public List<BlogPostDto> blogPostListDtos { get; set; } = new();
    public List<TopicDto> topicDtos { get; set; } = new();
    public List<ProjectDto> projectListDtos { get; set; } = new();
    public List<SocialMediaDto> socialMediaDtos { get; set; } = new();
    public AboutDto? aboutDto { get; set; }
    public BlogPostDto? BlogPostDto { get; set; }
}
