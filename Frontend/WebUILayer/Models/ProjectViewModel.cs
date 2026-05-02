using DtoLayer.BlogPostDtos;
using DtoLayer.ProjectDtos;
using DtoLayer.TopicDtos;

namespace WebUILayer.Models;

public class ProjectViewModel:BasePaginationViewModel
{
    public List<TopicDto> topicDtos { get; set; } = new();
    public List<ProjectDto> projectDtos { get; set; } = new();
    public ProjectDto? ProjectDto { get; set; }
    public List<BlogPostDto> blogPostDtos { get; set; } = new();
}
