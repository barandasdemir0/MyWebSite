using DtoLayer.ContactDtos;

namespace WebUILayer.Areas.Admin.Services.Abstract
{
    public interface IContactApiService:IGenericApiService<ContactDto,CreateContactDto,UpdateContactDto>
    {
        Task<UpdateContactDto> GetContactForEditAsync();
        Task SaveContactAsync(UpdateContactDto updateContactDto);
    }
}
