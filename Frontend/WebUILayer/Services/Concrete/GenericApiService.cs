using WebUILayer.Services.Abstract;

namespace WebUILayer.Services.Concrete
{
    public class GenericApiService<TListDto, TCreateDto, TUpdateDto> : IGenericApiService<TListDto, TCreateDto, TUpdateDto>
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
            return await response.Content.ReadFromJsonAsync<TListDto>();
        }

        public async Task DeleteAsync(Guid guid)
        {
            await _httpClient.DeleteAsync($"{_endpoint}/{guid}");
        }

        public virtual async Task<List<TListDto>> GetAllAsync()
        {
            var query = await _httpClient.GetFromJsonAsync<List<TListDto>>(_endpoint);
            if (query != null)
            {
                return query;
            }
            return new List<TListDto>();
        }

        public virtual async Task<TListDto?> GetByIdAsync(Guid guid)
        {
            return await _httpClient.GetFromJsonAsync<TListDto>($"{_endpoint}/{guid}");
        }


        public virtual async Task<TListDto?> UpdateAsync(Guid guid, TUpdateDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_endpoint}/{guid}", dto);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<TListDto>();
            }

            return default;


        }
    }
}
