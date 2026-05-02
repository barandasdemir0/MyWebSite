using DtoLayer.GithubRepoDtos;
using WebUILayer.Services.Abstract;

namespace WebUILayer.Services.Concrete;

public class PublicGithubApiService : PublicReadApiService<GithubRepoDto>, IPublicGithubApiService
{
    public PublicGithubApiService(HttpClient httpClient/*, string endpoint*/) : base(httpClient, "githubrepos")
    {
    }
}
