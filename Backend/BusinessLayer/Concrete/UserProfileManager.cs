using BusinessLayer.Abstract;
using DtoLayer.AuthDtos;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Identity;

namespace BusinessLayer.Concrete;

public class UserProfileManager : IUserProfileService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public UserProfileManager(UserManager<AppUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<bool> AssignRoleAsync(string UserId, string role, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(UserId);
        if (user == null)
        {
            return false;
        }
        if (!await _roleManager.RoleExistsAsync(role))
        {
            await _roleManager.CreateAsync(new IdentityRole<Guid>
            {
                Name = role
            });

        }
        await _userManager.AddToRoleAsync(user, role);
        return true;
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

    public async Task<bool> Toggle2FAAsync(string userId, Toggle2FADto toggle2FADto, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return false;

        }
        await _userManager.SetTwoFactorEnabledAsync(user, toggle2FADto.Enable);
        user.Preferred2FAProvider = toggle2FADto.Enable ? toggle2FADto.Provider : TwoFactorProvider.None;
        await _userManager.UpdateAsync(user);
        return true;
    }
    
}
