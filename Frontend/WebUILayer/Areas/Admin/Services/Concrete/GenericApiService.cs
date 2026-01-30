using WebUILayer.Areas.Admin.Services.Abstract;

namespace WebUILayer.Areas.Admin.Services.Concrete
{
    public class GenericApiService<TListDto, TCreateDto, TUpdateDto> : IGenericApiService<TListDto, TCreateDto, TUpdateDto> where TListDto: class
    {

        protected readonly HttpClient _httpClient;
        protected readonly string _endpoint;

        public GenericApiService(HttpClient httpClient, string endpoint)
        {
            _httpClient = httpClient;
            _endpoint = endpoint;
        }

        public virtual async Task<TListDto?> AddAsync(TCreateDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync(_endpoint, dto);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }
            return await response.Content.ReadFromJsonAsync<TListDto>();
        }

        public async Task DeleteAsync(Guid guid)
        {
            await _httpClient.DeleteAsync($"{_endpoint}/{guid}");
        }

        public virtual async Task<List<TListDto>> GetAllAsync()
        {

            var response = await _httpClient.GetAsync(_endpoint);
            if (!response.IsSuccessStatusCode)
            {
                return new List<TListDto>();
            }
            var query = await response.Content.ReadFromJsonAsync<List<TListDto>>();
            if (query == null)
            {
                return new List<TListDto>();
            }
            return query;
            
        }

        public virtual async Task<TListDto?> GetByIdAsync(Guid guid)
        {
            return await _httpClient.GetFromJsonAsync<TListDto>($"{_endpoint}/{guid}");
        }


        public virtual async Task<TListDto?> UpdateAsync(Guid guid, TUpdateDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_endpoint}/{guid}", dto);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();

                throw new Exception(error) ;
               
            }
            return await response.Content.ReadFromJsonAsync<TListDto>();



        }
    }
}
