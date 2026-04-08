using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DtoLayer.AuthDtos;
using Microsoft.AspNetCore.Identity;
using SharedKernel.Exceptions;

namespace BusinessLayer.Concrete;

public class UserProfileManager : IUserProfileService
{
    private readonly UserManager<AppUser> _userManager;


    public UserProfileManager(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> ChangePasswordAsync(string userId, ChangePasswordDto changePasswordDto, CancellationToken cancellationToken)
    {
      
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return false;
        }
        var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);
        return result.Succeeded;
    }


    public async Task<UserProfileDto> GetUserProfileAsync(string UserId, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(UserId);
        if (user == null)
        {
            throw new NotFoundException("Kullanıcı bulunamadı",UserId);
        }

        return new UserProfileDto
        {
            Id = user.Id.ToString(),
            Name = user.Name ?? "",
            Surname = user.Surname ?? "",
            Email = user.Email ?? "",
            CreatedAt = user.CreatedAt,
            TwoFactorEnabled = await _userManager.GetTwoFactorEnabledAsync(user),
            Preferred2FAProvider = user.Preferred2FAProvider
        };
    }  

   

   
    
}
