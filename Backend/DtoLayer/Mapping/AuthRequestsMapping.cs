using CV.EntityLayer.Entities;
using DtoLayer.AuthDtos.Requests;
using Mapster;

namespace DtoLayer.Mapping;

public class AuthRequestsMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterDto, AppUser>()
            .Map(dest => dest.UserName, src => src.Email)
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.EmailConfirmed);
        //RegisterDto
        // ChangePasswordDto
        //ForgotPasswordDto
        //SetNewPasswordDto
        //SendEmailCodeDto
        //VerifyResetOtpDto
        //Toggle2FADto
        //TwoFactorVerifyDto
        //LoginDto
        //RefreshTokenRequestDto

    }
}
