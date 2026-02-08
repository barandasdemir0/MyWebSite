using DtoLayer.ProjectDtos;
using DtoLayer.TopicDtos;

namespace WebUILayer.Areas.Admin.Models
{
    public class ProjectIndexViewModel:BasePaginationViewModel
    {
        public List<ProjectDto> projectDtos { get; set; } = new List<ProjectDto>();
        public List<TopicDto> topicDtos { get; set; } = new List<TopicDto>();
    }
}
