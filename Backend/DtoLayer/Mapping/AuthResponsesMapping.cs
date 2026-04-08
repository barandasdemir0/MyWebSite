using CV.EntityLayer.Entities;
using DtoLayer.AuthDtos.Requests;
using DtoLayer.AuthDtos.Responses;
using Mapster;

namespace DtoLayer.Mapping;

public class AuthResponsesMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AppUser, PendingUserDto>().Map(dest => dest.UserId, src => src.Id.ToString());
        config.NewConfig<AppUser, ApprovedUserDto>().Map(dest => dest.UserId, src => src.Id.ToString()).Ignore(dest => dest.Role);
        config.NewConfig<AppUser, UserProfileDto>().Map(dest => dest.Id, src => src.Id.ToString()).Ignore(dest => dest.Preferred2FAProvider, src => src.Preferred2FAProvider);

        //ApprovedUserDto
        //PendingUserDto
        //UserProfileDto
        //LoginResultDto
        //Setup2FAResultDto
        //RegisterResultDto


    }
}
