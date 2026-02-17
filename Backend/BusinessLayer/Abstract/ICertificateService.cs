using CV.EntityLayer.Entities;
using DtoLayer.BlogPostDtos;
using DtoLayer.CertificateDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract;

public interface ICertificateService : IGenericService<Certificate, CertificateDto, CreateCertificateDto, UpdateCertificateDto>
{
    Task<CertificateDto?> RestoreAsync(Guid guid, CancellationToken cancellationToken = default);
    Task<List<CertificateDto>> GetAllAdminAsync(CancellationToken cancellationToken = default);
}
