using CV.EntityLayer.Entities;
using DtoLayer.ContactDtos;
using Mapster;

namespace DtoLayer.Mapping;

public sealed class ContactMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Contact, ContactDtos.ContactDto>();
        config.NewConfig<CreateContactDto, Contact>().Ignore(x => x.Id);
        config.NewConfig<UpdateContactDto, Contact>().Ignore(x => x.Id);
    }
}
