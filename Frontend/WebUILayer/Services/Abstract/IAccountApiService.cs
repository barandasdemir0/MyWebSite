using DtoLayer.AuthDtos.Requests;

namespace WebUILayer.Services.Abstract;

public interface IAccountApiService
{
    Task ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
    Task<string?> VerifyResetOtpAsync(VerifyResetOtpDto verifyResetOtpDto);
    Task<bool> SetNewPasswordAsync(SetNewPasswordDto setNewPasswordDto);
}
