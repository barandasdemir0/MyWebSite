using BusinessLayer.Abstract;
using CV.EntityLayer.Entities;
using DtoLayer.AuthDtos.Requests;
using DtoLayer.AuthDtos.Responses;
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

    // Kullanıcı şifresini değiştirme
    public async Task<bool> ChangePasswordAsync(string userId, ChangePasswordDto changePasswordDto, CancellationToken cancellationToken)
    {
        // Kullanıcıyı ID'sine göre bul
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return false;
        } // Şifre değiştirme işlemini gerçekleştir
        var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);
        return result.Succeeded; // Şifre değiştirme işlemi başarılı mı?
    }


    // Kullanıcı profil bilgilerini alma

    public async Task<UserProfileDto> GetUserProfileAsync(string UserId, CancellationToken cancellationToken)
    {
        // Kullanıcıyı ID'sine göre bul
        var user = await _userManager.FindByIdAsync(UserId);
        if (user == null)
        {
            throw new NotFoundException("Kullanıcı bulunamadı",UserId); // Kullanıcı bulunamazsa NotFoundException fırlat
        }

        // Kullanıcı profil bilgilerini UserProfileDto'ya dönüştür ve döndür
        return new UserProfileDto
        {
            Id = user.Id.ToString(), // Kullanıcı ID'sini string olarak atama
            Name = user.Name ?? "", // Kullanıcı adı null ise boş string atama
            Surname = user.Surname ?? "", // Kullanıcı soyadı null ise boş string atama
            Email = user.Email ?? "", // Kullanıcı email'i null ise boş string atama
            CreatedAt = user.CreatedAt, // Kullanıcı oluşturulma tarihini atama
            TwoFactorEnabled = await _userManager.GetTwoFactorEnabledAsync(user), // Kullanıcının iki faktörlü kimlik doğrulama durumunu alma
            Preferred2FAProvider = user.Preferred2FAProvider // Kullanıcının tercih ettiği iki faktörlü kimlik doğrulama sağlayıcısını atama
        };
    }  

   

   
    
}
