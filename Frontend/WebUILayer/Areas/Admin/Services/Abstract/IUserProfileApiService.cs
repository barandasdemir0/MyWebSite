using DtoLayer.AuthDtos;

namespace WebUILayer.Areas.Admin.Services.Abstract;

public interface IUserProfileApiService
{
    Task<UserProfileDto?> GetUserProfileAsync();
    Task<bool> ChangePasswordAsync(ChangePasswordDto changePasswordDto);
    Task<bool> Toggle2FAAsync(Toggle2FADto dto);

}
