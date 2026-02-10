using CV.EntityLayer.Entities;
using DtoLayer.HeroDtos;
using DtoLayer.ProjectDtos;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.Mapping
{
    public sealed class ProjectMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Project, ProjectDtos.ProjectDto>()
                .Map(x=>x.Topics,y=>y.ProjectTopics.Select(z=>z.Topic.Name));
            config.NewConfig<Project, ProjectDtos.ProjectListDto>()
                .Map(x=>x.Topics,y=>y.ProjectTopics.Select(z=>z.Topic.Name));
            config.NewConfig<CreateProjectDto, Project>().Ignore(x => x.Id);
            config.NewConfig<UpdateProjectDto, Project>().Ignore(x => x.Id);
        }
    }
}
