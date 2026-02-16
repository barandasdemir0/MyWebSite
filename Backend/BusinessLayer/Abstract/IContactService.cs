using CV.EntityLayer.Entities;
using DtoLayer.ContactDtos;
using DtoLayer.SiteSettingDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract
{
    public interface IContactService:IGenericService<Contact,ContactDto,CreateContactDto,UpdateContactDto>
    {
        Task<ContactDto?> GetSingleAsync(CancellationToken cancellationToken = default);

        Task<ContactDto> SaveAsync(UpdateContactDto updateContactDto, CancellationToken cancellationToken = default);
    }
}
