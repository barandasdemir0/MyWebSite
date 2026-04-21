using DtoLayer.MessageDtos;

namespace WebUILayer.Services.Abstract;

public interface IPublicMessageApiService
{
    Task<bool> SendContactMessageAsync(CreateMessageDto createMessageDto);
}
