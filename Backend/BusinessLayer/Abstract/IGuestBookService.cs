using CV.EntityLayer.Entities;
using DtoLayer.GuestBookDtos;
using DtoLayer.ProjectDtos;
using SharedKernel.Shared;

namespace BusinessLayer.Abstract;

public interface IGuestBookService:IGenericService<GuestBook, GuestBookDto, CreateGuestBookDto,UpdateGuestBookDto>
{
    Task<GuestBookDto?> GetDetailsByIdAsync(Guid guid, CancellationToken cancellationToken = default);

    Task<GuestBookDto?> RestoreAsync(Guid guid, CancellationToken cancellationToken = default);
    Task<PagedResult<GuestBookDto>> GetAllAdminAsync(PaginationQuery paginationQuery , bool? isApproved = null, CancellationToken cancellationToken = default);
    Task<PagedResult<GuestBookDto>> GetAllUserAsync(PaginationQuery paginationQuery , CancellationToken cancellationToken = default);

    Task<GuestBookDto?> ApproveAsync(Guid guid, CancellationToken cancellationToken = default);


    //son 2 mesaj listeleme
    Task<List<GuestBookDto>> GetLatestAsync(int count, CancellationToken cancellationToken = default);
}
