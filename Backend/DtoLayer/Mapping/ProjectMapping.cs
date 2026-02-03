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
            config.NewConfig<Project, ProjectDtos.ProjectDto>();
            config.NewConfig<Project, ProjectDtos.ProjectListDto>();
            config.NewConfig<CreateProjectDto, Project>().Ignore(x => x.Id);
            config.NewConfig<UpdateProjectDto, Project>().Ignore(x => x.Id);
        }
    }
}
