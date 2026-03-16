using DtoLayer.AuthDtos;

namespace BusinessLayer.Abstract;

public interface IUserProfileService
{
    Task<UserProfileDto> GetUserProfileAsync(string UserId, CancellationToken cancellationToken);
    Task<bool> ChangePasswordAsync(string userId, ChangePasswordDto changePasswordDto, CancellationToken cancellationToken);
    Task<bool> Toggle2FAAsync(string userId, Toggle2FADto toggle2FADto, CancellationToken cancellationToken);
    Task<bool> AssignRoleAsync(string UserId, string role, CancellationToken cancellationToken);

    Task<List<PendingUserDto>> GetPendingUsersAsync(CancellationToken cancellationToken);
    Task<bool> ApproveUserAsync(string userId,string role, CancellationToken cancellationToken);
    Task<bool> RejectUserAsync(string userId, CancellationToken cancellationToken);
}
