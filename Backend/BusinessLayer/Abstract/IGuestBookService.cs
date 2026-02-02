using CV.EntityLayer.Entities;
using DtoLayer.CertificateDto;
using DtoLayer.GuestBookDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract
{
    public interface IGuestBookService:IGenericService<GuestBook,GuestBookListDto,CreateGuestBookDto,UpdateGuestBookDto>
    {
        Task<GuestBookDto?> GetDetailsByIdAsync(Guid guid);

        Task<GuestBookDto?> RestoreAsync(Guid guid);
        Task<List<GuestBookListDto>> GetAllAdminAsync();
    }
}
