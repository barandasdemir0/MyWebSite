using CV.EntityLayer.Entities;
using DtoLayer.EducationDto;
using DtoLayer.GithubRepoDto;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.Mapping;

public sealed class GithubRepoMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<GithubRepo, GithubRepoDto.GithubRepoDto>();
        config.NewConfig<CreateGithubRepoDto, GithubRepo>().Ignore(x => x.Id);
        config.NewConfig<UpdateGithubRepoDto, GithubRepo>().Ignore(x => x.Id);
    }
}
