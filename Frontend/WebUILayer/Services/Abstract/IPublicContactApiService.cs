using DtoLayer.ContactDtos;

namespace WebUILayer.Services.Abstract;

public interface IPublicContactApiService:IPublicReadApiService<ContactDto>
{
    Task<ContactDto> GetPublicContactSettingsAsync();
}
