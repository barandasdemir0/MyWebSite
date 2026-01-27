using CV.EntityLayer.Entities;
using DtoLayer.HeroDto;
using DtoLayer.ProjectDto;
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
            config.NewConfig<Project, ProjectDto.ProjectDto>();
            config.NewConfig<Project, ProjectDto.ProjectListDto>();
            config.NewConfig<CreateProjectDto, Project>().Ignore(x => x.Id);
            config.NewConfig<UpdateProjectDto, Project>().Ignore(x => x.Id);
        }
    }
}
