using CV.EntityLayer.Entities;
using DtoLayer.GithubRepoDto;
using DtoLayer.GuestBookDto;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.Mapping;

public sealed class GuestBookMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {

        config.NewConfig<GuestBook, GuestBookDto.GuestBookDto>();
        config.NewConfig<GuestBook, GuestBookDto.GuestBookListDto>();
        config.NewConfig<CreateGuestBookDto, GuestBook>().Ignore(x => x.Id);
        config.NewConfig<UpdateGuestBookDto, GuestBook>().Ignore(x => x.Id);
    }
}
