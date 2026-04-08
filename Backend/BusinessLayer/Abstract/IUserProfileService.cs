using DtoLayer.AuthDtos.Requests;
using DtoLayer.AuthDtos.Responses;

namespace BusinessLayer.Abstract;

public interface IUserProfileService
{
    Task<UserProfileDto> GetUserProfileAsync(string UserId, CancellationToken cancellationToken);
    Task<bool> ChangePasswordAsync(string userId, ChangePasswordDto changePasswordDto, CancellationToken cancellationToken);
}
