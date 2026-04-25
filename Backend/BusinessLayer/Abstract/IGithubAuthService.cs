using DtoLayer.GuestBookDtos;

namespace BusinessLayer.Abstract;

public interface IGithubAuthService
{
    Task<OAuthUserProfileDto?> AuthenticateAsync(GithubAuthRequestDto githubAuthRequestDto,CancellationToken cancellationToken=default);
}
