using CV.EntityLayer.Entities;
using DtoLayer.BlogpostDto;
using DtoLayer.CertificateDto;
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
