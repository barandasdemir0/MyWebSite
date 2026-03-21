using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DtoLayer.AuthDtos;
using EntityLayer.Entities;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;

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
            throw new Exception("Kullanıcı bulunamadı");
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
