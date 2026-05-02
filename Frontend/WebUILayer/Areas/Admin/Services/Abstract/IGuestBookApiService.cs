using DtoLayer.GuestBookDtos;
using SharedKernel.Shared;

namespace WebUILayer.Areas.Admin.Services.Abstract;

public interface IGuestBookApiService:IGenericApiService<GuestBookDto,CreateGuestBookDto,UpdateGuestBookDto>
{
    Task<PagedResult<GuestBookDto>> GetAllAdminAsync(PaginationQuery paginationQuery, bool? isApproved = null);
    Task<PagedResult<GuestBookDto>> GetAllUserAsync(PaginationQuery paginationQuery);

    Task ApproveAsync(Guid guid);
    Task RestoreAsync(Guid guid);

    Task<List<GuestBookDto>> GetLatestAsync(int count);
}
