using DtoLayer.CertificateDtos;
using WebUILayer.Services.Abstract;

namespace WebUILayer.Services.Concrete;

public class PublicCertificateApiService : PublicReadApiService<CertificateDto>, IPublicCertificateApiService
{
    public PublicCertificateApiService(HttpClient httpClient/*, string endpoint*/) : base(httpClient, "certificates")
    {
    }
}
