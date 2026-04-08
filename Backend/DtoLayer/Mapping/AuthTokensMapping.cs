using DtoLayer.AuthDtos.Responses;
using Mapster;

namespace DtoLayer.Mapping;

public class AuthTokensMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // RefreshTokenRequestDto, LoginResultDto gibi DTO’lar
        // Eğer entity ile map gerekiyorsa buraya ekle

        //RefreshTokenRequestDto(request)
        //LoginResultDto(response, eğer token içeriyor)
        //Setup2FAResultDto
        //TwoFactorVerifyDto(request / response)
    }
}
