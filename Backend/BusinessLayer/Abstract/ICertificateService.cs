using CV.EntityLayer.Entities;
using DtoLayer.BlogpostDtos;
using DtoLayer.CertificateDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract
{
    public interface ICertificateService:IGenericService<Certificate,CertificateDto,CreateCertificateDto,UpdateCertificateDto>
    {
        Task<CertificateDto?> RestoreAsync(Guid guid);
        Task<List<CertificateDto>> GetAllAdminAsync();
    }
}
