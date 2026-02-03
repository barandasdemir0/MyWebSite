using CV.EntityLayer.Entities;
using DtoLayer.ContactDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract
{
    public interface IContactService:IGenericService<Contact,ContactDto,CreateContactDto,UpdateContactDto>
    {
    }
}
