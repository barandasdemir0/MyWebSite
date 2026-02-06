using DtoLayer.ContactDtos;
using Mapster;
using WebUILayer.Areas.Admin.Services.Abstract;

namespace WebUILayer.Areas.Admin.Services.Concrete
{
    public class ContactApiService : GenericApiService<ContactDto, CreateContactDto, UpdateContactDto>, IContactApiService
    {
        public ContactApiService(HttpClient httpClient/*, string endpoint*/) : base(httpClient, "contacts")
        {
        }

        public async Task<UpdateContactDto> GetContactForEditAsync()
        {
            var list = await GetAllAsync();
            var query = list.FirstOrDefault();
            if (query == null)
            {
                return new UpdateContactDto();
            }
            return query.Adapt<UpdateContactDto>();
        }

        public async Task SaveContactAsync(UpdateContactDto updateContactDto)
        {
            var list = await GetAllAsync();
            var query = list.FirstOrDefault();
            if (query == null)
            {
                var create = updateContactDto.Adapt<CreateContactDto>();
                await AddAsync(create);
            }
            else
            {
                await UpdateAsync(query.Id, updateContactDto);
            }
        }
    }
}
