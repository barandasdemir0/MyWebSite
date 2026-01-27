using CV.EntityLayer.Entities;
using DtoLayer.CertificateDto;
using DtoLayer.ContactDto;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace DtoLayer.Mapping
{
    public sealed class ContactMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Contact, ContactDto.ContactDto>();
            config.NewConfig<CreateContactDto, Contact>().Ignore(x => x.Id);
            config.NewConfig<UpdateContactDto, Contact>().Ignore(x => x.Id);
        }
    }
}
