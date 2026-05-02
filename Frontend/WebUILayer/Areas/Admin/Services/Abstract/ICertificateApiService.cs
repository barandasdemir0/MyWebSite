using DtoLayer.CertificateDtos;

namespace WebUILayer.Areas.Admin.Services.Abstract
{
    public interface ICertificateApiService:IGenericApiService<CertificateDto,CreateCertificateDto,UpdateCertificateDto>
    {
        Task<List<CertificateDto>> GetAllAdminAsync();
        Task RestoreAsync(Guid guid);
    }
}
