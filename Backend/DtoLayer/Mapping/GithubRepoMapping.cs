using CV.EntityLayer.Entities;
using DtoLayer.GithubRepoDtos;
using Mapster;

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
