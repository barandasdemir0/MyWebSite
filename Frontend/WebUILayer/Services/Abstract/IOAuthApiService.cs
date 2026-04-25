using DtoLayer.GuestBookDtos;

namespace WebUILayer.Services.Abstract;

public interface IOAuthApiService
{
    Task<OAuthUserProfileDto?> GithubLoginAsync(GithubAuthRequestDto githubAuthRequestDto); 
    Task<OAuthUserProfileDto?> LinkedinLoginAsync(LinkedinAuthRequestDto linkedinAuthRequestDto); 
}
