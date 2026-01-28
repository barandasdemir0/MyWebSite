using CV.EntityLayer.Entities;
using DtoLayer.CertificateDto;
using DtoLayer.EducationDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract
{
    public interface IEducationService:IGenericService<Education,EducationDto,CreateEducationDto,UpdateEducationDto>
    {
        Task<EducationDto?> RestoreAsync(Guid guid);
    }
}
