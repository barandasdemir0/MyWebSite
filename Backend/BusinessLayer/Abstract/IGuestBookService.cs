using CV.EntityLayer.Entities;
using DtoLayer.CertificateDtos;
using DtoLayer.GuestBookDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract
{
    public interface IGuestBookService:IGenericService<GuestBook,GuestBookListDto,CreateGuestBookDto,UpdateGuestBookDto>
    {
        Task<GuestBookDto?> GetDetailsByIdAsync(Guid guid, CancellationToken cancellationToken = default);

        Task<GuestBookDto?> RestoreAsync(Guid guid, CancellationToken cancellationToken = default);
        Task<List<GuestBookListDto>> GetAllAdminAsync( CancellationToken cancellationToken = default);
    }
}
