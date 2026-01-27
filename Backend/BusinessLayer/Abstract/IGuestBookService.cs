using CV.EntityLayer.Entities;
using DtoLayer.GuestBookDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract
{
    public interface IGuestBookService:IGenericService<GuestBook,GuestBookListDto,CreateGuestBookDto,UpdateGuestBookDto>
    {
        Task<GuestBook?> GetDetailsByIdAsync(Guid guid);
    }
}
