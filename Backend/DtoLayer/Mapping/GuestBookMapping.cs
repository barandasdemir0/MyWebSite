using CV.EntityLayer.Entities;
using DtoLayer.GuestBookDtos;
using Mapster;

namespace DtoLayer.Mapping;

public sealed class GuestBookMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {

        config.NewConfig<GuestBook, GuestBookDtos.GuestBookDto>();
        config.NewConfig<GuestBook, GuestBookDtos.GuestBookListDto>();
        config.NewConfig<CreateGuestBookDto, GuestBook>().Ignore(x => x.Id);
        config.NewConfig<UpdateGuestBookDto, GuestBook>().Ignore(x => x.Id);
    }
}
