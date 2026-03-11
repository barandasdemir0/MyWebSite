using DtoLayer.AuthDtos;

namespace WebUILayer.Areas.Admin.Services.Abstract;

public interface IUserProfileApiService
{
    Task<UserProfileDto?> GetUserProfileAsync(string userId);
    Task<bool> ChangePasswordAsync(string userId, ChangePasswordDto changePasswordDto);
    Task<bool> Toggle2FAAsync(string userId, Toggle2FADto dto);
}
