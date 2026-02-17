using CV.EntityLayer.Entities;
using DtoLayer.GuestBookDtos;
using SharedKernel.Shared;

namespace BusinessLayer.Abstract;

public interface IGuestBookService:IGenericService<GuestBook,GuestBookListDto,CreateGuestBookDto,UpdateGuestBookDto>
{
    Task<GuestBookDto?> GetDetailsByIdAsync(Guid guid, CancellationToken cancellationToken = default);

    Task<GuestBookDto?> RestoreAsync(Guid guid, CancellationToken cancellationToken = default);
    Task<PagedResult<GuestBookListDto>> GetAllAdminAsync(PaginationQuery paginationQuery , CancellationToken cancellationToken = default);
    Task<PagedResult<GuestBookListDto>> GetAllUserAsync(PaginationQuery paginationQuery , CancellationToken cancellationToken = default);
}
