using DtoLayer.MessageDtos;
using SharedKernel.Enums;
using SharedKernel.Shared;

namespace WebUILayer.Areas.Admin.Services.Abstract;

public interface IMessageApiService : IGenericApiService<MessageDto, CreateMessageDto, UpdateMessageDto>
{
    Task<PagedResult<MessageDto>> GetByFolderAsync(MessageFolder folder, PaginationQuery paginationQuery);
    Task<PagedResult<MessageDto>> GetStarredAsync(PaginationQuery paginationQuery);
    Task<PagedResult<MessageDto>> GetReadAsync(PaginationQuery paginationQuery);
    Task<PagedResult<MessageDto>> GetAllAdminAsync(PaginationQuery paginationQuery);
    Task<Dictionary<string, int>> GetFolderCountsAsync();
    Task<MessageDto?> GetDetailAsync(Guid guid);
    Task MarkAsReadAsync(Guid guid);
    Task ToggleStarAsync(Guid guid);
    Task RestoreAsync(Guid guid);
}
