using DtoLayer.CertificateDtos;
using WebUILayer.Areas.Admin.Services.Abstract;

namespace WebUILayer.Areas.Admin.Services.Concrete
{
    public class CertificateApiService : GenericApiService<CertificateDto, CreateCertificateDto, UpdateCertificateDto>, ICertificateApiService
    {
        public CertificateApiService(HttpClient httpClient/*, string endpoint*/) : base(httpClient, "certificates")
        {
        }

        public async Task<List<CertificateDto>> GetAdminAllAsync()
        {
            var query = await _httpClient.GetFromJsonAsync<List<CertificateDto>>($"{_endpoint}/admin-all");
            if (query == null)
            {
                return new List<CertificateDto>();
            }
            return query;
        }

        public async Task RestoreAsync(Guid guid)
        {
            var response = await _httpClient.PutAsync($"{_endpoint}/restore/{guid}", null);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }
        }
    }
}
