using CV.EntityLayer.Entities;
using DtoLayer.CertificateDtos;
using DtoLayer.EducationDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract
{
    public interface IEducationService:IGenericService<Education,EducationDto,CreateEducationDto,UpdateEducationDto>
    {
        Task<EducationDto?> RestoreAsync(Guid guid);
        Task<List<EducationDto>> GetAllAdminAsync();
    }
}
