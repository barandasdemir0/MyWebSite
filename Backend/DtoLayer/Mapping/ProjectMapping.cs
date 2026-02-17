using CV.EntityLayer.Entities;
using DtoLayer.ProjectDtos;
using Mapster;

namespace DtoLayer.Mapping;

public sealed class ProjectMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Project, ProjectDtos.ProjectDto>()
            .Map(x=>x.Topics,y=>y.ProjectTopics.Select(z=>z.Topic.Name).ToList())
            .Map(x=>x.TopicIds,y=>y.ProjectTopics.Select(z=>z.TopicId).ToList());
        config.NewConfig<Project, ProjectDtos.ProjectListDto>()
            .Map(x=>x.Topics,y=>y.ProjectTopics.Select(z=>z.Topic.Name).ToList());
        config.NewConfig<CreateProjectDto, Project>().Ignore(x => x.Id);
        config.NewConfig<UpdateProjectDto, Project>().Ignore(x => x.Id);
    }
}
