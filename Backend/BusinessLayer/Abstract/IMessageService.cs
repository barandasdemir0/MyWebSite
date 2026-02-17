using CV.EntityLayer.Entities;
using DtoLayer.MessageDtos;
using SharedKernel.Shared;

namespace BusinessLayer.Abstract;

public interface IMessageService : IGenericService<Message, MessageDto, CreateMessageDto, UpdateMessageDto>
{
    Task<MessageDto?> GetDetailsByIdAsync(Guid guid, CancellationToken cancellationToken = default);
    //Task<List<MessageListDto>> GetAllAdminAsync(CancellationToken cancellationToken = default);

    Task<PagedResult<MessageListDto>> GetAllAdmin(PaginationQuery paginationQuery, CancellationToken cancellationToken = default);
}
