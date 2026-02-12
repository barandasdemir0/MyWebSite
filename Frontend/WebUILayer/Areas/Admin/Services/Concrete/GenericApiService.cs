using WebUILayer.Areas.Admin.Services.Abstract;

namespace WebUILayer.Areas.Admin.Services.Concrete
{
    public class GenericApiService<TListDto, TCreateDto, TUpdateDto> : IGenericApiService<TListDto, TCreateDto, TUpdateDto> where TListDto: class
    {

        protected readonly HttpClient _httpClient; // HTTP istekleri için
        protected readonly string _endpoint;  // API URL'si

        public GenericApiService(HttpClient httpClient, string endpoint)
        {
            _httpClient = httpClient;
            _endpoint = endpoint;  // Örn: "topics", "jobskillcategories"
        }

        //virtual → Alt sınıflarda override edilebilir.Mesela TopicApiService'de GetAllAdminAsync gibi ek metotlar var ama ihtiyaç olursa GetAllAsync'i de değiştirebilir.

        public virtual async Task<TListDto?> AddAsync(TCreateDto dto)
        {
            // POST isteği gönder (DTO'yu JSON olarak body'de)
            var response = await _httpClient.PostAsJsonAsync(_endpoint, dto);
            // HTTP: POST https://localhost:5001/api/topics
            // Body: {"topicName":"Frontend","topicDescription":"..."}

            //  Hata varsa exception fırlat
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);// MVC Controller catch'te yakalayacak
            }

            //  API'nin döndürdüğü sonucu oku (CreatedAtAction'dan gelen)
            return await response.Content.ReadFromJsonAsync<TListDto>();
        }

        public async Task DeleteAsync(Guid guid)
        {
            // DELETE https://localhost:5001/api/topics/abc-123
            var response =  await _httpClient.DeleteAsync($"{_endpoint}/{guid}");
            // Sonuç döndürmüyor, sadece silme komutu gönderiyor
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }
        }

        public virtual async Task<List<TListDto>> GetAllAsync()
        {
            //  API'ye GET isteği gönder
            var response = await _httpClient.GetAsync(_endpoint);
            // HTTP: GET https://localhost:5001/api/topics

            // Başarısızsa boş liste dön
            if (!response.IsSuccessStatusCode)
            {
                return new List<TListDto>();
            }

            // JSON'ı C# nesnesine dönüştür
            var query = await response.Content.ReadFromJsonAsync<List<TListDto>>();
            // JSON: [{"id":"abc","topicName":"Frontend"},{"id":"def","topicName":"Backend"}]
            // C#:   List<TopicDto> { TopicDto{...}, TopicDto{...} }

            // Null kontrolü
            if (query == null)
            {
                return new List<TListDto>();
            }
            return query;
            
        }

        public virtual async Task<TListDto?> GetByIdAsync(Guid guid)
        {
            //return await _httpClient.GetFromJsonAsync<TListDto>($"{_endpoint}/{guid}"); --> bu eski
            var response = await _httpClient.GetAsync($"{_endpoint}/{guid}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            return await response.Content.ReadFromJsonAsync<TListDto>();
        }


        public virtual async Task<TListDto?> UpdateAsync(Guid guid, TUpdateDto dto)
        {
            // PUT https://localhost:5001/api/topics/abc-123
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
