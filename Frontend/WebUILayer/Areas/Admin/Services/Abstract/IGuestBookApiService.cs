using DtoLayer.GuestBookDtos;
using SharedKernel.Shared;

namespace WebUILayer.Areas.Admin.Services.Abstract;

public interface IGuestBookApiService:IGenericApiService<GuestBookListDto,CreateGuestBookDto,UpdateGuestBookDto>
{
    Task<PagedResult<GuestBookListDto>> GetAllAdminAsync(PaginationQuery paginationQuery);
    Task<PagedResult<GuestBookListDto>> GetAllUserAsync(PaginationQuery paginationQuery);

    Task ApproveAsync(Guid guid);
    Task RestoreAsync(Guid guid);
}
