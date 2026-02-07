using DtoLayer.CertificateDtos;

namespace WebUILayer.Areas.Admin.Services.Abstract
{
    public interface ICertificateApiService:IGenericApiService<CertificateDto,CreateCertificateDto,UpdateCertificateDto>
    {
        Task<List<CertificateDto>> GetAdminAllAsync();
        Task RestoreAsync(Guid guid);
    }
}
