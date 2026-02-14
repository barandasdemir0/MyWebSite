using DtoLayer.GuestBookDtos;
using DtoLayer.MessageDtos;
using EntityLayer.Concrete;
using SharedKernel.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract
{
    public interface IMessageService : IGenericService<Message, MessageDto, CreateMessageDto, UpdateMessageDto>
    {
        Task<MessageDto?> GetDetailsByIdAsync(Guid guid, CancellationToken cancellationToken = default);
        //Task<List<MessageListDto>> GetAllAdminAsync(CancellationToken cancellationToken = default);

        Task<PagedResult<MessageListDto>> GetAllAdmin(PaginationQuery paginationQuery, CancellationToken cancellationToken = default);
    }
}
