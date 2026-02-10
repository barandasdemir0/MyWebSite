using DtoLayer.ProjectDtos;
using DtoLayer.TopicDtos;

namespace WebUILayer.Areas.Admin.Models;

public class ProjectCreateViewModel
{
    public CreateProjectDto CreateProjectDtos { get; set; } = new ();
    public List<TopicDto> topicDtos { get; set; } = new();
}
