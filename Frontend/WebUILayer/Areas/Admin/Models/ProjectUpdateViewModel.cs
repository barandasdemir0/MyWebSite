using DtoLayer.ProjectDtos;
using DtoLayer.TopicDtos;

namespace WebUILayer.Areas.Admin.Models;

public class ProjectUpdateViewModel
{
    public UpdateProjectDto UpdateProjectDto { get; set; } = new();
    public List<TopicDto> topicDtos { get; set; } = new();
}
