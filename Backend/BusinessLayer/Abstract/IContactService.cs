using CV.EntityLayer.Entities;
using DtoLayer.ContactDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract
{
    public interface IContactService:IGenericService<Contact,ContactDto,CreateContactDto,UpdateContactDto>
    {
    }
}
