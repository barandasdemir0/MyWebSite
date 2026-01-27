using DtoLayer.MessageDto;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract
{
    public interface IMessageService:IGenericService<Message,MessageListDto,CreateMessageDto,UpdateMessageDto>
    {
        Task<MessageDto?> GetDetailsByIdAsync(Guid guid);
    }
}
