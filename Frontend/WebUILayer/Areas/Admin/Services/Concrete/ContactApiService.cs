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
            var response = await _httpClient.GetAsync($"{_endpoint}/single");
            if (!response.IsSuccessStatusCode)
            {
                return new UpdateContactDto();
            }
            var query = await response.Content.ReadFromJsonAsync<ContactDto>();
            if (query == null)
            {
                return new UpdateContactDto();
            }
            return query.Adapt<UpdateContactDto>();
        }

        public async Task SaveContactAsync(UpdateContactDto updateContactDto)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_endpoint}/save", updateContactDto);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }
        }


        #region eski kodlar
        //public async Task<UpdateContactDto> GetContactForEditAsync()
        //{
        //    var list = await GetAllAsync();
        //    var query = list.FirstOrDefault();
        //    if (query == null)
        //    {
        //        return new UpdateContactDto();
        //    }
        //    return query.Adapt<UpdateContactDto>();
        //}

        //public async Task SaveContactAsync(UpdateContactDto updateContactDto)
        //{
        //    var list = await GetAllAsync();
        //    var query = list.FirstOrDefault();
        //    if (query == null)
        //    {
        //        var create = updateContactDto.Adapt<CreateContactDto>();
        //        await AddAsync(create);
        //    }
        //    else
        //    {
        //        await UpdateAsync(query.Id, updateContactDto);
        //    }
        //}
        #endregion
    }
}
