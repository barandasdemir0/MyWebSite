using DtoLayer.AuthDtos;
using EntityLayer.Entities;
using Mapster;

namespace DtoLayer.Mapping;

public class UserMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AppUser, PendingUserDto>().Map(dest => dest.UserId, src => src.Id.ToString());
    }
}
