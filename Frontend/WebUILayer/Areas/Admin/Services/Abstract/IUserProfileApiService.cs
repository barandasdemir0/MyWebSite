using DtoLayer.AuthDtos;

namespace WebUILayer.Areas.Admin.Services.Abstract;

public interface IUserProfileApiService
{
    Task<UserProfileDto?> GetUserProfileAsync();
    Task<bool> ChangePasswordAsync(ChangePasswordDto changePasswordDto);
    Task<bool> Toggle2FAAsync(Toggle2FADto dto);

    Task<List<PendingUserDto>> GetPendingUsersAsync();
    Task<bool> ApproveUserAsync(string userId,string role);
    Task<bool> RejectUserAsync(string userId);

    Task<List<ApprovedUserDto>> GetAllUsersAsync();
    Task<List<string>> GetRolePermissionsAsync(string roleName);
    Task<bool> SaveRolePermissions(string roleName, List<string> permissions);

}
