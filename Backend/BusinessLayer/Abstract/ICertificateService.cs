using CV.EntityLayer.Entities;
using DtoLayer.CertificateDtos;

namespace BusinessLayer.Abstract;

public interface ICertificateService : IGenericService<Certificate, CertificateDto, CreateCertificateDto, UpdateCertificateDto>
{
    Task<CertificateDto?> RestoreAsync(Guid guid, CancellationToken cancellationToken = default);
    Task<List<CertificateDto>> GetAllAdminAsync(CancellationToken cancellationToken = default);
}
