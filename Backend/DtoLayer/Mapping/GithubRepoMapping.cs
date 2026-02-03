using CV.EntityLayer.Entities;
using DtoLayer.EducationDtos;
using DtoLayer.GithubRepoDtos;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.Mapping;

public sealed class GithubRepoMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<GithubRepo, GithubRepoDtos.GithubRepoDto>();
        config.NewConfig<CreateGithubRepoDto, GithubRepo>().Ignore(x => x.Id);
        config.NewConfig<UpdateGithubRepoDto, GithubRepo>().Ignore(x => x.Id);
    }
}
