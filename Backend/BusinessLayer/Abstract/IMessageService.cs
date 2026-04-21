using CV.EntityLayer.Entities;
using DtoLayer.MessageDtos;
using SharedKernel.Enums;
using SharedKernel.Shared;

namespace BusinessLayer.Abstract;

public interface IMessageService : IGenericService<Message, MessageDto, CreateMessageDto, UpdateMessageDto>
{
    Task<MessageDto?> GetDetailsByIdAsync(Guid guid, CancellationToken cancellationToken = default);
    //Task<List<MessageListDto>> GetAllAdminAsync(CancellationToken cancellationToken = default);

    Task<PagedResult<MessageDto>> GetAllAdmin(PaginationQuery paginationQuery, CancellationToken cancellationToken = default);


    // ── YENİ: Listeleme ──
    Task<PagedResult<MessageDto>> GetByFolderAsync(MessageFolder folder, PaginationQuery paginationQuery, CancellationToken cancellationToken = default);

    Task<PagedResult<MessageDto>> GetStarredAsync(PaginationQuery paginationQuery, CancellationToken cancellationToken = default);

    Task<PagedResult<MessageDto>> GetReadAsync(PaginationQuery paginationQuery, CancellationToken cancellationToken = default);

    // ── YENİ: Sidebar badge sayıları ──

    Task<Dictionary<string, int>> GetFolderCountsAsync(CancellationToken cancellationToken = default);

    // ── YENİ: Durum değiştirme ──

    Task<bool> MarkAsReadAsync(Guid guid, CancellationToken cancellationToken = default);
    Task<bool> ToggleStarAsync(Guid guid, CancellationToken cancellationToken = default);
   
    // ── YENİ: Restore ──
    Task<MessageDto?> RestoreAsync(Guid guid, CancellationToken cancellationToken = default);




}
