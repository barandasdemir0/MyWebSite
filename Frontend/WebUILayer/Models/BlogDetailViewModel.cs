using DtoLayer.AboutDtos;
using DtoLayer.BlogPostDtos;
using DtoLayer.ProjectDtos;
using DtoLayer.SocialMediaDtos;

namespace WebUILayer.Models;

public class BlogDetailViewModel
{
    public BlogPostDto? BlogPostDto { get; set; } 
    public AboutDto? AboutDto { get; set; }
    public List<SocialMediaDto> SocialMediaDtos { get; set; } = new();
    public List<ProjectDto> ProjectDtos { get; set; } = new();
}
