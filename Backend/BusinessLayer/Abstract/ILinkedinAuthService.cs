using DtoLayer.GuestBookDtos;

namespace BusinessLayer.Abstract;

public interface ILinkedinAuthService
{
    Task<OAuthUserProfileDto?> AuthenticateAsync(LinkedinAuthRequestDto linkedinAuthRequestDto,CancellationToken cancellationToken = default);
}
